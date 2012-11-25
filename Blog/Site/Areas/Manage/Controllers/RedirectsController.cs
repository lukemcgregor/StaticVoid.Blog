using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    [AuthorAuthorize]
    public class RedirectsController : Controller
    {
        private readonly IRepository<Redirect> _redirectRepository;
        public RedirectsController(IRepository<Redirect> redirectRepository) 
        {
            _redirectRepository = redirectRepository;
        }

        public ActionResult Index()
        {
            return View(_redirectRepository.GetRedirects().Select(r=>new RedirectModel{
                From = r.OldRoute,
                To = r.NewRoute,
                Temporary = !r.IsPermanent,
                Id = r.Id
            }));
        }

        public ActionResult Delete(int id)
        {
            var redirect = _redirectRepository.GetBy(r=>r.Id == id);

            if (redirect == null)
            {
                return HttpNotFound();
            }

            return PartialView("DeleteModal", new RedirectModel
            {
                Id = redirect.Id,
                From = redirect.OldRoute,
                To = redirect.NewRoute,
                Temporary = !redirect.IsPermanent
            });
        }

        [HttpPost]
        public ActionResult Delete(int id, RedirectModel model)
        {
            var redirect = _redirectRepository.GetBy(r => r.Id == id);

            if (redirect == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                _redirectRepository.Delete(redirect);
                return Json(new { success = true });
            }

            return PartialView("DeleteModal", new RedirectModel
            {
                Id = redirect.Id,
                From = redirect.OldRoute,
                To = redirect.NewRoute,
                Temporary = !redirect.IsPermanent
            });
        }

        public ActionResult Create()
        {
            return PartialView("CreateModal");
        }

        [HttpPost]
        public ActionResult Create(RedirectModel model)
        {
            if (ModelState.IsValid)
            {
                _redirectRepository.Create(new Redirect
                {
                    IsPermanent = !model.Temporary,
                    OldRoute = model.From,
                    NewRoute = model.To
                });
                return Json(new { success = true });
            }

            return PartialView("CreateModal", model);
        }
    }
}
