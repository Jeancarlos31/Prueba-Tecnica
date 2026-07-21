using Microsoft.EntityFrameworkCore;
using ProductosApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Controladores + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Productos API",
        Version = "v1",
        Description = "Prueba técnica: API REST de gestión de productos (CRUD) con ASP.NET Core y SQL Server."
    });
});

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS para permitir que el frontend Angular (http://localhost:4200) consuma el API
const string AngularCorsPolicy = "AngularCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularCorsPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Productos API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors(AngularCorsPolicy);
app.UseAuthorization();
app.MapControllers();

// Crea la base de datos (si no existe) y aplica los datos semilla definidos
// en AppDbContext.OnModelCreating. Se usa EnsureCreated en lugar de
// migraciones para simplificar la ejecución de esta prueba técnica.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
