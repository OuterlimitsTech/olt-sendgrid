using System;
using System.Net.Mail;
using System.Text;

namespace OLT.Email
{    
    public abstract class OltCalendarInviteArgs<T> : OltBodyArgs<T>
       where T : OltCalendarInviteArgs<T>
    {
        protected  byte[] CalendarInviteBtyes { get; set; }

        public const string DefaultFileName = "invite.ics";
        public const string DefaultContentType = "text/calendar";

        protected OltCalendarInviteArgs()
        {
        }

        /// <summary>
        /// ICS File in Bytes
        /// </summary>
        /// <returns></returns>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public T WithCalendarInvite(byte[] icsFileBytes)
        {            
            this.CalendarInviteBtyes = icsFileBytes ?? throw new ArgumentNullException(nameof(icsFileBytes));
            return (T)this;
        }

        public override MailMessage CreateMessage(OltEmailRecipientResult recipients)
        {
            var msg = base.CreateMessage(recipients);

            if (CalendarInviteBtyes != null)
            {
                msg.Headers.Add("Content-class", "urn:content-classes:calendarmessage");

                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType(DefaultContentType);
                contentType.Parameters.Add("method", "REQUEST");
                contentType.Parameters.Add("name", DefaultFileName);
                AlternateView avCal = AlternateView.CreateAlternateViewFromString(Encoding.UTF8.GetString(CalendarInviteBtyes, 0, CalendarInviteBtyes.Length), contentType);
                msg.AlternateViews.Add(avCal);
            }

            return msg;

            
        }
    }
}
