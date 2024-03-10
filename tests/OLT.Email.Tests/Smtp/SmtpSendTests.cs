//using FluentAssertions;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using OLT.Email.Tests.Smtp.Assets;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace OLT.Email.Tests.Smtp
//{
//    /// <summary>
//    /// BE CAREFUL.  The free tier for mailtrap limits the number of emails sent
//    /// </summary>
//    public class SmtpSendTests
//    {
//        private readonly OltSmtpServer _smtpTestServer;
//        private readonly IConfiguration _configuration;

//        public SmtpSendTests(
//            IConfiguration configuration,
//            IOptions<OltSmtpServer> options)
//        {
//            _configuration = configuration;
//            _smtpTestServer = options.Value;
//        }

//        [Fact]
//        public async Task SmtpEmailExtensionsExceptionsTest()
//        {
//            //Use invalid SMTP Server for this test
//            var smtpServer = new OltSmtpServer
//            {
//                Host = _smtpTestServer.Host,
//                Credentials = null
//            };

//            Assert.Throws<ArgumentNullException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, new OltSmtpEmail()).Send(true));  //SHOULD FAIL
//            await Assert.ThrowsAsync<ArgumentNullException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, new OltSmtpEmail()).SendAsync(true));  //SHOULD FAIL

//            var smtpEmail = new OltSmtpEmail
//            {
//                Subject = Faker.Lorem.GetFirstWord(),
//                Body = Faker.Lorem.Paragraph(4),
//                From = new OltEmailAddress
//                {
//                    Name = Faker.Name.FullName(),
//                    Email = Faker.Internet.Email()
//                },
//            };

//            Assert.Throws<OltEmailValidationException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).Send(true));  //SHOULD FAIL
//            await Assert.ThrowsAsync<OltEmailValidationException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).SendAsync(true));  //SHOULD FAIL


//            try
//            {
//                var result = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).Send(false);
//                Assert.NotEmpty(result.Errors);
//            }
//            catch
//            {
//                throw;
//            }

//            try
//            {
//                var result = await OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).SendAsync(false);
//                Assert.NotEmpty(result.Errors);
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        [Fact]
//        public void ApplicationErrorEmailTests()
//        {
//            var exceptionMessage = Faker.Lorem.Paragraph(10);
//            var appName = $"Unit Test of {nameof(ApplicationErrorEmailTests)}";
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

//            Assert.Equal(appName, smtpEmail.AppName);
//            Assert.Equal(environment, smtpEmail.Environment);

//            var args = OltSmtpEmailExtensions.BuildOltEmailClient(ex, _smtpTestServer, smtpEmail);
//            Assert.True(args.AllowSend(email));

//            args = OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, smtpEmail, ex);
//            Assert.True(args.AllowSend(email));           


//            Assert.Throws<Exception>(() => OltSmtpEmailExtensions.OltEmailError(ex, _smtpTestServer, smtpEmail, true)); //SENDS EMAIL

//            try
//            {
//                OltSmtpEmailExtensions.OltEmailError(ex, _smtpTestServer, smtpEmail, false); //SENDS EMAIL
//                Assert.True(true);
//            }
//            catch
//            {
//                Assert.True(false);
//            }
            
            

//        }

//        [Fact]
//        public async Task InvalidServerException()
//        {
//            var invalidServer = new OltSmtpServer
//            {
//                Host = _smtpTestServer.Host,
//                Port = null,
//                Credentials = null
//            };

//            var smtpEmail = new OltSmtpEmail
//            {
//                Subject = $"{Faker.Lorem.Words(10).Last()} {Faker.Lorem.Words(40).Last()}",
//                Body = Faker.Lorem.Paragraph(4),
//                From = new OltEmailAddress
//                {
//                    Name = Faker.Name.FullName(),
//                    Email = Faker.Internet.Email()
//                },
//                Recipients = new OltEmailRecipients
//                {
//                    To = new List<IOltEmailAddress>
//                        {
//                            new OltEmailAddress
//                            {
//                                Name = Faker.Name.FullName(),
//                                Email = Faker.Internet.FreeEmail()
//                            }
//                        },
//                },
//            };

//            var result1 = OltSmtpEmailExtensions.BuildOltEmailClient(invalidServer, true, smtpEmail).Send(false);
//            var result2 = await OltSmtpEmailExtensions.BuildOltEmailClient(invalidServer, true, smtpEmail).SendAsync(false);

//            Assert.False(result2.Success);
//            result2.Should().BeEquivalentTo(result1);

//            Assert.Throws<System.Net.Mail.SmtpException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(invalidServer, true, smtpEmail).Send(true));
//            await Assert.ThrowsAsync<System.Net.Mail.SmtpException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(invalidServer, true, smtpEmail).SendAsync(true));

//        }


//        [Fact]
//        public async Task SmtpWhitelist()
//        {
//            var buildVersion = _configuration.GetValue<string>("GITHUB_RUN_NUMBER") ??
//                              Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER") ??
//                              "[No Run Number]";

//            var now = DateTimeOffset.Now;
//            var name = Faker.Name.FullName();
//            var email = Faker.Internet.Email();
//            var bccName = Faker.Name.FullName();
//            var bccEmail = Faker.Internet.Email();
//            var bccName2 = Faker.Name.FullName();
//            var bccEmail2 = Faker.Internet.Email();

//            var fromName = $"{Faker.Name.First()} Unit Test";
//            var fromEmail = _configuration.GetValue<string>("SMTP_FROM_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_FROM_ADDRESS");
//            var subject = $"Unit Test Email {now:f} Run - {buildVersion} ";
//            var message = $"{buildVersion} -> This was generated on {now:f} from {this.GetType().Assembly.GetName().Name}.{nameof(SmtpSendTests)}.{nameof(SmtpEmail)}";


