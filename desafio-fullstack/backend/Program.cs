using System.Text.Json;
using System.Text.Json.Serialization;
using LeadsApi.Data;
using LeadsApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IEmailFake, FileEmailFake>();


builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Spa", p =>
        p.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Spa");
app.MapControllers();

{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


    for (int i = 0; i < 10; i++)
    {
        try { await db.Database.MigrateAsync(); break; }
        catch { await Task.Delay(1000); }
    }

    await SeedHelper.EnsureSeedAsync(db, targetNew: 30);
}

await app.RunAsync();

public partial class Program { }