using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using StaticVoid.Xml.DataContractSerialiser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Text;
using System.Net.Mime;
using System.Runtime.Serialization;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    public class RecoveryController : BlogBaseController
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Redirect> _redirectRepository;
        private readonly IRepository<Style> _styleRepository;
        private readonly IRepository<Data.Blog> _blogRepository;
        private readonly ISecurityHelper _securityHelper;

        public RecoveryController(
            IRepository<Post> postRepository,
            IRepository<Redirect> redirectRepository,
            IRepository<Style> styleRepository,
            IRepository<Data.Blog> blogRepository,
            ISecurityHelper securityHelper,
            IHttpContextService httpContext)
            : base(blogRepository, httpContext)
        {
            _postRepository = postRepository;
            _redirectRepository = redirectRepository;
            _styleRepository = styleRepository;
            _blogRepository = blogRepository;
            _securityHelper = securityHelper;
        }

        public ActionResult Recovery()
        {
            return PartialView("Recovery");
        }

        public ActionResult Restore()
        {
            return PartialView("Restore");
        }

        [HttpPost]
        public ActionResult Restore(string correlationToken, HttpPostedFileBase file)
        {
            var xml = Encoding.UTF8.GetString(file.InputStream.ToBytes());

            var backup = xml.Deserialise<BlogBackup>();

            return Json(new { posts = backup.Posts.Select(p=> new{ p.Title }) });
        }

        public FileResult Backup()
        {
            var backup = new BlogBackup
            {
                Posts = _postRepository.PostsForBlog(CurrentBlog.Id).ToList(),
                Redirects = _redirectRepository.GetRedirects(CurrentBlog.Id).ToList()                
            };

            if (CurrentBlog.StyleId.HasValue)
            {
                backup.BlogStyle = _styleRepository.GetById(CurrentBlog.StyleId.Value);
            }

            var xml = backup.Serialise();

            return File(Encoding.UTF8.GetBytes(xml), MediaTypeNames.Application.Octet, "test.xml");
        }


        
    }
    public class BlogBackup
        {
            public List<Post> Posts { get; set; }
            public List<Redirect> Redirects { get; set; }
            public Style BlogStyle { get; set; }
        }
}
