namespace OLT.Email
{
    /// <summary>
    /// OLT Email Configuration
    /// </summary>
    public class OltEmailConfiguration
    {

        /// <summary>
        /// Default From Address if not specified in Sned
        /// </summary>
        public virtual OltEmailAddress From { get; set; } = new OltEmailAddress();

        /// <summary>
        /// Test Environment whitelist 
        /// </summary>
        public virtual OltEmailConfigurationWhitelist TestWhitelist { get; set; } = new OltEmailConfigurationWhitelist();


        /// <summary>
        /// Running in Production and send all emails if true
        /// </summary>
        public virtual bool Production { get; set; }

       
    }
}