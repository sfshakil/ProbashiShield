using FluentValidation;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Domain.Services.Admin.Concretes;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using ProbashiShield.Domain.AutoMapperConfigs;
using System;
using System.Net.Http;

namespace ProbashiShield.Domain.ServiceExtensions
{
    public static class DomainServiceCollectionExtensions
    {
        public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(DomainMappingProfile));
            services.AddScoped<IAgencyDataSetsService, AgencyDataSetsService>();
            services.AddScoped<IDocumentsService, DocumentsService>();
            services.AddScoped<IOCRService, OCRService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IJobCategoriesService, JobCategoriesService>();
            services.AddScoped<ICountryFeeLimitsService, CountryFeeLimitsService>();
            services.AddScoped<ISalaryReferencesService, SalaryReferencesService>();

            services.AddTransient<IImporter, ExcelImporter>();
            services.AddTransient<IExportFileByTemplate, ExcelExporter>();

            services.AddHttpClient<IOllamaService, OllamaService>(
            client =>
            {
                string baseUrl = "http://localhost:11434";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMinutes(3); // 3 minutes
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, chain, policyErrors) =>
                {
                    return true;
                }
            })
            .AddPolicyHandler(GetRetryPolicy(configuration))
            .AddPolicyHandler(GetCircuitBreakerPolicy(configuration));
            
            services.AddHttpClient<IGeminiService, GeminiService>(
            client =>
            {
                string baseUrl = "https://generativelanguage.googleapis.com/v1beta/models";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMinutes(3); // 3 minutes
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, chain, policyErrors) =>
                {
                    return true;
                }
            })
            .AddPolicyHandler(GetRetryPolicy(configuration))
            .AddPolicyHandler(GetCircuitBreakerPolicy(configuration));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IConfiguration configuration)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(int.Parse(configuration["PollyConfig:MaxRetry"]), retryAttempt =>
                {
                    return TimeSpan.FromSeconds(15);
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IConfiguration configuration)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .CircuitBreakerAsync(int.Parse(configuration["PollyConfig:AllowedErrorCountBeforeCicuitBreaks"]), TimeSpan.FromSeconds(5));
        }

    }
}