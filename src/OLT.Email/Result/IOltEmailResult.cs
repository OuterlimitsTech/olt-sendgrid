using System.Collections.Generic;

namespace OLT.Email
{
    public interface IOltEmailResult
    {
        bool Success { get; }
        List<string> Errors { get; set; }
        OltEmailRecipientResult RecipientResults { get; set; }
    }

}
