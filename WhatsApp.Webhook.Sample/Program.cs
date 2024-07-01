
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;
using WhatsApp.Webhook.API;
using WhatsApp.Webhook.API.Configuration;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Extensions;
using Microsoft.Extensions.Options;

namespace WhatsApp.Webhook.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<WebhookHandShakeOptions>(builder.Configuration.GetSection("WhatsApp:Webhook:Handshake"));
            builder.Services.Configure<WhatsAppOptions>(builder.Configuration.GetSection("WhatsApp"));

            var context = builder.Services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();
            builder.Services.AddSerilog(opt =>
            {
                opt.Enrich.FromLogContext();
                opt.WriteTo.File(Path.Combine(context.WebRootPath, "logs", "diagnostics-.txt"),
                    rollingInterval: RollingInterval.Minute,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    rollOnFileSizeLimit: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1));
            });


            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            var whatsAppOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<WhatsAppOptions>>().Value; 
            WhatsAppBusinessCloudApiConfig whatsAppConfig = new WhatsAppBusinessCloudApiConfig();
            whatsAppConfig.WhatsAppBusinessPhoneNumberId = whatsAppOptions.PhoneId;
            whatsAppConfig.WhatsAppBusinessAccountId = whatsAppOptions.AccountId;
            whatsAppConfig.WhatsAppBusinessId = whatsAppOptions.AppId;
            whatsAppConfig.AccessToken = whatsAppOptions.ApiKey;
            builder.Services.AddWhatsAppBusinessCloudApiService(whatsAppConfig);
            builder.Services.AddHttpClient<IWhatsAppBusinessClient, WhatsAppBusinessClient>(options => options.BaseAddress = WhatsAppBusinessRequestEndpoint.BaseAddress);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler();

            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseCors(opt =>
            {
                opt.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            });
            app.MapControllers();

            app.Run();
        }
    }
}
