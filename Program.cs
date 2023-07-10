using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Configuration;

namespace BulkSMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.EnableAnnotations();

                // Change Swagger Title
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bulk SMS", Version = "v1" });
            });

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // Add Hangfire Services
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration["ConnectionStrings:HangfireConnection"]));
            builder.Services.AddHangfireServer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c=>
                {
                    c.DefaultModelsExpandDepth(-1);
                });
            }

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                //AppPath = "" //The path for the Back To Site link. Set to null in order to hide the Back To  Site link.
                DashboardTitle = "VengaFi BulkSMS Gateway",
                Authorization = new[]
                {
                    new HangfireCustomBasicAuthenticationFilter{
                        User = builder.Configuration["Hangfire:Dashboard:Username"],
                        Pass = builder.Configuration["Hangfire:Dashboard:Password"]
                    }
                }
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();

            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });*/

            app.MapControllers();

            app.Run();
        }
    }
}