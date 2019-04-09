using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWeb2019.Models;

namespace MyWeb2019
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var sqlConnection = GetSqlConnectionAsync();

            services.AddDbContext<AdventureWorksContext>(options =>
                options.UseSqlServer(sqlConnection,
                sqlServerOptionsAction: sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    sqlServerOptions.UseRowNumberForPaging();
                })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public SqlConnection GetSqlConnectionAsync()
        {
            string tenantId = !string.IsNullOrWhiteSpace(Configuration["TenantId"]) ? Configuration["TenantId"] :
                throw new ApplicationException($"ERROR: Missing value for configuration parameter called 'TenantId'");

            string dbServer = !string.IsNullOrWhiteSpace(Configuration["DbServerName"]) ? Configuration["DbServerName"] :
                throw new ApplicationException($"ERROR: Missing value for configuration parameter called 'DbServerName'");

            string dbName = !string.IsNullOrWhiteSpace(Configuration["DbName"]) ? Configuration["DbName"] :
                throw new ApplicationException($"ERROR: Missing value for configuration parameter called 'DbName'");

            var token = GetAccessToken(tenantId);

            var builder = new SqlConnectionStringBuilder();

            builder["Data Source"] = $"tcp:{dbServer}.database.windows.net,1433";
            builder["Initial Catalog"] = dbName;
            builder["Connect Timeout"] = 30;
            builder["Persist Security Info"] = false;
            builder["TrustServerCertificate"] = false;
            builder["Encrypt"] = true;
            builder["MultipleActiveResultSets"] = false;
            var connectionString = builder.ToString();

            var con = new SqlConnection(connectionString);

            con.AccessToken = token;

            return con;
        }

        public static string GetAccessToken(string tenantId)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            var result = azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/", tenantId).GetAwaiter().GetResult();

            return result;
        }

    }
}
