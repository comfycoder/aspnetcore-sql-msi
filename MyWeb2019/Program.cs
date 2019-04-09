using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Logging;

namespace MyWeb2019
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.Build();

                    var keyVaultName = builtConfig["KeyVaultName"];

                    if (!string.IsNullOrWhiteSpace(keyVaultName))
                    {
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();

                        var keyVaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(
                                azureServiceTokenProvider.KeyVaultTokenCallback));

                        var keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";

                        Debug.WriteLine($"Key Vault URI: {keyVaultUri}");

                        config.AddAzureKeyVault(
                            keyVaultUri,
                            keyVaultClient,
                            new DefaultKeyVaultSecretManager());
                    }
                })
                .UseStartup<Startup>();
    }
}
