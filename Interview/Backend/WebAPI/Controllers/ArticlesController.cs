using Entity;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IWebScraperService _webScraperService;
        private readonly IElasticsearchService _elasticsearchService;

        public ArticlesController(IWebScraperService webScraperService, IElasticsearchService elasticsearchService)
        {
            _webScraperService = webScraperService;
            _elasticsearchService = elasticsearchService;
        }

        [HttpGet]
        public IActionResult GetScraping()
        {
            try
            {
                var headlines = _webScraperService.CrawlSozcuHeadlines();

                if (headlines != null && headlines.Any())
                {
                    var articles = headlines.Select(h =>
                    {
                        var split = h.Split(" - ");

                        var url = split.Length > 1 ? split[1] : "#";

                        var uri = new Uri(url);
                        var source = uri.Host.Replace("www.", ""); 

                        return new Article
                        {
                            Title = split[0],
                            Url = split.Length > 1 ? split[1] : "#",
                            PublishedDate = DateTime.Now,
                            Source = source
                        };
                    }).ToList();

                    _elasticsearchService.IndexArticles(articles);

                    return Ok(new
                    {
                        Message = "Haber Başlıkları başarılı bir şekilde getirildi",
                        Data = articles
                    });
                }
                else
                {
                    return NotFound(new { Message = "Haber başlığı bulunamadı" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllArticles()
        {
            try
            {
                var articles = _elasticsearchService.ListArticles("*");

                if (articles != null && articles.Any())
                {
                    return Ok(new
                    {
                        Message = "Haber Başlıkları listelendi",
                        Data = articles
                    });
                }
                else
                {
                    return NotFound(new { Message = "Elasticsearch'te haber başlığı bulunamadı" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetArticlesByQuery(string query)
        {
            try
            {
                var articles = _elasticsearchService.SearchArticlesWithQuery(query);

                if (articles != null && articles.Any())
                {
                    return Ok(new
                    {
                        Data = articles
                    });
                }
                else
                {
                    return NotFound(new { Message = "Veri bulunamadı" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
