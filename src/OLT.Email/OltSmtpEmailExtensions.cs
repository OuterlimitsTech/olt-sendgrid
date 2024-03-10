
using System;

namespace OLT.Email
{

    public static class OltSmtpEmailExtensions
    {

        /// <summary>
        /// Build whitelist compliant SMTP client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public static OltEmailClientSmtp BuildOltEmailClient<T>(this OltSmtpConfiguration configuration, T template)  where T : IOltSmtpEmail
        {
            return BuildOltEmailClient(configuration.Smtp, configuration.Production, template).WithWhitelist(configuration.TestWhitelist);
        }

        /// <summary>
        /// Build whitelist compliant SMTP client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="smtpServer"></param>
        /// <param name="enableProduction">Sends all email, otherwise email must be in whitelist</param>
        /// <param name="template"></param>
        /// <returns></returns>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public static OltEmailClientSmtp BuildOltEmailClient<T>(this OltSmtpServer smtpServer, bool enableProduction, T template) where T : IOltSmtpEmail
        {
            var args = new OltEmailClientSmtp()
                .WithFromEmail(template.From)
                .WithSmtpHost(smtpServer.Host)
                .WithBody(template.Body)
                .WithSubject(template.Subject)
                .WithRecipients(template.Recipients)
                .EnableProductionEnvironment(enableProduction);


            if (smtpServer.Credentials?.Username != null && smtpServer.Credentials.Password != null)
            {
                args.WithSmtpNetworkCredentials(smtpServer.Credentials.Username, smtpServer.Credentials.Password);
            }

            if (smtpServer.Port != null && smtpServer.Port > 0)
            {
                args.WithSmtpPort(smtpServer.Port.Value);
            }

            return args;
        }

        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public static OltEmailClientSmtp BuildOltEmailClient<T>(this OltSmtpServer server, T template, Exception ex) where T : IOltApplicationErrorEmail
        {            
            return BuildOltEmailClient(ex, server, template);
        }

        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public static OltEmailClientSmtp BuildOltEmailClient<T>(this Exception ex, OltSmtpServer server, T template) where T : IOltApplicationErrorEmail
        {
            return BuildOltEmailClient(server, true, template).WithAppError(ex, template.AppName, template.Environment);
        }


        /// <summary>
        /// Sends email with exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="server"></param>
        /// <param name="template"></param>
        /// <param name="rethrowException"></param>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public static void OltEmailError(this Exception ex, OltSmtpServer server, IOltApplicationErrorEmail template, bool rethrowException = false)
        {
            BuildOltEmailClient(ex, server, template).Send(false);
            if (rethrowException)
            {
                throw ex;
            }
        }

    }
}