using System.ComponentModel.DataAnnotations;
using UserManagementApi.Middleware;
using UserManagementApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Colección simulada en memoria
var users = new List<User>
{
    new User { Id = 1, Name = "Ana García", Email = "ana.garcia@example.com", Age = 25 },
    new User { Id = 2, Name = "Carlos López", Email = "carlos.lopez@example.com", Age = 32 }
};

// GET ALL: Listar todos los usuarios
app.MapGet("/api/users", () => Results.Ok(users))
   .WithName("GetAllUsers")
   .Produces<List<User>>(StatusCodes.Status200OK);

// GET BY ID: Obtener usuario por ID
app.MapGet("/api/users/{id:int}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null)
    {
        return Results.NotFound(new { message = $"Usuario con ID {id} no fue encontrado." });
    }
    return Results.Ok(user);
})
.WithName("GetUserById")
.Produces<User>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// POST: Crear nuevo usuario con validación (Criterio: Validación - 5 pts)
app.MapPost("/api/users", (User newUser) =>
{
    // Validación de anotaciones del modelo
    var validationContext = new ValidationContext(newUser);
    var validationResults = new List<ValidationResult>();

    if (!Validator.TryValidateObject(newUser, validationContext, validationResults, true))
    {
        var errors = validationResults.Select(r => r.ErrorMessage).ToList();
        return Results.BadRequest(new { errors });
    }

    // Regla de unicidad de email depurada con asistencia de Copilot
    if (users.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
    {
        return Results.BadRequest(new { message = "Ya existe un usuario registrado con este correo electrónico." });
    }

    newUser.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
    users.Add(newUser);

    return Results.Created($"/api/users/{newUser.Id}", newUser);
})
.WithName("CreateUser")
.Produces<User>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

// PUT: Actualizar usuario existente
app.MapPut("/api/users/{id:int}", (int id, User updatedUser) =>
{
    var existingUser = users.FirstOrDefault(u => u.Id == id);
    if (existingUser is null)
    {
        return Results.NotFound(new { message = $"Usuario con ID {id} no encontrado." });
    }

    // Validación de datos
    var validationContext = new ValidationContext(updatedUser);
    var validationResults = new List<ValidationResult>();

    if (!Validator.TryValidateObject(updatedUser, validationContext, validationResults, true))
    {
        var errors = validationResults.Select(r => r.ErrorMessage).ToList();
        return Results.BadRequest(new { errors });
    }

    existingUser.Name = updatedUser.Name;
    existingUser.Email = updatedUser.Email;
    existingUser.Age = updatedUser.Age;

    return Results.Ok(existingUser);
})
.WithName("UpdateUser")
.Produces<User>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

// DELETE: Eliminar usuario
app.MapDelete("/api/users/{id:int}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null)
    {
        return Results.NotFound(new { message = $"Usuario con ID {id} no encontrado." });
    }

    users.Remove(user);
    return Results.NoContent();
})
.WithName("DeleteUser")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();

