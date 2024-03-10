namespace OLT.Email
{

    /// <summary>
    /// Defines Standard Email
    /// </summary>
    public class OltEmailAddress : IOltEmailAddress
    {
        public OltEmailAddress()
        {
        }

        public OltEmailAddress(string email)
        {
            Email = email;
        }

        public OltEmailAddress(string email, string name) : this(email)
        {
            Name = name;
        }

        /// <summary>
        /// Email Address
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Name is optional
        /// </summary>
        public virtual string Name { get; set; }
    }
}