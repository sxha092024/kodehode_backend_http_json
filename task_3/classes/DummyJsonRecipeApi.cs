using System.Collections.Specialized;
using System.Text.Json;
using System.Web;
using DummyJson;

public class DummyJsonRecipeApi
{
    // field name is a slight misnomer, really it should be the root domain, 
    // and then service path extensions. However, instead of fully implementing
    // an API wrapper client, we're fudging some details.
    static string BaseUrl = "https://dummyjson.com/recipes";

    internal HttpClient httpClient = new HttpClient();
    public async Task<DummyJsonRecipe> GetOne()
    {
        HttpResponseMessage resp = await httpClient.GetAsync(httpClient.BaseAddress + "/1");
        string text = await resp.Content.ReadAsStringAsync();

        DummyJsonRecipe? a = JsonSerializer.Deserialize<DummyJsonRecipe>(text);
        if (a != null)
        {
            return a;
        }
        else
        {
            FileStream fs = File.Open("./erroring_responses.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.AutoFlush = true;
            await sw.WriteLineAsync(text);
            throw new Exception($"Could not parse response:\n`{text}`");
        }
    }

    public async Task<Results> GetMany(int limit = 30, int skip = 0)
    {
        NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("limit", limit.ToString());
        if (skip > 0)
        {
            queryString.Add("skip", skip.ToString());
        }
        Uri requestUri = new Uri($"{BaseUrl}?{queryString.ToString()}");

        string textResponse = await httpClient.GetStringAsync(requestUri);

        Results? recipes = JsonSerializer.Deserialize<Results>(textResponse);
        if (recipes != null && recipes.total > 0)
        {
            return recipes;
        }
        else
        {
            FileStream fs = File.Open("./erroring_responses.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.AutoFlush = true;
            await sw.WriteLineAsync(textResponse);
            throw new Exception($"Could not parse response:\n`{textResponse}`");
        }
    }

    public DummyJsonRecipeApi()
    {
        httpClient.BaseAddress = new System.Uri(BaseUrl);
    }
}