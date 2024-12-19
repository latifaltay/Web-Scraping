using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Pages
{
    public class IndexModel : PageModel
    {
        public List<Article> Articles { get; set; } = new List<Article>();
        public string Message { get; set; }
        public string SearchQuery { get; set; }


        public async Task OnGetAsync(string query = null)
        {
            if (!string.IsNullOrEmpty(query))
            {
                await SearchArticles(query);
            }
            else
            {
                await FetchArticlesFromElasticsearch();
            }
        }


        public async Task<IActionResult> OnGetSearchAsync(string query)
        {
            SearchQuery = query;

            if (string.IsNullOrWhiteSpace(query))
            {
                Message = "Lütfen bir arama terimi girin.";
                Articles = new List<Article>();
            }
            else
            {
                await SearchArticles(query);
            }

            return Page();
        }


        public async Task OnPostCrawlAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7143");

            var scrapeResponse = await client.GetAsync("/api/articles/GetScraping");
            if (scrapeResponse.IsSuccessStatusCode)
            {
                var scrapeJson = await scrapeResponse.Content.ReadAsStringAsync();
                var scrapeResult = JsonSerializer.Deserialize<ApiResult>(scrapeJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Message = scrapeResult?.Message ?? "Scrap işlemi başarıyla tamamlandı";
            }
            else
            {
                Message = "Scrap işlemi sırasında bir hata ile karşılaşıldı";
            }

        }


        public async Task FetchArticlesFromElasticsearch()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7143");

            var response = await client.GetAsync("/api/Articles/GetAllArticles");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResult>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Data != null)
                {
                    Articles = result.Data;
                }
            }
        }

        public async Task SearchArticles(string query)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7143");

            var response = await client.GetAsync($"/api/Articles/GetArticlesByQuery?query={Uri.EscapeDataString(query)}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResult>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Data != null)
                {
                    Articles = result.Data;
                    Message = result.Message;
                }
                else
                {
                    Articles = new List<Article>();
                    Message = "Aranan öğe bulunamadı.";
                }
            }
            else
            {
                Articles = new List<Article>();
                Message = "Aranan öğe bulunamadı.";
            }
        }

        public class ApiResult
        {
            public string Message { get; set; }
            public List<Article> Data { get; set; }
        }

        public class Article
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string Source { get; set; }
            public DateTime PublishedDate { get; set; }
        }
    }
}
