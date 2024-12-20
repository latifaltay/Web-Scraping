﻿using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IElasticsearchService
    {
        void IndexArticles(List<Article> articles); 
        List<Article> ListArticles(string query);       
        bool DoesArticleExist(string title, string url); 
        List<Article> SearchArticlesWithQuery(string query); 
    }
}
