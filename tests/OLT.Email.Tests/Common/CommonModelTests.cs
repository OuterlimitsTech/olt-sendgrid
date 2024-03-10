using FluentAssertions;
using OLT.Email.Tests.Common.Assets;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OLT.Email.Tests.Common
{
    public class CommonModelTests
    {
        [Fact]
        [Obsolete]
        public void EmailAddress()
        {
            var email = Faker.Internet.Email();
            var personName = Faker.Name.FullName();

            var model = new OltEmailAddress(email);
            Assert.Equal(email, model.Email);
            Assert.Null(model.Name);


            model = new OltEmailAddress(email, personName);
            Assert.Equal(email, model.Email);
            Assert.Equal(personName, model.Name);
        }

        [Fact]
        [Obsolete]
        public void EmailAttachment()
        {            
            var contentType = Faker.Internet.DomainWord();
            var fileName = Faker.Lorem.GetFirstWord();
            var words = Faker.Lorem.Paragraph(1);
            var bytes = Encoding.ASCII.GetBytes(words);

            var model = new OltEmailAttachment
            {
                FileName = fileName,
                ContentType = contentType,
                Bytes = bytes,
            };


            Assert.Equal(model.FileName, fileName);
            Assert.Equal(model.ContentType, contentType);
            Assert.Equal(model.Bytes, bytes);

            var args = new TestArgs();
            Assert.Empty(args.AttachmentValue);
            args.WithAttachment(model);
            Assert.NotEmpty(args.AttachmentValue);

        }


        [Fact]
        [Obsolete]
        public void EmailTag()
        {
            var tag = Faker.Lorem.GetFirstWord();
            var value = Faker.Lorem.Paragraph(2);

            var model = new OltEmailTag
            {
                Tag = tag,
                Value = value,
            };

            Assert.Equal(tag, model.Tag);
            Assert.Equal(value, model.Value);

            model = new OltEmailTag(tag, value);
            Assert.Equal(tag, model.Tag);
            Assert.Equal(value, model.Value);

            Assert.Empty(OltEmailTag.ToDictionary(null));

            var list = new List<OltEmailTag>();
            var model2 = new OltEmailTag(Faker.Name.First(), Faker.Lorem.Paragraph(1));
            list.Add(model);
            list.Add(model);
            list.Add(model2);

            var dictionary = OltEmailTag.ToDictionary(list);
            Assert.NotEmpty(dictionary);
            dictionary.Should().HaveCount(2);

            var compareTo = new Dictionary<string, string>();
            compareTo.Add(model.Tag, model.Value);
            compareTo.Add(model2.Tag, model2.Value);
            dictionary.Should().BeEquivalentTo(compareTo);

        }


        [Fact]
        [Obsolete]
        public void EmailResults()
        {
            var error = Faker.Lorem.GetFirstWord();
            var email = Faker.Internet.Email();            

            var args = new TestArgs();
            Assert.Throws<ArgumentNullException>(() => new OltEmailAddressResult(null, args));
            Assert.Throws<ArgumentNullException>(() => new OltEmailAddressResult(new OltEmailAddress(email), null));


            var model = new OltEmailAddressResult(new OltEmailAddress(email), args);
            Assert.Equal(email, model.Email);
            Assert.Null(model.Name);

            var personName = Faker.Name.FullName();
            
            model = new OltEmailAddressResult(new OltEmailAddress(email, personName), args);
            Assert.Equal(email, model.Email);
            Assert.Equal(personName, model.Name);
            Assert.True(model.Skipped);
            Assert.NotEmpty(model.SkipReason);
            Assert.False(model.Success);

            args.EnableProductionEnvironment(true);
            model = new OltEmailAddressResult(new OltEmailAddress(email, personName), args);
            Assert.False(model.Skipped);
            Assert.Null(model.SkipReason);
            Assert.Null(model.Error);
            Assert.True(model.Success);

            model.Error = error;
            Assert.False(model.Skipped);
            Assert.Null(model.SkipReason);
            Assert.Equal(error, model.Error);
            Assert.False(model.Success);


            var resultRecipient = new OltEmailRecipientResult();
            Assert.Empty(resultRecipient.To);
            Assert.Empty(resultRecipient.CarbonCopy);

            resultRecipient.To.Add(model);
            resultRecipient.CarbonCopy.Add(model);

            Assert.NotEmpty(resultRecipient.To);
            Assert.NotEmpty(resultRecipient.CarbonCopy);


            var result = new OltEmailResult();
            Assert.Empty(result.RecipientResults.To);
            Assert.Empty(result.RecipientResults.CarbonCopy);
            Assert.Empty(result.Errors);
            Assert.True(result.Success);

            result.RecipientResults = resultRecipient;
            result.Errors.Add(Faker.Lorem.GetFirstWord());

            Assert.NotEmpty(result.RecipientResults.To);
            Assert.NotEmpty(result.RecipientResults.CarbonCopy);
            Assert.NotEmpty(result.Errors);
            Assert.False(result.Success);

        }
    }
}