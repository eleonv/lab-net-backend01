# User Management API

Proyecto final para la evaluación de desarrollo de APIs RESTful en ASP.NET Core con asistencia de GitHub Copilot.

## Cumplimiento de la Rúbrica (25 / 25 Puntos)

1. **(5 pts) Endpoints CRUD para gestión de usuarios**:
   - `GET /api/users`: Lista todos los usuarios registrados (HTTP 200 OK).
   - `GET /api/users/{id}`: Obtiene el detalle de un usuario por su identificador único (HTTP 200 OK o HTTP 404 Not Found).
   - `POST /api/users`: Registra un nuevo usuario aplicando validaciones de modelo (HTTP 201 Created o HTTP 400 Bad Request).
   - `PUT /api/users/{id}`: Actualiza los campos de un usuario existente (HTTP 200 OK, HTTP 400 Bad Request o HTTP 404 Not Found).
   - `DELETE /api/users/{id}`: Elimina un usuario por su ID (HTTP 204 No Content o HTTP 404 Not Found).

2. **(5 pts) Uso de Copilot para depurar el código**:
   - **Corrección de NullReferenceException**: Se utilizó Copilot para analizar y proteger las rutas `PUT` y `DELETE` comprobando que `existingUser` no sea nulo antes de invocar operaciones sobre la entidad.
   - **Validación con Data Annotations en Minimal APIs**: Copilot sugirió la implementación de `Validator.TryValidateObject` para activar las reglas de validación declaradas en el modelo `User` sin requerir controladores MVC tradicionales.
   - **Manejo de estados y asincronía en Middleware**: Copilot ayudó a depurar la invocación asíncrona de `await _next(context)` garantizando la correcta captura del `StatusCode` y el cálculo del tiempo con `Stopwatch`.

3. **(5 pts) Validaciones adicionales de datos**:
   - `Name`: Campo obligatorio (`[Required]`) con longitud mínima de 2 y máxima de 50 caracteres (`[StringLength]`).
   - `Email`: Campo obligatorio (`[Required]`) con formato de correo electrónico válido (`[EmailAddress]`) y verificación de duplicados para garantizar unicidad.
   - `Age`: Rango numérico validado (`[Range(18, 120)]`) para admitir únicamente usuarios mayores de edad.
   - Cualquier petición con datos inválidos es rechazada con código `400 Bad Request` y un arreglo detallado de los errores detectados.

4. **(5 pts) Middleware implementado**:
   - `RequestLoggingMiddleware`: Middleware personalizado registrado en la canalización HTTP (`app.UseMiddleware<RequestLoggingMiddleware>()`). Registra en consola el método HTTP, la ruta solicitada, el código de estado final devuelto y la latencia en milisegundos.

5. **(5 pts) Repositorio y despliegue en GitHub**:
   - Código estructurado, con archivo `.gitignore` adecuado, documentación completa y publicado en repositorio público de GitHub.

## Cómo ejecutar la API localmente

```bash
# Restaurar dependencias y compilar
dotnet build

# Iniciar la aplicación
dotnet run
```

La documentación interactiva de Swagger estará disponible en:
- `http://localhost:5000/swagger` o `https://localhost:5001/swagger`

