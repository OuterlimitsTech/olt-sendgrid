using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Tests.Smtp
{ 

    public class SmtpEmailClientTests
    {
        private readonly string FakeHost = Faker.Internet.DomainName();

        [Fact]
        [Obsolete]
        public void PortNumberTests()
        {

            var smtpEmail = SmtpHelper.FakerSmtpEmail(2, 2);

            var smtpServer = new OltSmtpServer
            {
                Host = FakeHost,
                Port = null,
                Credentials = null
            };

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).CreateClient())
            {
                Assert.True(client.Port > 0);
            }

            short? portNumber = 0;
            smtpServer.Port = portNumber;

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).CreateClient())
            {
                Assert.NotEqual(portNumber.Value, client.Port);
            }

            portNumber = 2525;
            smtpServer.Port = portNumber;

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).CreateClient())
            {
                Assert.Equal(portNumber.Value, client.Port);
            }

        }

        [Fact]
        [Obsolete]
        public void CredentialTests()
        {
            var smtpEmail = SmtpHelper.FakerSmtpEmail(2, 2);
            var smtpServer = new OltSmtpServer
            {
                Host = FakeHost,
                Credentials = new OltSmtpCredentials
                {
                    Username = null,
                    Password = null
                }
            };

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).CreateClient())
            {
                Assert.Null(client.Credentials);
            }

            smtpServer.Credentials.Username = null;
            smtpServer.Credentials.Password = Faker.Internet.DomainName();

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).CreateClient())
            {
                Assert.Null(client.Credentials);
            }


            smtpServer.Credentials.Username = Faker.Internet.UserName();
            smtpServer.Credentials.Password = null;

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).CreateClient())
            {
                Assert.Null(client.Credentials);
            }


            smtpServer.Credentials.Username = Faker.Internet.UserName();
            smtpServer.Credentials.Password = Faker.Internet.DomainName();

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).CreateClient())
            {
                Assert.NotNull(client.Credentials);
                var cred = (NetworkCredential)client.Credentials;
                Assert.Equal(smtpServer.Credentials.Username, cred.UserName);
                Assert.Equal(smtpServer.Credentials.Password, cred.Password);

            }

            smtpServer.Credentials = null;

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).CreateClient())
            {
                Assert.Null(client.Credentials);
            }
        }
    }
}
