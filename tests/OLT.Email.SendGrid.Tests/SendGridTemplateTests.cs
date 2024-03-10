using FluentAssertions;
using Microsoft.Extensions.Configuration;
using OLT.Email.SendGrid.Tests.Assets;
using System.Collections.Generic;
using Xunit;

namespace OLT.Email.SendGrid.Tests
{
    public class SendGridTemplateTests
    {
        [Fact]
        public void EmailTagTemplateTest()
        {
            var template = new FakeEmailTagTemplate();
            var firstName1 = Faker.Name.First();
            var email1 = Faker.Internet.Email();

            var firstName2 = Faker.Name.First();
            var email2 = Faker.Internet.Email();


            template.Value1 = Faker.Internet.DomainName();
            template.Value2 = Faker.Internet.DomainSuffix();

            template.Recipients.To.Add(new OltEmailAddress(email1, firstName1));
            template.Recipients.To.Add(new OltEmailAddress(email2, firstName2));

            var list = new List<OltEmailAddress>
            {
                new OltEmailAddress(email1, firstName1),
                new OltEmailAddress(email2, firstName2)
            };

            template.Recipients.To.Should().BeEquivalentTo(list);

            Assert.Equal(nameof(FakeEmailTagTemplate), template.TemplateId);
            Assert.NotEmpty(template.Tags);

            Assert.NotNull(template.GetTemplateData());

        }
    }
}
