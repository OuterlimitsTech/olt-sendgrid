using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OLT.Email
{
    public interface IOltEmailClient
    {
        bool AllowSend(string emailAddress);
        bool IsValid { get; }
        List<string> ValidationErrors();
        OltEmailRecipientResult BuildRecipients();
    }

    public interface IOltEmailClient<out TClient, out TMessage, TResult> : IOltEmailClient
        where TClient : class
        where TMessage : class
        where TResult : class, IOltEmailResult
    {        
        TClient CreateClient();
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        TMessage CreateMessage(OltEmailRecipientResult recipients);
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        TResult Send(bool throwExceptions);
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        Task<TResult> SendAsync(bool throwExceptions);

    }
}