//            var smtpEmail = new OltSmtpEmail
//            {
//                Subject = subject,
//                Body = message,
//                Recipients = new OltEmailRecipients
//                {
//                    To = new List<IOltEmailAddress>
//                        {
//                            new OltEmailAddress
//                            {
//                                Name = name,
//                                Email = email
//                            }
//                        },
//                    CarbonCopy = new List<IOltEmailAddress>
//                        {
//                            new OltEmailAddress
//                            {
//                                Name = bccName,
//                                Email = bccEmail
//                            },
//                            new OltEmailAddress
//                            {
//                                Name = bccName2,
//                                Email = bccEmail2
//                            }
//                        },
//                },
//                From = new OltEmailAddress
//                {
//                    Name = fromName,
//                    Email = fromEmail
//                },
//            };

//            //var whitelist = new OltEmailConfigurationWhitelist();

//            var args = OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, false, smtpEmail); //.WithWhitelist(whitelist); 
//            var noRecipientsValidationException = await Assert.ThrowsAsync<OltEmailNoRecipientsValidationException>(() => args.SendAsync(true));  //Should Fail due to whitelist
//            Assert.Equal("No Recipients were attached.  This can be caused by skipping due to the whitelist", noRecipientsValidationException.Message);

                        
//            args.Send(false).Errors.Should().NotBeEmpty().And.HaveCount(1);

//            var result = await args.SendAsync(false);
//            result.Success.Should().BeFalse();
//            result.Errors.Should().NotBeEmpty().And.HaveCount(1);

//        }

//        [Fact]
//        public async Task SmtpEmail()
//        {
//            Assert.NotNull(_smtpTestServer);
//            Assert.NotNull(_smtpTestServer.Host);
//            Assert.True(_smtpTestServer.Port > 0);
//            Assert.NotNull(_smtpTestServer.Credentials.Username);
//            Assert.NotNull(_smtpTestServer.Credentials.Password);
//            Assert.False(_smtpTestServer.DisableSsl);
            

//            var buildVersion = _configuration.GetValue<string>("GITHUB_RUN_NUMBER") ??
//                               Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER") ??
//                               "[No Run Number]";

//            var now = DateTimeOffset.Now;
//            var name = Faker.Name.FullName();
//            var email = _configuration.GetValue<string>("SMTP_TO_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_TO_ADDRESS");
//            var bccName = Faker.Name.FullName();
//            var bccEmail = Faker.Internet.Email();
//            var bccName2 = Faker.Name.FullName();
//            var bccEmail2 = Faker.Internet.Email();

//            var fromName = $"{Faker.Name.First()} Unit Test";
//            var fromEmail = _configuration.GetValue<string>("SMTP_FROM_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_FROM_ADDRESS");
//            var subject = $"Unit Test Email {now:f} Run - {buildVersion} ";
//            var message = $"{buildVersion} -> This was generated on {now:f} from {this.GetType().Assembly.GetName().Name}.{nameof(SmtpSendTests)}.{nameof(SmtpEmail)}";


//            var smtpEmail = new OltSmtpEmail
//            {
//                Subject = subject,
//                Body = message,
//                Recipients = new OltEmailRecipients
//                {
//                    To = new List<IOltEmailAddress>
//                        {
//                            new OltEmailAddress
//                            {
//                                Name = name,
//                                Email = email
//                            }
//                        },
//                    CarbonCopy = new List<IOltEmailAddress>
//                        {
//                            new OltEmailAddress
//                            {
//                                Name = bccName,
//                                Email = bccEmail
//                            },
//                            new OltEmailAddress
//                            {
//                                Name = bccName2,
//                                Email = bccEmail2
//                            }
//                        },
//                },
//                From = new OltEmailAddress
//                {
//                    Name = fromName,
//                    Email = fromEmail
//                },
//            };


//            Assert.NotNull(smtpEmail.Recipients);
//            Assert.NotEmpty(smtpEmail.Recipients.To);
//            Assert.NotEmpty(smtpEmail.Recipients.CarbonCopy);
//            Assert.Equal(name, smtpEmail.Recipients.To[0].Name);
//            Assert.Equal(email, smtpEmail.Recipients.To[0].Email);

//            Assert.Equal(bccName, smtpEmail.Recipients.CarbonCopy[0].Name);
//            Assert.Equal(bccEmail, smtpEmail.Recipients.CarbonCopy[0].Email);

//            Assert.Equal(bccName2, smtpEmail.Recipients.CarbonCopy[1].Name);
//            Assert.Equal(bccEmail2, smtpEmail.Recipients.CarbonCopy[1].Email);
            
//            Assert.Equal(subject, smtpEmail.Subject);
//            Assert.Equal(message, smtpEmail.Body);
//            Assert.Equal(fromName, smtpEmail.From.Name);
//            Assert.Equal(fromEmail, smtpEmail.From.Email);

//            var result = OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, true, smtpEmail).Send(true); //SENDS EMAIL
//            Assert.True(result.Success);

//            var result2 = await OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, true, smtpEmail).SendAsync(true); //SENDS EMAIL
//            Assert.True(result.Success);
//            result2.Should().BeEquivalentTo(result);
//            result2.RecipientResults.To.Should().HaveSameCount(smtpEmail.Recipients.To);
//            result2.RecipientResults.CarbonCopy.Should().HaveSameCount(smtpEmail.Recipients.CarbonCopy);
//        }

       


//    }
//}