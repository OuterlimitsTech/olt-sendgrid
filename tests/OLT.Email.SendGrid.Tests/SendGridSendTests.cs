//using FluentAssertions;
//using Microsoft.Extensions.Options;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using OLT.Email.SendGrid.Common;
//using OLT.Email.SendGrid.Tests.Assets;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Threading.Tasks;
//using Xunit;

//namespace OLT.Email.SendGrid.Tests
//{

//    public class SendGridSendTests
//    {
//        private readonly SendGridProductionConfiguration _prodConfig;

//        public SendGridSendTests(IOptions<SendGridProductionConfiguration> options)
//        {
//            _prodConfig = options.Value;
//        }

//        [Fact]
//        public async Task SandBoxMode()
//        {
//            var firstName = Faker.Name.First();
//            var fullName = $"{firstName} Unit Test";
//            var template = JsonEmailTemplate.FakerData(_prodConfig.TemplateIdJson);
//            template.Recipients.To.Add(new OltEmailAddress(_prodConfig.ToEmail, fullName));
//            template.TemplateData.Recipient.First = firstName;
//            template.TemplateData.Recipient.FullName = fullName;

//            var result = await OltEmailSendGridExtensions.BuildOltEmailClient(_prodConfig, template)
//                .EnableSandbox()
//                .EnableProductionEnvironment(true)
//                .SendAsync(true);


//            result.Should().NotBeNull();
//            result.Errors.Should().BeEmpty();
//        }

//        [Fact]
//        public async Task SendJsonEmail()
//        {            
//            Assert.NotNull(_prodConfig.ApiKey);

//            var firstName = Faker.Name.First();
//            var fullName = $"{firstName} Unit Test";

//            var template = JsonEmailTemplate.FakerData(_prodConfig.TemplateIdJson);
//            template.Recipients.To.Add(new OltEmailAddress(_prodConfig.ToEmail, fullName));
//            template.Recipients.To.Add(new OltEmailAddress(Faker.Internet.Email(), $"Unit Test {Faker.Name.Last()}"));

            
//            template.TemplateData.Recipient.First = firstName;
//            template.TemplateData.Recipient.FullName = fullName;
//            var uid = Guid.NewGuid().ToString();

//            template.TemplateData.Build.Version = $"{_prodConfig.RunNumber}.Sync";

//            var result = OltEmailSendGridExtensions.BuildOltEmailClient(_prodConfig, template)
//                .WithCustomArg("email_uid", uid)
//                .Send(true);

//            Assert.True(result.Success);

//            template.TemplateData.Build.Version = $"{_prodConfig.RunNumber}.Async";

//            var resultAsync = await OltEmailSendGridExtensions.BuildOltEmailClient(_prodConfig, template)
//                .WithCustomArg("email_uid", uid)
//                .SendAsync(true);

//            Assert.True(resultAsync.Success);

//            template.TemplateData.Build.Version = $"{_prodConfig.RunNumber}.Manually (with JSON Data)";

//            var whitelist = new OltEmailConfigurationWhitelist 
//            {
//                Email =  _prodConfig.ToEmail,
//                Domain = Faker.Internet.DomainName(),
//            };

//            var recipients = new OltEmailRecipients 
//            { 
//                To = new List<IOltEmailAddress> 
//                { 
//                    new OltEmailAddress 
//                    { 
//                        Name = Faker.Name.FullName(),
//                        Email = _prodConfig.ToEmail 
//                    } 
//                },
//                CarbonCopy = new List<IOltEmailAddress> 
//                {
//                    new OltEmailAddress
//                    {
//                        Name = Faker.Name.FullName(),
//                        Email = Faker.Internet.Email()
//                    }
//                }
//            };

//            var client = new OltSendGridClient()
//                .WithFromEmail(_prodConfig.From)
//                .WithApiKey(_prodConfig.ApiKey)
//                .WithWhitelist(whitelist)
//                .WithTemplate(template.TemplateId, template.GetTemplateData())
//                .WithRecipients(recipients)
//                .WithCustomArg("email_uid", uid)
//                .WithCustomArg("some_internal_id", Faker.Identification.UkNhsNumber())
//                //false will only send to whitelist domains or email address
//                //true will ignore whitelist and send all emails.
//                //typically controlled via config/environment variable
//                .EnableProductionEnvironment(false);

//            resultAsync = await client.SendAsync(true);

//            Assert.True(resultAsync.Success);
                        

//            client = new OltSendGridClient()
//                .WithFromEmail(_prodConfig.From)
//                .WithApiKey(_prodConfig.ApiKey)
//                .WithWhitelist(whitelist)
//                .WithTemplate(_prodConfig.TemplateIdNoData)
//                .WithRecipients(recipients)
//                .WithCustomArg("email_uid", uid)
//                .WithCustomArg("some_internal_id", Faker.Identification.UkNhsNumber())
//                .EnableProductionEnvironment(false);

//            resultAsync = await client.SendAsync(true);

//            Assert.True(resultAsync.Success);
//        }

//        [Fact]
//        public async Task SendTagEmail()
//        {
//            Assert.NotNull(_prodConfig.ApiKey);

//            var firstName = Faker.Name.First();
//            var fullName = $"{firstName} Unit Test";

//            var template = TagEmailTemplate.FakerData(_prodConfig.TemplateIdTag);
//            template.Recipients.To.Add(new OltEmailAddress(_prodConfig.ToEmail, fullName));
//            template.Recipients.To.Add(new OltEmailAddress(Faker.Internet.Email(), $"Unit Test {Faker.Name.Last()}"));

//            template.Build.Version = _prodConfig.RunNumber;
//            template.Recipient.First = firstName;
//            template.Recipient.FullName = fullName;

