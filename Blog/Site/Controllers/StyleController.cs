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
        private readonly IRepository<Style> _styleRepo;

        public StyleController(IRepository<Style> styleRepo)
        {
            _styleRepo = styleRepo;
        }

        public ActionResult Css(Guid id)
        {
            var style = _styleRepo.GetBy(s => s.Id == id);

            if (style == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Resource not found");
            }
            
            return Content(Less.Parse(style.Css), "text/css");
        }

    }
}
