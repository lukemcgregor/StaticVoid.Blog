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
    public class RedirectController : Controller
    {       
        private readonly IRepository<Redirect> _redirectRepository;

        public RedirectController(IRepository<Redirect> redirectRepository)
        {
            _redirectRepository = redirectRepository;
        }

        public ActionResult ProcessRedirect(string path)
        {
            var redirect = _redirectRepository.GetRedirectFor(path);

            if (redirect == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Resource not found");
            }
            else if(!redirect.NewRoute.StartsWith("/"))
            {
                redirect.NewRoute =  "/" + redirect.NewRoute;
            }

            if(redirect.IsPermanent)
            {
                return RedirectPermanent(redirect.NewRoute);
            }
            else
            {
                return Redirect(redirect.NewRoute);
            }
        }

    }
}
