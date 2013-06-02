using StaticVoid.Blog.Data;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using StaticVoid.Xml.XmlSerialiser;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Controllers
{
    public class SitemapController : BlogBaseController
    {
        private readonly IRepository<Post> _postRepository;

        public SitemapController(
            IRepository<Post> postRepository, 
            IRepository<Data.Blog> blogRepo, 
            IHttpContextService httpContext) : base(blogRepo,httpContext)
        {
            _postRepository = postRepository;
        }

        public ActionResult Sitemap()
        {
            GoogleSiteMap sitemap = new GoogleSiteMap
            {
                Urls = _postRepository.PublishedPosts(CurrentBlog.Id).Select(p => new SiteUrl { Location = string.Format("{0}/{1}", CurrentBlog.AuthoritiveUrl, p.Path.TrimStart('/')) }).ToList()
            };

            sitemap.Urls.Insert(0, new SiteUrl
            {
                Location = String.Format("{0}/Index/Posts", CurrentBlog.AuthoritiveUrl),
                ChangeFrequency = "Daily"
            });

            return Content(sitemap.Serialise(), "text/xml");
        }
    }

    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class GoogleSiteMap
    {
        [XmlElement("url")]
        public List<SiteUrl> Urls { get; set; }
    }

    public class SiteUrl
    {
        [XmlElement("loc")]
        public string Location { get; set; }
        [XmlElement("lastmod")]
        public DateTime? LastModified { get; set; }
        [XmlElement("changefreq")]
        public string ChangeFrequency { get; set; }

        public bool ShouldSerializeLastModified() { return LastModified.HasValue; }
    }
}
