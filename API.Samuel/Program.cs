using API.Samuel.DAPPER;
using API.Samuel.LOGICA.NEGOCIO;

var builder = WebApplication.CreateBuilder(args);

// Servicios

builder.Services.AddControllers();

builder.Services.AddScoped<InicioSesionBLL>();
builder.Services.AddScoped<InicioSesionDAL>();
builder.Services.AddScoped<ClientesBLL>();
builder.Services.AddScoped<ClientesDAL>();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodos", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Documentación OpenAPI activa también en producción: /openapi/v1.json
app.MapOpenApi();

app.UseCors("PermitirTodos");

app.UseAuthorization();

// Página de inicio: confirma que la API está en línea y muestra los endpoints
app.MapGet("/", () => Results.Ok(new
{
    api = "API.Samuel - Estudio Fotográfico Enfoque",
    estado = "En línea",
    endpoints = new[]
    {
        "POST   /api/InicioSesion",
        "GET    /api/Clientes",
        "GET    /api/Clientes/{id}",
        "POST   /api/Clientes",
        "PUT    /api/Clientes/{id}",
        "DELETE /api/Clientes/{id}"
    }
}));

app.MapControllers();

app.Run();
