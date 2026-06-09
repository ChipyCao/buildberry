using Buildberry.Models;
using Buildberry.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<IBuildsService, BuildsService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// API Endpoints
var api = app.MapGroup("/api")
    .WithOpenApi();

api.MapGet("/builds", GetBuilds)
    .WithName("GetBuilds")
    .WithOpenApi()
    .WithDescription("Retrieve GitHub Actions workflow runs");

app.Run();

// Endpoint handler
async Task<IEnumerable<WorkflowRun>> GetBuilds(IBuildsService buildsService)
{
    return await buildsService.GetBuildsAsync();
}
