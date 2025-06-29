using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BL.Exception; // ודא שה-using הזה קיים

namespace BL.Services;

// [הוספה]: הוספנו את הגדרת הקלאס הזה שהיה חסר
public class ValidatePromptRequest
{
    public string UserPrompt { get; set; }
    public string CategoryName { get; set; }
    public string SubCategoryName { get; set; }
}

public class PromptHandling : IBLPrompt
{
    private readonly IPrompt _learningRepository;
    private readonly ISubCategory _subCategoryRepository;
    private readonly ICategory _categoryRepository;
    private readonly OpenAiSettings _openAiSettings;
    private readonly IHttpClientFactory _httpClientFactory;

    public PromptHandling(IOptions<OpenAiSettings> openAiSettings, IDal dal, IHttpClientFactory httpClientFactory)
    {
        _learningRepository = dal.Prompt;
        _subCategoryRepository = dal.SubCategory;
        _categoryRepository = dal.Category;
        _openAiSettings = openAiSettings.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> ProcessPromptAsync(BLPrompt prompt)
    {
        ValidatePrompt(prompt);

        bool isRelevant = await ValidateRequestRelevance(prompt);
        if (!isRelevant)
        {
            throw new HttpRequestException("הפנייה אינה מתאימה לקטגוריה או תת־קטגוריה שנבחרו.");
        }

        var aiResponse = await CallOpenAiApi(prompt.Prompt1);

        prompt.Response = aiResponse;
        Create(prompt); // [תיקון]: קריאה לפונקציה סינכרונית

        return aiResponse;
    }

    // [תיקון]: הפכנו את הפונקציה ל-public כדי שתתאים לממשק IBLPrompt
    public async Task<string> CallOpenAiApi(string promptText)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[] { new { role = "user", content = promptText } },
            max_tokens = 150
        };

        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiSettings.ApiKey);

        var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OpenAiResponse>(responseBody);

        return result?.choices?[0]?.message?.content ?? string.Empty;
    }

    // [תיקון]: כל הפונקציות הבאות הותאמו לעבודה סינכרונית מול ה-DAL
    // כדי לפתור את שגיאות הקומפילציה.

    public void Create(BLPrompt entity)
    {
        Prompt dalPrompt = ConvertToDal(entity);
        _learningRepository.Create(dalPrompt);
    }

    public BLPrompt Read(int id)
    {
        var dalPrompt = _learningRepository.Read(id);
        if (dalPrompt == null) return null;
        return ConvertToBl(dalPrompt);
    }

    public void Update(BLPrompt entity)
    {
        ValidatePrompt(entity);
        var dalPrompt = ConvertToDal(entity);
        _learningRepository.Update(dalPrompt);
    }

    public void Delete(int id)
    {
        _learningRepository.Delete(id);
    }

    public List<BLPrompt> GetAll()
    {
        return _learningRepository.GetAll().Select(p => ConvertToBl(p)).ToList();
    }

    public List<BLPrompt> GetPromptsByUserId(int userId)
    {
        return _learningRepository.GetAll().Where(p => p.UserId == userId).Select(p => ConvertToBl(p)).ToList();
    }

    public IEnumerable<BLPrompt> GetPromptsByUserIdAndCategory(int userId, int categoryId)
    {
        return _learningRepository.GetAll()
           .Where(p => p.UserId == userId && p.CategoryId == categoryId)
           .Select(p => ConvertToBl(p)).ToList();
    }

    // --- פונקציות פרטיות ---

    private async Task<bool> ValidateRequestRelevance(BLPrompt prompt)
    {
        var category = _categoryRepository.Read(prompt.CategoryId);
        var subCategory = _subCategoryRepository.Read(prompt.SubCategoryId);

        var request = new ValidatePromptRequest
        {
            UserPrompt = prompt.Prompt1,
            CategoryName = category?.Name ?? "Unknown",
            SubCategoryName = subCategory?.Name ?? "Unknown"
        };

        string validationPrompt = $@"
                הטקסט הבא הוא שאלה או בקשה של משתמש:
                ""{request.UserPrompt}""
                האם השאלה קשורה לנושא '{request.CategoryName}' או תת־הנושא '{request.SubCategoryName}'?
                אם כן, ענה 'כן'. אם לא, ענה 'לא'.";

        string aiResponse = await CallOpenAiApi(validationPrompt);
        string answer = aiResponse.Trim().ToLower();

        return answer.StartsWith("כן") || answer.StartsWith("yes");
    }

    private void ValidatePrompt(BLPrompt prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt.Prompt1))
            throw new ValidationException("הטקסט של הפנייה לא יכול להיות ריק.");
        // ... שאר בדיקות הולידציה
    }

    private BLPrompt ConvertToBl(Prompt dalPrompt)
    {
        return new BLPrompt
        {
            Id = dalPrompt.Id,
            UserId = dalPrompt.UserId,
            CategoryId = dalPrompt.CategoryId,
            SubCategoryId = dalPrompt.SubCategoryId,
            Prompt1 = dalPrompt.Prompt1,
            Response = dalPrompt.Response,
            CreatedAt = dalPrompt.CreatedAt
        };
    }

    private Prompt ConvertToDal(BLPrompt blPrompt)
    {
        return new Prompt
        {
            Id = blPrompt.Id,
            UserId = blPrompt.UserId,
            CategoryId = blPrompt.CategoryId,
            SubCategoryId = blPrompt.SubCategoryId,
            Prompt1 = blPrompt.Prompt1,
            Response = blPrompt.Response,
            CreatedAt = blPrompt.CreatedAt == default ? DateTime.UtcNow : blPrompt.CreatedAt
        };
    }
}