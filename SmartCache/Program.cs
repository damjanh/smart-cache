using Microsoft.AspNetCore.Authentication;
using SmartCache.Auth;
using SmartCache.Middleware;
using SmartCache.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IEmailsService, OrleansEmailsService>();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);
builder.Services.AddAuthorization();

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

app.UseMiddleware<EmailValidation>();
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();