using BL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Api
{
    // זהו החוזה המעודכן. הוא תואם למחלקה הסינכרונית שבנינו.
    public interface IBLPrompt
    {
        // --- פונקציות אסינכרוניות שנשארות (כי הן מערבות קריאות רשת) ---
        Task<string> ProcessPromptAsync(BLPrompt prompt);
        Task<string> CallOpenAiApi(string promptText);

        // --- פונקציות שהפכו לסינכרוניות כדי להתאים ל-DAL ---
        void Create(BLPrompt entity);
        BLPrompt Read(int id);
        void Update(BLPrompt entity);
        void Delete(int id);
        List<BLPrompt> GetAll();
        List<BLPrompt> GetPromptsByUserId(int userId);
        IEnumerable<BLPrompt> GetPromptsByUserIdAndCategory(int userId, int categoryId);
    }
}