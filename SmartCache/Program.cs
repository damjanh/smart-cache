using System.Diagnostics;
using Azure.Identity;
using Azure.Storage.Blobs;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using SmartCache.Auth;
using SmartCache.Middleware;
using SmartCache.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IEmailsService, OrleansEmailsService>();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);
builder.Services.AddAuthorization();

builder.Host.UseOrleans(static siloBuilder =>
{
    //siloBuilder.AddMemoryGrainStorage("emails"); // Locally for now 
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddAzureBlobGrainStorage("emails", options =>
    {
        string? serviceUrl = Environment.GetEnvironmentVariable("BLOB_SERVICE_URL");
        Debug.Assert(serviceUrl != null);
        var serviceUri = new Uri(serviceUrl);
        options.BlobServiceClient = new BlobServiceClient(serviceUri, new DefaultAzureCredential());
        options.ContainerName = "emails"; // Blob container name
    });
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