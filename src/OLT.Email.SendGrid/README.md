[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

# SendGrid Builder pattern that introduces a whitelist

_The goal was to allow email processes to run in a test environment and only send to domains and/or email addresses in a whitelist. Production environments would ignore the whitelist and send it to all recipients_

---

- ## Template with Data

```csharp

var templateId = "d-valuefromsendgrid";
var templateData = new MyTemplateDataClass();  //Contains data tha will serialize to JSON and bind to template

var fromEmail = new OltEmailAddress
{
    Name = "My Company Name",
    Email = "no-reply@mycompany.com",
};

var whitelist = new OltEmailConfigurationWhitelist
{
    Email = new List<string> { "someemail@email.com", "anotherperson@fake.com" },
    Domain = new List<string> { "google.com", "microsoft.com" },  //Any email that ends with domain name will send in non-prod environments
};

var recipients = new OltEmailRecipients
{
    To = new List<IOltEmailAddress>
    {
        new OltEmailAddress
        {
            Name = "Name Here"
            Email = "someemail@email.com"
        }
    },
    CarbonCopy = new List<IOltEmailAddress>
    {
        new OltEmailAddress
        {
            Name = "Another Name Here"
            Email = "someemail@email.com"
        }
    }
};

var client = new OltSendGridClient()
    .WithFromEmail(fromEmail)
    .WithApiKey(_config.ApiKey)
    .WithWhitelist(whitelist)
    .WithTemplate(templateId, templateData)
    .WithRecipients(recipients)
    .WithCustomArg("email_uid", uid)
    .WithCustomArg("some_internal_id", internalId)
    //1) false will only send to whitelist domains or email addresses
    //2) true will ignore whitelist and send all emails.
    //3) typically controlled via config/environment variable
    .EnableProductionEnvironment(false);

resultAsync = await client.SendAsync();
```

---

- ## Template with No Data

```csharp

var templateId = "d-valuefromsendgrid";

var whitelist = new OltEmailConfigurationWhitelist
{
    Email = new List<string> { "someemail@email.com", "anotherperson@fake.com" },
    Domain = new List<string> { "google.com", "microsoft.com" },  //Any email that ends with domain name will send in non-prod environments
};

var recipients = new OltEmailRecipients
{
    To = new List<IOltEmailAddress>
    {
        new OltEmailAddress
        {
            Name = "Name Here"
            Email = "someemail@email.com"
        }
    }
};

var client = new OltSendGridClient()
    .WithFromEmail("no-reply@mycompany.com", "Company Name")
    .WithApiKey(_config.ApiKey)
    .WithWhitelist(whitelist)
    .WithTemplate(templateId)  // <-----  Just send the templateId
    .WithRecipients(recipients)
    .EnableProductionEnvironment(true); // <---- This will send all emails ignoring the whitelist

resultAsync = await client.SendAsync();
```
