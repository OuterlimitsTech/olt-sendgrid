using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Tests.Smtp
{
    public class SmtpMessageTests
    {
        private readonly string FakeHost = Faker.Internet.DomainName();

        [Fact]
        [Obsolete]
        public void TestRecipients()
        {
            var smtpServer = new OltSmtpServer
            {
                Host = FakeHost,
                Credentials = null
            };

            var smtpEmail = SmtpHelper.FakerSmtpEmail(5, 4);
            var toList = new List<IOltEmailAddress>() { smtpEmail.Recipients.To[1], smtpEmail.Recipients.To[2] };
            var ccList = new List<IOltEmailAddress>() { smtpEmail.Recipients.CarbonCopy[0], smtpEmail.Recipients.CarbonCopy[2] };

            var prodEnv = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail);
            using (var msg = prodEnv.CreateMessage(prodEnv.BuildRecipients()))
            {
                var compareTo = msg.To.Select(x => new OltEmailAddress { Email = x.Address, Name = x.DisplayName }).ToList();
                var compareCc = msg.CC.Select(x => new OltEmailAddress { Email = x.Address, Name = x.DisplayName }).ToList();
                Assert.Empty(msg.Bcc);
                compareTo.Should().BeEquivalentTo(smtpEmail.Recipients.To);
                compareCc.Should().BeEquivalentTo(smtpEmail.Recipients.CarbonCopy);
            }


            prodEnv = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail)
                .WithWhitelist(SmtpHelper.BuildWhitelist(toList))
                .WithWhitelist(SmtpHelper.BuildWhitelist(ccList));

            using (var msg = prodEnv.CreateMessage(prodEnv.BuildRecipients()))
            {
                var compareTo = msg.To.Select(x => new OltEmailAddress { Email = x.Address, Name = x.DisplayName }).ToList();
                var compareCc = msg.CC.Select(x => new OltEmailAddress { Email = x.Address, Name = x.DisplayName }).ToList();
                Assert.Empty(msg.Bcc);
                compareTo.Should().BeEquivalentTo(smtpEmail.Recipients.To);
                compareCc.Should().BeEquivalentTo(smtpEmail.Recipients.CarbonCopy);
            }

            var testEnv = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer,false, smtpEmail);
            using (var msg = testEnv.CreateMessage(testEnv.BuildRecipients()))
            {
                Assert.Empty(msg.To);
                Assert.Empty(msg.CC);
                Assert.Empty(msg.Bcc);
            }

            testEnv = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, false, smtpEmail)
                .WithWhitelist(SmtpHelper.BuildWhitelist(toList))
                .WithWhitelist(SmtpHelper.BuildWhitelist(ccList));

            using (var msg = testEnv.CreateMessage(testEnv.BuildRecipients()))
            {
                var compareTo = msg.To.Select(x => new OltEmailAddress { Email = x.Address, Name = x.DisplayName }).ToList();
                var compareCc = msg.CC.Select(x => new OltEmailAddress { Email = x.Address, Name = x.DisplayName }).ToList();
                Assert.Empty(msg.Bcc);
                compareTo.Should().BeEquivalentTo(toList);
                compareCc.Should().BeEquivalentTo(ccList);
            }
        }



        [Fact]
        [Obsolete]
        public void TestCalendar()
        {

            var smtpServer = new OltSmtpServer
            {
                Host = FakeHost,
                Credentials = null
            };

            var created = DateTimeOffset.UtcNow.ToString("s").Replace(":", string.Empty).Replace("-", string.Empty);
            var start = DateTimeOffset.UtcNow.AddHours(6).ToString("s").Replace(":", string.Empty).Replace("-", string.Empty);
            var end = DateTimeOffset.UtcNow.AddHours(7).ToString("s").Replace(":", string.Empty).Replace("-", string.Empty);

            var calString = @$"
                BEGIN:VCALENDAR
                METHOD:REQUEST
                PRODID:-//github.com/rianjs/ical.net//NONSGML ical.net 4.0//EN
                VERSION:2.0
                BEGIN:VEVENT
                ATTENDEE;CN={Faker.Name.FullName()};RSVP=TRUE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:mailto:{Faker.Internet.Email()}
                ATTENDEE;CN={Faker.Name.FullName()};RSVP=TRUE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:mailto:{Faker.Internet.FreeEmail()}
                CREATED:{created}Z
                DTEND:{end}Z
                DTSTAMP:{created}Z
                DTSTART:{start}Z
                LAST-MODIFIED:{created}Z
                LOCATION:In a van down by the river
                ORGANIZER;CN={Faker.Name.FullName()}:mailto:{Faker.Internet.Email()}
                SEQUENCE:0
                STATUS:CONFIRMED
                SUMMARY:This is a bogus invite
                TRANSP:OPAQUE
                UID:9e31362b-4d65-44bc-b8ad-a29c7b80f294
                END:VEVENT
                END:VCALENDAR
                ".RemoveDoubleSpaces();

            var smtpEmail = new OltSmtpEmail
            {
                Subject = $"Invite Test to {Faker.Address.City()}",
                Body = Faker.Lorem.Paragraph(4),
                From = new OltEmailAddress
                {
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()
                },
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                        {
                            new OltEmailAddress
                            {
                                Name = Faker.Name.FullName(),
                                Email = Faker.Internet.FreeEmail()
                            }
                        },
                },
            };

            var bytes = Encoding.ASCII.GetBytes(calString);

            Assert.NotEmpty(bytes);
            var args = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).WithCalendarInvite(bytes);

            using (var msg = args.CreateMessage(args.BuildRecipients()))
            {
                Assert.NotEmpty(msg.Headers);
                Assert.NotEmpty(msg.AlternateViews);
                Assert.Equal("urn:content-classes:calendarmessage", msg.Headers["Content-class"]);
                AlternateView avCal = msg.AlternateViews[0];
                Assert.NotNull(avCal);
                Assert.Equal("text/calendar", avCal.ContentType.MediaType);
                Assert.Equal("invite.ics", avCal.ContentType.Name);
                Assert.Equal("REQUEST", avCal.ContentType.Parameters["method"]);
            }
        }


        
    }
}
