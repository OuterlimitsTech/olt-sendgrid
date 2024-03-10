using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OLT.Email.Tests.Smtp.Assets;

namespace OLT.Email.Tests
{
    [Obsolete]
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureHostConfiguration(builder =>
            {
                builder
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", true)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<Startup>();
            });
        }

        public virtual void ConfigureServices(IServiceCollection services, HostBuilderContext hostBuilderContext)
        {
            var configuration = hostBuilderContext.Configuration;

            services.Configure<OltEmailConfiguration>(configuration.GetSection("EmailConfig"));
            services.Configure<OltSmtpConfiguration>(configuration.GetSection("SmtpEmailConfig"));



            var portConfigValue = configuration.GetValue<string>("SMTP_PORT") ?? Environment.GetEnvironmentVariable("SMTP_PORT");
            short? portNumber = string.IsNullOrEmpty(portConfigValue) ? null : Convert.ToInt16(portConfigValue);

            var smtpTestServer = new OltSmtpServer
            {
                Host = configuration.GetValue<string>("SMTP_HOST") ?? Environment.GetEnvironmentVariable("SMTP_HOST"),
                Port = portNumber,
                DisableSsl = false,
                Credentials = new OltSmtpCredentials 
                {
                    Username = configuration.GetValue<string>("SMTP_USERNAME") ?? Environment.GetEnvironmentVariable("SMTP_USERNAME"),
                    Password = configuration.GetValue<string>("SMTP_PASSWORD") ?? Environment.GetEnvironmentVariable("SMTP_PASSWORD"),
                },                
            };


            services.Configure<OltSmtpServer>(opt =>
            {
                opt.Host = smtpTestServer.Host;
                opt.Port = smtpTestServer.Port;
                opt.DisableSsl = smtpTestServer.DisableSsl;
                opt.Credentials.Username = smtpTestServer.Credentials.Username;
                opt.Credentials.Password = smtpTestServer.Credentials.Password;                
            });

        }
    }
}
