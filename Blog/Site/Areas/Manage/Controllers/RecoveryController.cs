using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using StaticVoid.Xml.DataContractSerialiser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public FileResult Backup()
        {
            var backup = new BlogBackup
            {
                Posts = _postRepository.PostsForBlog(CurrentBlog.Id).AsEnumerable(),
                Redirects = _redirectRepository.GetRedirects(CurrentBlog.Id).AsEnumerable()                
            };

            if (CurrentBlog.StyleId.HasValue)
            {
                backup.BlogStyle = _styleRepository.GetById(CurrentBlog.StyleId.Value);
            }

            var xml = backup.Serialise();

            return File(System.Text.Encoding.UTF8.GetBytes(xml), System.Net.Mime.MediaTypeNames.Application.Octet, "test.xml");
        }

        [HttpPost]
        public ActionResult Restore(HttpPostedFileBase file)
        {
            throw new NotImplementedException();
        }

        class BlogBackup
        {
            public IEnumerable<Post> Posts { get; set; }
            public IEnumerable<Redirect> Redirects { get; set; }
            public Style BlogStyle { get; set; }
        }
    }
}
