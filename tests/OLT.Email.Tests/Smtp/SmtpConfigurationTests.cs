using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Tests.Smtp
{
    [Obsolete]
    public class SmtpConfigurationTests
    {
        public class AppSettingsJsonDto
        {
            public OltSmtpConfiguration SmtpEmailConfig { get; set; } = new OltSmtpConfiguration();
        }


        //private readonly OltEmailConfiguration _emailConfiguration;
        private readonly string FakeHost = Faker.Internet.DomainName();

        //public SmtpConfigurationTests(IOptions<OltSmtpConfiguration> options)
        //{
        //    _emailConfiguration = options.Value;
        //}


        [Fact]
        [Obsolete]
        public void OltSmtpConfigurationTests()
        {
            var smtpServer = new OltSmtpServer
            {
                Host = FakeHost,
                Port = null,
                Credentials = null
            };


            var smtpEmail = SmtpHelper.FakerSmtpEmail(8, 6);
            var whiteList = new List<IOltEmailAddress>() { smtpEmail.Recipients.To[1], smtpEmail.Recipients.To[2], smtpEmail.Recipients.CarbonCopy[0], smtpEmail.Recipients.CarbonCopy[2] };


            var configuration = new OltSmtpConfiguration
            {
                Smtp = smtpServer,
                Production = false,
                From = (OltEmailAddress)SmtpHelper.FakerEmailAddress(),
                TestWhitelist = SmtpHelper.BuildWhitelist(whiteList)
            };

            var args = OltSmtpEmailExtensions.BuildOltEmailClient(configuration, smtpEmail);
            smtpEmail.Recipients.To.ForEach(rec =>
            {
                var expected = whiteList.Any(p => p.Email == rec.Email);
                Assert.Equal(expected, args.AllowSend(rec.Email));
            });
            smtpEmail.Recipients.CarbonCopy.ForEach(rec =>
            {
                var expected = whiteList.Any(p => p.Email == rec.Email);
                Assert.Equal(expected, args.AllowSend(rec.Email));
            });



        }

        [Fact]
        [Obsolete]
        public void OltSmtpServerTests()
        {
            var host = Faker.Internet.DomainName();
            var port = Convert.ToInt16(Faker.RandomNumber.Next(1, short.MaxValue));
            var username = Faker.Internet.UserName();
            var password = Faker.Lorem.GetFirstWord();

            var server = new OltSmtpServer();

            Assert.NotNull(server.Credentials);
            Assert.Null(server.Port);
            Assert.False(server.DisableSsl);

            server.Host = host;
            server.Port = port;
            server.DisableSsl = true;
            server.Credentials.Username = username;
            server.Credentials.Password = password;

            Assert.Equal(host, server.Host);
            Assert.Equal(port, server.Port);
            Assert.True(server.DisableSsl);
            Assert.Equal(username, server.Credentials.Username);
            Assert.Equal(password, server.Credentials.Password);
        }

        //[Fact]
        //public async Task OptionsTests()
        //{
        //    Assert.NotNull(_emailConfiguration);

        //    string fileName = "appsettings.json";
        //    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

        //    using (FileStream openStream = File.OpenRead(filePath))
        //    {
        //        AppSettingsJsonDto expectedConfig = await JsonSerializer.DeserializeAsync<AppSettingsJsonDto>(openStream);
        //        Assert.NotNull(expectedConfig);
        //        _emailConfiguration.Should().BeEquivalentTo(expectedConfig?.SmtpEmailConfig);
        //    }

        //}

    }
}