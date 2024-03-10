namespace OLT.Email
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class OltEmailNoRecipientsValidationException : OLT.Core.OltException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public const string DefaultMessage = "No Recipients were attached.  This can be caused by skipping due to the whitelist";

        public OltEmailNoRecipientsValidationException() : base(DefaultMessage)
        {
        }
    }
}
