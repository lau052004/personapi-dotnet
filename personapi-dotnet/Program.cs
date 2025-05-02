using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// 1) Registrar el DbContext con la cadena "DefaultConnection"
builder.Services.AddDbContext<PersonaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Registrar solo controllers de API
builder.Services.AddControllers();

var app = builder.Build();

// 3) Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 4) Mapear rutas de API
app.MapControllers();

// (Opcional) Mantener rutas MVC si también tienes vistas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
