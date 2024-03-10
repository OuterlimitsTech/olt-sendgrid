using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email.Tests.Smtp
{
    public static class SmtpHelper
    {
        public static IOltEmailAddress FakerEmailAddress()
        {
            return new OltEmailAddress
            {
                Name = Faker.Name.FullName(),
                Email = Faker.Internet.FreeEmail()
            };
        }

        public static OltEmailConfigurationWhitelist BuildWhitelist(List<IOltEmailAddress> emailAddresses)
        {
            var config = new OltEmailConfigurationWhitelist();
            config.Email = string.Join(';', emailAddresses.Select(p => p.Email));
            return config;
        }

        public static OltSmtpEmail FakerSmtpEmail(int numTo, int numCarbonCopy)
        {
            var result = new OltSmtpEmail
            {
                Subject = $"Invite Test to {Faker.Address.City()}",
                Body = Faker.Lorem.Paragraph(4),
                From = new OltEmailAddress
                {
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()
                }                
            };

            for(int i = 0; i < numTo; i++)
            {
                result.Recipients.To.Add(FakerEmailAddress());
            }

            for (int i = 0; i < numCarbonCopy; i++)
            {
                result.Recipients.CarbonCopy.Add(FakerEmailAddress());
            }

            return result;
        }
    }
}
