using odm_api.Repositories;
using odm_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Forcer Kestrel à écouter sur toutes les interfaces réseau
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5104); //port
});

// Configuration de la base de données
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<MagasinRepository>();
builder.Services.AddScoped<SiteRepository>();
builder.Services.AddScoped<ExportateurRepository>();
builder.Services.AddScoped<LotRepository>();





builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();



