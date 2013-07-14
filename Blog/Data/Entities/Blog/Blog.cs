﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid BlogGuid { get; set; }
        public string Twitter { get; set; }
        public string AuthoritiveUrl { get; set; }
        public string AnalyticsKey { get; set; }
        public string DisqusShortname { get; set; }
        public Guid? BlogTemplateId { get; set; }
        public BlogTemplate BlogTemplate { get; set; }
        public int AdminSecurableId { get; set; }
        public Securable AdminSecurable { get; set; }
        public int AuthorSecurableId { get; set; }
        public Securable AuthorSecurable { get; set; }
    }
}
