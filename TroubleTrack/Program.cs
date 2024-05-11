using Microsoft.OpenApi.Models;
using TroubleTrack.Database;
using TroubleTrack.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MongoDatabase>();
builder.Services.AddSingleton<ProjectsService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
