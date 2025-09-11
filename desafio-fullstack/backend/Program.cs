using System.Text.Json;
using LeadsApi.Data;
using LeadsApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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
        p.WithOrigins("http://localhost:5173") // porta do Vite
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Spa");
app.MapControllers();

app.Run();
