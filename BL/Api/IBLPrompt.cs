using BL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Api
{
    public interface IBLPrompt
    {
        Task<string> ProcessPromptAsync(BLPrompt prompt);
        Task<string> CallOpenAiApi(string promptText);

        void Create(BLPrompt entity);
        BLPrompt Read(int id);
        void Update(BLPrompt entity);
        void Delete(int id);
        List<BLPrompt> GetAll();
        List<BLPrompt> GetPromptsByUserId(int userId);
        IEnumerable<BLPrompt> GetPromptsByUserIdAndCategory(int userId, int categoryId);
    }
}