//            var args = OltEmailSendGridExtensions.BuildOltEmailClient(_prodConfig, template).WithCustomArg("email_uid", Guid.NewGuid().ToString());

//            if (_prodConfig.UnsubscribeGroupId > 0)
//            {
//                args = args.WithUnsubscribeGroupId(_prodConfig.UnsubscribeGroupId.Value);
//            }

//            var resultAsync = await args.SendAsync(true);

//            Assert.True(resultAsync.Success);
//        }


//        [Fact]
//        public void ApplicationErrorEmailTests()
//        {
//            Assert.NotNull(_prodConfig.ApiKey);

//            var exceptionMessage = Faker.Lorem.Paragraph(10);
//            var appName = $"SendGrid Unit Test of {nameof(ApplicationErrorEmailTests)}";
//            var environment = Faker.Company.Name();
//            var email = Faker.Internet.Email();
//            var ex = new Exception(exceptionMessage);


//            var smtpEmail = new OltApplicationErrorEmail
//            {
//                Subject = Faker.Lorem.Words(34).Last(),
//                Body = Faker.Lorem.Paragraph(4),
//                AppName = appName,
//                Environment = environment,
//                From = new OltEmailAddress
//                {
//                    Name = Faker.Name.FullName(),
//                    Email = Faker.Internet.Email()
//                },
//                Recipients = new OltEmailRecipients
//                {
//                    To = new List<IOltEmailAddress>
//                    {
//                        new OltEmailAddress(email)
//                    }
//                }
//            };

//            Assert.Throws<Exception>(() => OltEmailSendGridSmtpExtensions.OltEmailError(ex, _prodConfig.ApiKey, smtpEmail, true)); //SENDS EMAIL
//            try
//            {
//                OltEmailSendGridSmtpExtensions.OltEmailError(ex, _prodConfig.ApiKey, smtpEmail, false); //SENDS EMAIL
//                Assert.True(true);
//            }
//            catch
//            {
//                Assert.True(false);
//            }
//        }



//        [Fact]
//        public async Task ExtensionsExceptionsTest()
//        {
//            var config = new OltEmailConfigurationSendGrid();
//            var template = JsonEmailTemplate.FakerData(_prodConfig.TemplateIdJson);

//            Assert.Throws<ArgumentNullException>(() => OltEmailSendGridExtensions.BuildOltEmailClient(config, template).Send(true));  //SHOULD FAIL
//            await Assert.ThrowsAsync<ArgumentNullException>(() => OltEmailSendGridExtensions.BuildOltEmailClient(config, template).SendAsync(true));  //SHOULD FAIL

//            var noRecTemplate = JsonEmailTemplate.FakerData(_prodConfig.TemplateIdJson);
//            noRecTemplate.Recipients = new OltEmailRecipients();
//            Assert.Throws<OltSendGridValidationException>(() => OltEmailSendGridExtensions.BuildOltEmailClient(_prodConfig, noRecTemplate).Send(true));  //SHOULD FAIL
//            await Assert.ThrowsAsync<OltSendGridValidationException>(() => OltEmailSendGridExtensions.BuildOltEmailClient(_prodConfig, noRecTemplate).SendAsync(true));  //SHOULD FAIL
//        }

//        [Fact]
//        public async Task SendExceptionTest()
//        {
//            Assert.NotNull(_prodConfig.ApiKey);

//            var firstName = Faker.Name.First();
//            var fullName = $"{firstName} Unit Test";
            
//            var fakeTemplate = FakeNoTemplateDataTemplate.FakerData(0, 0);
//            fakeTemplate.Recipients.To.Add(new OltEmailAddress(_prodConfig.ToEmail, fullName));
//            fakeTemplate.Recipients.To.Add(new OltEmailAddress(Faker.Internet.Email(), $"Unit Test {Faker.Name.Last()}"));

//            //Invalid Template
//            var args = OltEmailSendGridExtensions.BuildOltEmailClient(_prodConfig, fakeTemplate);

//            var result = await args.SendAsync(false);
//            Assert.NotEmpty(result.Errors);

//            try
//            {
//                result = await args.SendAsync(true);  //Should not throw exception as all errors are from send grid
//            }
//            catch
//            {
//                throw;
//            }
            

//            var config = new OltEmailConfigurationSendGrid
//            {
//                From = _prodConfig.From,
//                Production = _prodConfig.Production,
//                TestWhitelist = _prodConfig.TestWhitelist,
//                ApiKey = "FakerAPIKey",
//            };

//            var jsonTemplate = TagEmailTemplate.FakerData(_prodConfig.TemplateIdTag);
//            jsonTemplate.Build.Version = _prodConfig.RunNumber;
//            jsonTemplate.Recipient.First = firstName;
//            jsonTemplate.Recipient.FullName = fullName;

//            //Invalid API Key
//            args = OltEmailSendGridExtensions.BuildOltEmailClient(config, jsonTemplate);

            
//            var noRecipientsValidationException = await Assert.ThrowsAsync<OltEmailNoRecipientsValidationException>(() => args.SendAsync(true));
//            Assert.Equal("No Recipients were attached.  This can be caused by skipping due to the whitelist", noRecipientsValidationException.Message);


//            result = await args.SendAsync(false);
//            Assert.NotEmpty(result.Errors);
//            result.Errors.Where(p => p.StartsWith("OLT.Email.OltEmailNoRecipientsValidationException: No Recipients were attached.  This can be caused by skipping due to the whitelist"))
//                .Should()
//                .NotBeEmpty()
//                .And
//                .HaveCount(1);
//        }
//    }
//}
