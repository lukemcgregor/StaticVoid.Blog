using dotless.Core;
using StaticVoid.Blog.Data;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Controllers
{
    public class StyleController : Controller
    {
        private readonly IRepository<BlogTemplate> _blogTemplateRepo;

        public StyleController(IRepository<BlogTemplate> blogTemplateRepo)
        {
            _blogTemplateRepo = blogTemplateRepo;
        }

        public ActionResult Css(Guid id)
        {
            var style = _blogTemplateRepo.GetBy(s => s.Id == id);

            if (style == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Resource not found");
            }
            
            return Content(Less.Parse(style.Css), "text/css");
        }

    }
}
