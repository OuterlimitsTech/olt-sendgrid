namespace OLT.Email
{

    /// <summary>
    /// Defines Standard Email
    /// </summary>
    public interface IOltEmailAddress
    {
        /// <summary>
        /// Email Address
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Name is optional
        /// </summary>
        string Name { get; }
    }
}