namespace OLT.Email.SendGrid
{
    public interface IOltApiKeyArgs<out T>
    {
        T WithApiKey(string apiKey);
    }
}
