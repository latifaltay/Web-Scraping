﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Article
    {
        public string Id { get; set; } 
        public string Title { get; set; } 
        public string Url { get; set; } 
        public DateTime PublishedDate { get; set; } 
        public string Source { get; set; }
    }
}
