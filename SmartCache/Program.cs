using SmartCache.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IEmailsService, OrleansEmailsService>();

builder.Host.UseOrleans(static siloBuilder =>
{
    siloBuilder.AddMemoryGrainStorage("emails"); // Locally for now 
    siloBuilder.UseLocalhostClustering();
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();