using FluentAssertions;
using OLT.Email.SendGrid.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.SendGrid.Tests
{
    public class SendGridGeneralTests
    {
        [Fact]
        [Obsolete]
        public void ServerValues()
        {
            var password = Faker.Internet.UserName();
            var server = new OltSmtpServerSendGrid(password);
            short port = 587;

            Assert.Equal("smtp.sendgrid.net", server.Host);
            Assert.Equal(port, server.Port);
            Assert.False(server.DisableSsl);
            Assert.Equal("apikey", server.Credentials.Username);
            Assert.Equal(password, server.Credentials.Password);

            Assert.Throws<ArgumentNullException>(() => new OltSmtpServerSendGrid(null));
        }

        [Fact]
        [Obsolete]
        public void SmtpClient()
        {
            var password = Faker.Internet.UserName();
            var server = new OltSmtpServerSendGrid(password);
            var args = new OltEmailClientSmtpSendGrid().WithApiKey(password);

            using (var client = args.CreateClient())
            {
                Assert.Equal(server.Host, client.Host);
                Assert.Equal(server.Port.Value, client.Port);
                Assert.True(client.EnableSsl);
                Assert.Equal(password, server.Credentials.Password);

                var cred = (NetworkCredential)client.Credentials;
                Assert.Equal(server.Credentials.Username, cred.UserName);                
                Assert.Equal(password, cred.Password);
            }
        }

        [Fact]
        public void SendGridValidationExceptionTests()
        {
            var error1 = Faker.Internet.UserName();
            var error2 = Faker.Internet.DomainName();

            var errors = new List<string>() { error1, error2 };
            var ex = new OltSendGridValidationException(errors);
            
            Assert.Equal("SendGrid Validation Errors", ex.Message);
            Assert.NotEmpty(ex.Errors);
            ex.Errors.Should().BeEquivalentTo(errors);

        }

        [Fact]
        [Obsolete]
        public void EmailTemplateTests()
        {            
            var data = EmailDataJson.FakerData();
            var template = new FakeJsonEmailTemplate
            {
                TemplateData = data
            };

            var firstName1 = Faker.Name.First();
            var email1 = Faker.Internet.Email();

            var firstName2 = Faker.Name.First();
            var email2 = Faker.Internet.Email();

            var list = new List<OltEmailAddress>
            {
                new OltEmailAddress(email1, firstName1),
                new OltEmailAddress(email2, firstName2)
            };

            template.Recipients.To.Add(new OltEmailAddress(email1, firstName1));
            template.Recipients.To.Add(new OltEmailAddress(email2, firstName2));
            template.Recipients.To.Should().BeEquivalentTo(list);
            template.TemplateData.Should().BeEquivalentTo(data);
            Assert.Equal(nameof(FakeJsonEmailTemplate), template.TemplateId);
            template.GetTemplateData().Should().BeEquivalentTo(data);
        }
    }
}
