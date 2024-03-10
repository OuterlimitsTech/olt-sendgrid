using FluentAssertions;
using OLT.Email.SendGrid.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.SendGrid.Tests
{
    public class SendGridMessageTests
    {

        [Fact]
        [Obsolete]
        public void DisabledRecipientsNonProdTest()
        {
            var template = FakeEmailTagTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(true, 0, 0);

            var toList = new List<IOltEmailAddress>() { template.Recipients.To[1], template.Recipients.To[2] };
            var ccList = new List<IOltEmailAddress>() { template.Recipients.CarbonCopy[0], template.Recipients.CarbonCopy[2] };


            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);

            var msg = args.CreateMessage(args.BuildRecipients());
            var personalization = msg.Personalizations[0];
            var compareTo = personalization.Tos.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            var compareCc = personalization.Ccs.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            Assert.Null(personalization.Bccs);

            compareTo.Should().BeEquivalentTo(template.Recipients.To);
            compareCc.Should().BeEquivalentTo(template.Recipients.CarbonCopy);


            args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template)
                .WithWhitelist(SendGridHelper.BuildWhitelist(toList))
                .WithWhitelist(SendGridHelper.BuildWhitelist(ccList));

            msg = args.CreateMessage(args.BuildRecipients());
            personalization = msg.Personalizations[0];
            compareTo = personalization.Tos.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            compareCc = personalization.Ccs.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            Assert.Null(personalization.Bccs);

            compareTo.Should().BeEquivalentTo(template.Recipients.To);
            compareCc.Should().BeEquivalentTo(template.Recipients.CarbonCopy);


        }


        [Fact]
        [Obsolete]
        public void DisabledRecipientsProdTest()
        {
            var template = FakeEmailTagTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(false, 0, 0);

            var toList = new List<IOltEmailAddress>() { template.Recipients.To[1], template.Recipients.To[2] };
            var ccList = new List<IOltEmailAddress>() { template.Recipients.CarbonCopy[0], template.Recipients.CarbonCopy[2] };

            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);

            var msg = args.CreateMessage(args.BuildRecipients());
            var personalization = msg.Personalizations[0];
            Assert.Null(personalization.Tos);
            Assert.Null(personalization.Ccs);
            Assert.Null(personalization.Bccs);

            args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template)
                .WithWhitelist(SendGridHelper.BuildWhitelist(toList))
                .WithWhitelist(SendGridHelper.BuildWhitelist(ccList));

            msg = args.CreateMessage(args.BuildRecipients());
            personalization = msg.Personalizations[0];


            var compareTo = personalization.Tos.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            var compareCc = personalization.Ccs.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();

            Assert.Null(personalization.Bccs);

            compareTo.Should().BeEquivalentTo(toList);
            compareCc.Should().BeEquivalentTo(ccList);

        }

        [Fact]
        [Obsolete]
        public void SandboxModeTest()
        {
            var template = FakeEmailTagTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(true, 0, 0);

            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template).EnableSandbox();
            var msg = args.CreateMessage(args.BuildRecipients());
            Assert.NotNull(msg.MailSettings);
            Assert.NotNull(msg.MailSettings.SandboxMode);
            Assert.True(msg.MailSettings.SandboxMode.Enable);

            msg = OltEmailSendGridExtensions.BuildOltEmailClient(config, template).CreateMessage(args.BuildRecipients());
            Assert.NotNull(msg.MailSettings);
            Assert.NotNull(msg.MailSettings.SandboxMode);
            Assert.False(msg.MailSettings.SandboxMode.Enable);
        }


        [Fact]
        [Obsolete]
        public void UnsubscribeGroupTest()
        {
            var template = FakeEmailTagTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(true, 0, 0);

            var number = Faker.RandomNumber.Next(100, 10000);
            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template).WithUnsubscribeGroupId(number);
            var msg = args.CreateMessage(args.BuildRecipients());
            Assert.NotNull(msg.Asm);
            Assert.Equal(number, msg.Asm.GroupId);

            msg = OltEmailSendGridExtensions.BuildOltEmailClient(config, template).CreateMessage(args.BuildRecipients());
            Assert.Null(msg.Asm);
        }

        [Fact]
        [Obsolete]
        public void AttachmentTest()
        {
            var attachments = SendGridHelper.FakerAttachment(10);
            var template = FakeEmailTagTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(true, 0, 0);
            
            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);

            attachments.ForEach(item => args.WithAttachment(item));            

            var msg = args.CreateMessage(args.BuildRecipients());
            Assert.NotNull(msg.Attachments);
            msg.Attachments.Select(c => c.Filename).Should().Equal(attachments.Select(c => c.FileName));
            msg.Attachments.Select(c => c.Type).Should().Equal(attachments.Select(c => c.ContentType));

            msg = OltEmailSendGridExtensions.BuildOltEmailClient(config, template).CreateMessage(args.BuildRecipients());
            Assert.Null(msg.Attachments);
        }

        [Fact]
        [Obsolete]
        public void ClickTrackingTest()
        {
            var template = FakeEmailTagTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(true, 0, 0);
            
            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template).WithoutClickTracking();
            var msg = args.CreateMessage(args.BuildRecipients());
            Assert.NotNull(msg.TrackingSettings);
            Assert.NotNull(msg.TrackingSettings.ClickTracking);
            Assert.False(msg.TrackingSettings.ClickTracking.Enable);
            Assert.False(msg.TrackingSettings.ClickTracking.EnableText);

            msg = OltEmailSendGridExtensions.BuildOltEmailClient(config, template).CreateMessage(args.BuildRecipients());
            Assert.NotNull(msg.TrackingSettings);
            Assert.NotNull(msg.TrackingSettings.ClickTracking);
            Assert.True(msg.TrackingSettings.ClickTracking.Enable);
            Assert.True(msg.TrackingSettings.ClickTracking.EnableText);

        }

        [Fact]
        [Obsolete]
        public void TemplateDataTest()
        {
            var config = SendGridHelper.FakerConfig(true, 0, 0);

            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, FakeNoTemplateDataTemplate.FakerData(7, 5));
            var msg = args.CreateMessage(args.BuildRecipients());
            Assert.Null(msg.Personalizations[0].TemplateData);

            var template = FakeEmailTagTemplate.FakerData(7, 5);
            args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);
            msg = args.CreateMessage(args.BuildRecipients());
            Assert.NotNull(msg.Personalizations[0].TemplateData);
            msg.Personalizations[0].TemplateData.Should().BeEquivalentTo(template.GetTemplateData());
        }


        [Fact]
        [Obsolete]
        public void TemplateIdTest()
        {
            var template = FakeNoTemplateDataTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(true, 0, 0);

            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);
            var msg = args.CreateMessage(args.BuildRecipients());
            Assert.Equal(template.TemplateId, msg.TemplateId);
        }

        [Fact]
        [Obsolete]
        public void FromTest()
        {
            var template = FakeNoTemplateDataTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(true, 0, 0);

            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);
            var msg = args.CreateMessage(args.BuildRecipients());
            Assert.Equal(config.From.Email, msg.From.Email);
            Assert.Equal(config.From.Name, msg.From.Name);
        }

        [Fact]
        [Obsolete]
        public void CustomArgsTest()
        {
            var template = FakeNoTemplateDataTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(true, 0, 0);
            var customArgs = SendGridHelper.FakerCustomArgs(10);
            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);
            foreach(var customArg in customArgs)
            {
                args = args.WithCustomArg(customArg.Key, customArg.Value);  
            }
            var msg = args.CreateMessage(args.BuildRecipients());
            msg.Personalizations[0].CustomArgs.Should().BeEquivalentTo(customArgs);

            args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);
            msg = args.CreateMessage(args.BuildRecipients());            
            Assert.Null(msg.Personalizations[0].CustomArgs);
        }



    }
}
