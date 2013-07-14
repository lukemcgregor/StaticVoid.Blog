using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
    public class BlogTemplate
    {
        public Guid Id { get; set; }
        public TemplateMode TemplateMode { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public string Css { get; set; }
        public string HtmlTemplate { get; set; }
        public DateTime LastModified { get; set; }
    }

    public enum TemplateMode
    {
        NoscriptMetaRefresh,
        DomRip,
        BodyOnly,
        NoNojsFallback,
        NoDomCustomisation
    }
}
