using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email.SendGrid.Tests
{
    public static class SendGridHelper
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

        public static Dictionary<string, string> FakerCustomArgs(int number)
        {
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < number; i++)
            {
                var num = Faker.RandomNumber.Next(1, 2000);
                dict.Add($"Key-{i}", $"Value-{num}");
            }
            return dict;
        }

        public static OltEmailConfigurationSendGrid FakerConfig(bool production, int numEmailWhitelist, int numDomainWhitelist)
        {
            var result = new OltEmailConfigurationSendGrid
            {
                ApiKey = Faker.Company.Name(),
                Production = production,
                From = new OltEmailAddress
                {
                    Email = Faker.Internet.Email(),
                    Name = Faker.Name.FullName(),
                },
            };

            for(int i = 0; i < numEmailWhitelist; i++)
            {
                result.TestWhitelist.Email = FakerEmailAddress().Email;
            }

            for (int i = 0; i < numDomainWhitelist; i++)
            {
                result.TestWhitelist.Email = Faker.Internet.DomainName();
            }

            return result;
        }

        public static List<OltEmailAttachment> FakerAttachment(int number)
        {
            var contentType = "text/plain";
            var starting = 15;
            var result = new List<OltEmailAttachment>();

            for (int idx = 1; idx <= number; idx++)
            {
                var seed = starting * idx;
                
                var fileName = $"{Faker.Lorem.Words(seed).Last()}-{idx}.txt";
                var bytes = Encoding.ASCII.GetBytes(Faker.Lorem.Paragraph(seed));

                var attachment = new OltEmailAttachment
                {
                    FileName = fileName,
                    ContentType = contentType,
                    Bytes = bytes,
                };

                result.Add(attachment);
            }

            return result;
        }
    }
}
