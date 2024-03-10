using FluentAssertions;
using OLT.Constants;
using OLT.Email.Tests.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Email.Tests.Common
{
    public class CommonArgBuilderTests
    {

        [Fact]
        [Obsolete]
        public void Recipients()
        {
            var args = new TestArgs();
            var toList = new List<IOltEmailAddress>
            {
                new OltEmailAddress
                {
                    Email = Faker.Internet.Email(),
                    Name = Faker.Name.FullName()
                }
            };

            var carbonList = new List<IOltEmailAddress>
            {
                new OltEmailAddress
                {
                    Email = Faker.Internet.Email(),
                    Name = Faker.Name.FullName()
                }
            };

            args = args.WithRecipients(new OltEmailRecipients { To = null, CarbonCopy = null });
            Assert.NotNull(args.ToValue);
            Assert.NotNull(args.CarbonCopyValue);
            Assert.Empty(args.ToValue);
            Assert.Empty(args.CarbonCopyValue);

            var emailRecipients = new OltEmailRecipients
            {
                To = toList,
                CarbonCopy = carbonList
            };

            args = args.WithRecipients(emailRecipients);
            args.ToValue.Should().BeEquivalentTo(toList.OfType<OltEmailAddress>());
            args.CarbonCopyValue.Should().BeEquivalentTo(carbonList.OfType<OltEmailAddress>());
            args.BuildRecipientsValue.To.Should().BeEquivalentTo(toList.OfType<OltEmailAddress>());
            args.BuildRecipientsValue.CarbonCopy.Should().BeEquivalentTo(carbonList.OfType<OltEmailAddress>());

            var duplicate = new List<IOltEmailAddress>();
            duplicate.AddRange(toList);
            duplicate.AddRange(carbonList);

            args = args.WithRecipients(new OltEmailRecipients { To = duplicate, CarbonCopy = duplicate });

            var compareTo = new List<IOltEmailAddress>();
            compareTo.AddRange(toList);
            compareTo.AddRange(duplicate);
            args.ToValue.Should().BeEquivalentTo(compareTo.OfType<OltEmailAddress>());

            compareTo.Clear();
            compareTo.AddRange(carbonList);
            compareTo.AddRange(duplicate);

            args.CarbonCopyValue.Should().BeEquivalentTo(compareTo.OfType<OltEmailAddress>());
            args.BuildRecipientsValue.To.Should().BeEquivalentTo(duplicate.OfType<OltEmailAddress>());
            args.BuildRecipientsValue.CarbonCopy.Should().BeEmpty();

        }

        [Fact]
        [Obsolete]
        public void FromEmail()
        {
            var args = new TestArgs();

            var email = Faker.Internet.Email();

            args = args.WithFromEmail(email);
            Assert.Equal(email, args.EmailValue.Email);
            Assert.Null(args.EmailValue.Name);


            email = Faker.Internet.FreeEmail();
            args.WithFromEmail(new OltEmailAddress(email));
            Assert.Equal(email, args.EmailValue.Email);
            Assert.Null(args.EmailValue.Name);


            var personName = Faker.Name.FullName();

            args = args.WithFromEmail(email, personName);
            Assert.Equal(email, args.EmailValue.Email);
            Assert.Equal(personName, args.EmailValue.Name);

            args.WithFromEmail(new OltEmailAddress(email, personName));
            Assert.Equal(email, args.EmailValue.Email);
            Assert.Equal(personName, args.EmailValue.Name);
            args.ValidationErrors().Should().HaveCount(1);

            args.WithFromEmail(new OltEmailAddress(" ", personName));
            args.ValidationErrors().Should().HaveCount(2);

            args.WithFromEmail(new OltEmailAddress("", personName));
            args.ValidationErrors().Should().HaveCount(2);

            args.WithFromEmail(new OltEmailAddress(null, personName));
            args.ValidationErrors().Should().HaveCount(2);
        }

        [Fact]
        [Obsolete]
        public void Whitelist()
        {
            var whiteEmail = Faker.Internet.Email();
            var whiteDomain = Faker.Internet.DomainName();
            var whiteDomainEmail = $"{Faker.Internet.UserName()}@{whiteDomain}";
            var badDomainEmail = $"{Faker.Internet.UserName()}@bad.{Faker.Internet.DomainName()}";


            var emailList = new List<string>
            {
                whiteEmail,
                Faker.Internet.Email(),
                Faker.Internet.FreeEmail(),
                Faker.Internet.Email(),
                Faker.Internet.FreeEmail(),
                "",
                " ",
                null
            };

            var domainList = new List<string>
            {
                whiteDomain,
                Faker.Internet.DomainName(),
                Faker.Internet.DomainName(),
                Faker.Internet.DomainName(),
                "",
                " ",
                null
            };

            var config = new OltEmailConfigurationWhitelist { Domain = null, Email = null };
            config.DomainParsed.Should().BeEmpty();
            config.EmailParsed.Should().BeEmpty();

            config = new OltEmailConfigurationWhitelist { Domain = string.Join(';', domainList), Email = string.Join(';', emailList) };
            config.DomainParsed.Should().HaveCount(domainList.Count - 3);
            config.EmailParsed.Should().HaveCount(emailList.Count - 3);


            var args = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = null, Email = null });
            Assert.False(args.AllowSend(Faker.Internet.Email()));
            Assert.False(args.AllowSend(badDomainEmail));
            Assert.False(args.AllowSend(whiteEmail));
            Assert.False(args.AllowSend(whiteDomainEmail));            


            args = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = null, Email = string.Join(';', emailList) });
            Assert.False(args.AllowSend(Faker.Internet.Email()));
            Assert.False(args.AllowSend(badDomainEmail));
            Assert.True(args.AllowSend(whiteEmail));
            Assert.False(args.AllowSend(whiteDomainEmail));

            args = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = string.Join(';', domainList), Email = null });
            Assert.False(args.AllowSend(Faker.Internet.Email()));
            Assert.False(args.AllowSend(badDomainEmail));
            Assert.False(args.AllowSend(whiteEmail));
            Assert.True(args.AllowSend(whiteDomainEmail));

            args = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = string.Join(';', domainList), Email = string.Join(';', emailList) });
            Assert.False(args.AllowSend(Faker.Internet.Email()));
            Assert.False(args.AllowSend(badDomainEmail));
            Assert.True(args.AllowSend(whiteEmail));
            Assert.True(args.AllowSend(whiteDomainEmail));

            new OltEmailClientSmtp().EnableProductionEnvironment(false).Invoking(args => args.WithWhitelist(new OltEmailAddress { Email = null })).Should().Throw<InvalidOperationException>();
            
            args = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailAddress {  Email = whiteEmail });
            Assert.False(args.AllowSend(Faker.Internet.Email()));
            Assert.False(args.AllowSend(badDomainEmail));
            Assert.True(args.AllowSend(whiteEmail));
            Assert.False(args.AllowSend(whiteDomainEmail));
        }

        [Fact]
        [Obsolete]
        public void Errors()
        {

            var args = new TestArgs();


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
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From);
            errors.Should().BeEquivalentTo(compareErrors);

            args.Invoking(args => args.WithFromEmail(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithFromEmail(null, null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithRecipients(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithAttachment(null)).Should().Throw<ArgumentNullException>();

            OltEmailConfigurationWhitelist config = null;
            OltEmailAddress emailAddress = null;
            args.Invoking(args => args.WithWhitelist(config)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithWhitelist(emailAddress)).Should().Throw<ArgumentNullException>();

            var msg = Faker.Lorem.GetFirstWord();
            var testEx = new OltEmailValidationException(compareErrors, msg);
            testEx.Errors.Should().BeEquivalentTo(compareErrors);
            Assert.Equal(msg, testEx.Message);

        }
    }
}