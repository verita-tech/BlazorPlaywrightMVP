using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace BlazorApp.UITests.Infrastructure;

public class BlazorAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Verwende Testing-Konfiguration
            config.AddJsonFile("appsettings.Testing.json", optional: false);
        });

        builder.UseUrls("http://localhost:0"); // Random port
    }
}
