using FluentAssertions;
using OLT.Constants;
using OLT.Email.Tests.Smtp.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Tests.Smtp
{
    public class SmtpArgBuilderTests
    {

        [Fact]
        [Obsolete]
        public void WithSmtpHost()
        {
            var args = new SmtpTestArgs();

            var value = Faker.Lorem.GetFirstWord();

            args = args.WithSmtpHost(value);
            Assert.Equal(value, args.SmtpHostValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
        }

        [Fact]
        [Obsolete]
        public void WithSmtpSSLDisabled()
        {
            var args = new SmtpTestArgs();

            Assert.False(args.SmtpSSLDisabledValue);
            args = args.WithSmtpSSLDisabled(true);
            Assert.True(args.SmtpSSLDisabledValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
        }

        [Fact]
        [Obsolete]
        public void WithSmtpPort()
        {
            var args = new SmtpTestArgs();

            var value = Convert.ToInt16(Faker.RandomNumber.Next(1, short.MaxValue));

            args = args.WithSmtpPort(value);
            Assert.Equal(value, args.SmtpPortValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
        }

        [Fact]
        [Obsolete]
        public void WithSubject()
        {
            var args = new SmtpTestArgs();

            var value = Faker.Lorem.GetFirstWord();

            args = args.WithSubject(value);
            Assert.Equal(value, args.SubjectLineValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Body);
        }


        [Fact]
        [Obsolete]
        public void WithAppError()
        {
            var args = new SmtpTestArgs();
            var errorMsg = Faker.Lorem.Paragraph(10);
            var appName = Faker.Lorem.Words(20).Last();
            var environment = Faker.Lorem.Words(10).Last();

            var ex = new Exception(errorMsg);
            

            args = args.WithAppError(ex, appName, environment);
            Assert.StartsWith($"[{appName}] APPLICATION ERROR in {environment} Environment occurred at", args.SubjectLineValue);
            Assert.Equal($"The following error occurred:{Environment.NewLine}{ex}", args.BodyValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host);
        }

        [Fact]
        [Obsolete]
        public void WithBody()
        {
            var args = new SmtpTestArgs();

            var value = Faker.Lorem.Paragraph(10);

            args = args.WithBody(value);
            Assert.Equal(value, args.BodyValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject);
        }

        [Fact]
        [Obsolete]
        public void WithSmtpNetworkCredentials()
        {
            var args = new SmtpTestArgs();

            var username = Faker.Internet.UserName();
            var password = Faker.Internet.DomainWord();

            args = args.WithSmtpNetworkCredentials(username, password);
            Assert.Equal(username, args.SmtpUsernameValue);
            Assert.Equal(password, args.SmtpPasswordValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
        }


        [Fact]
        [Obsolete]
        public void Errors()
        {

            var args = new SmtpTestArgs();

            args.Invoking(args => args.DoValidation()).Should().Throw<OltEmailValidationException>().WithMessage(OltEmailValidationException.DefaultMessage);

            List<string> compareErrors = new List<string>();
            try
            {
                args.DoValidation();
            }
            catch (OltEmailValidationException ex)
            {
                compareErrors = ex.Errors;
                var errorResult = ex.ToEmailResult();
                compareErrors.Should().Equal(errorResult.Errors);
                Assert.False(errorResult.Success);
            }

            var errors = args.ValidationErrors();
            Assert.NotEmpty(errors);
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
            errors.Should().BeEquivalentTo(compareErrors);

            args.Invoking(args => args.WithSmtpNetworkCredentials(null, null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithSmtpNetworkCredentials(Faker.Internet.UserName(), null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithSmtpNetworkCredentials(null, Faker.Lorem.GetFirstWord())).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithBody(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithSubject(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithSmtpPort(0)).Should().Throw<ArgumentOutOfRangeException>();
            args.Invoking(args => args.WithSmtpHost(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithCalendarInvite(null)).Should().Throw<ArgumentNullException>();            
            args.Invoking(args => args.WithAppError(null, null, null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithAppError(new Exception(Faker.Name.Last()), null, null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithAppError(new Exception(Faker.Name.First()), Faker.Company.Name(), null)).Should().Throw<ArgumentNullException>();
        }


        [Fact]
        [Obsolete]
        public void AllowSendTests()
        {

            var blockEmail1 = Faker.Internet.Email();
            var blockEmail2 = Faker.Internet.Email();

            var whiteEmail = Faker.Internet.Email();
            var whiteDomain = Faker.Internet.DomainName();
            var whiteEmailDomain = $"{Faker.Internet.UserName()}@{whiteDomain}";
            

            var bccWhiteEmail = Faker.Internet.Email();
            var bccWhiteDomain = Faker.Internet.DomainName();
            var bccWhiteEmailDomain = $"{Faker.Internet.UserName()}@{bccWhiteDomain}";

            var whitelist = new OltEmailConfigurationWhitelist();
            whitelist.Email = $"{whiteEmail};{bccWhiteEmail};;"; //extra semicolon is intentional
            whitelist.Domain = $"{whiteDomain};{bccWhiteDomain}; ;"; //extra semicolon is intentional


            var server = new OltSmtpServer
            {
                Host = Faker.Internet.DomainName(),
                DisableSsl = true,
                Port = Convert.ToInt16(Faker.RandomNumber.Next(1, short.MaxValue)),
                Credentials = new OltSmtpCredentials
                {
                    Username = Faker.Internet.UserName(),
                    Password = Faker.Lorem.GetFirstWord()
                }
            };

            var smtpEmail = new OltSmtpEmail
            {
                Subject = Faker.Lorem.Sentence(),
                Body = Faker.Lorem.Paragraph(),
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                    {
                        new OltEmailAddress
                        {
                            Email = whiteEmail
                        },
                        new OltEmailAddress
                        {
                            Email = whiteEmailDomain
                        },
                        new OltEmailAddress
                        {
                            Email = blockEmail1
                        },
                    },
                    CarbonCopy = new List<IOltEmailAddress>
                    {
                        new OltEmailAddress
                        {
                            Email = bccWhiteEmail
                        },
                        new OltEmailAddress
                        {
                            Email = bccWhiteEmailDomain
                        },
                        new OltEmailAddress
                        {
                            Email = blockEmail2
                        },
                    }
                },
                From = new OltEmailAddress
                {
                    Email = Faker.Internet.Email()
                },
            };

            var args = OltSmtpEmailExtensions.BuildOltEmailClient(server, false, smtpEmail).WithWhitelist(whitelist);
            var recipientResult = args.BuildRecipients();

            recipientResult.To.Should().NotBeNull();
            recipientResult.To.Should().HaveCount(smtpEmail.Recipients.To.Count);

            recipientResult.CarbonCopy.Should().NotBeNull();
            recipientResult.CarbonCopy.Should().HaveCount(smtpEmail.Recipients.CarbonCopy.Count);

            TestResult(recipientResult.To.FirstOrDefault(p => p.Email == whiteEmail), true);
            TestResult(recipientResult.To.FirstOrDefault(p => p.Email == whiteEmailDomain), true);
            TestResult(recipientResult.To.FirstOrDefault(p => p.Email == blockEmail1), false);

            TestResult(recipientResult.CarbonCopy.FirstOrDefault(p => p.Email == bccWhiteEmail), true);
            TestResult(recipientResult.CarbonCopy.FirstOrDefault(p => p.Email == bccWhiteEmailDomain), true);
            TestResult(recipientResult.CarbonCopy.FirstOrDefault(p => p.Email == blockEmail2), false);

            
        }

        [Obsolete]
        private static void TestResult(OltEmailAddressResult result, bool success)
        {
            Assert.Equal(success, result.Success);
            Assert.Null(result.Error);

            if (success)
            {
                Assert.True(result.Sent);
                Assert.False(result.Skipped);
                Assert.Null(result.SkipReason);                
            }
            else
            {
                Assert.False(result.Sent);
                Assert.True(result.Skipped);
                Assert.Equal("Email not in whitelist", result.SkipReason);
            }
            

        }
    }
}
