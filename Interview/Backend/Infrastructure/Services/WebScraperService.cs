using Entity;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Infrastructure.Services
{

    public class WebScraperService : IWebScraperService
    {
        private readonly string _url = "https://www.sozcu.com.tr/gundem";
        private readonly IElasticsearchService _elasticsearchService;

        public WebScraperService(IElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        public List<string> CrawlSozcuHeadlines()
        {
            var headlines = new List<string>();

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");

            var driver = new ChromeDriver(options);

            try
            {
                driver.Navigate().GoToUrl(_url);

                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElements(By.XPath("//a[@class='d-block']/span[@class='d-block fs-5 fw-semibold text-truncate-2']")).Count > 0);

                var nodes = driver.FindElements(By.XPath("//a[@class='d-block']/span[@class='d-block fs-5 fw-semibold text-truncate-2']"));

                foreach (var node in nodes)
                {
                    var title = node.Text.Trim();
                    var link = node.FindElement(By.XPath("..")).GetAttribute("href");

                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(link))
                    {
                        if (!_elasticsearchService.DoesArticleExist(title, link))
                        {
                            headlines.Add($"{title} - {link}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
            }
            finally
            {
                driver.Quit(); 
            }

            return headlines;
        }
    }

}