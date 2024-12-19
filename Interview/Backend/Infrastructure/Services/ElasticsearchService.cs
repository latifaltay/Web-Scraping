using Entity;
using Infrastructure.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly ElasticClient _client;

        public ElasticsearchService(string elasticsearchUrl)
        {
            var settings = new ConnectionSettings(new Uri(elasticsearchUrl)).DefaultIndex("news");
            _client = new ElasticClient(settings);
        }

        public void IndexArticles(IEnumerable<Article> articles)
        {
            foreach (var article in articles)
            {
                var response = _client.IndexDocument(article);
                if (!response.IsValid)
                {
                    Console.WriteLine($"Haber başlıkları indexleme işlemi sırasında hata oluştu: {response.OriginalException.Message}");
                }
            }
        }

        public List<Article> SearchArticles(string query)
        {
            var searchResponse = _client.Search<Article>(s => s
                .Index("news")
                .Size(1000)
                .Query(q => q.MatchAll())
            );

            if (!searchResponse.IsValid)
            {
                return new List<Article>();
            }

            return searchResponse.Documents.ToList();
        }

        public bool DoesArticleExist(string title, string url)
        {
            var searchResponse = _client.Search<Article>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Must(
                            m => m.Term(t => t.Field(f => f.Title).Value(title)),
                            m => m.Term(t => t.Field(f => f.Url).Value(url))
                        )
                    )
                )
            );

            return searchResponse.Hits.Any();
        }



        public List<Article> SearchArticlesWithQuery(string query)
        {
            var searchResponse = _client.Search<Article>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            sh => sh.Wildcard(wc => wc
                                .Field(f => f.Title)
                                .Value($"*{query.ToLower()}*")
                            ),
                            sh => sh.Wildcard(wc => wc
                                .Field(f => f.Source)
                                .Value($"*{query.ToLower()}*")
                            )
                        )
                    )
                )
            );

            if (!searchResponse.IsValid)
            {
                Console.WriteLine($"Haber başlığı arama sırasında hata oluştu: {searchResponse.DebugInformation}");
                return new List<Article>();
            }

            return searchResponse.Documents.ToList();
        }

    }
}
