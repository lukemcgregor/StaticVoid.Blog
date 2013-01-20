using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    public class StyleEditorController : Controller
    {
        private readonly IRepository<Style> _styleRepo;
        private readonly IRepository<Data.Blog> _blogRepo;

        public StyleEditorController(IRepository<Style> styleRepo, IRepository<Data.Blog> blogRepo)
        {
            _styleRepo = styleRepo;
            _blogRepo = blogRepo;
        }

        public ActionResult Edit(Guid id)
        {
            var style = _styleRepo.GetBy(s => s.Id == id);

            return PartialView("EditStyleModal", new StyleModel { Css = style.Css });
        }

        [HttpPost]
        public ActionResult Edit(Guid id, StyleModel model)
        {
            if (ModelState.IsValid)
            {
                var style = _styleRepo.GetBy(s => s.Id == id);

                style.Css = model.Css;
                _styleRepo.Update(style);

                return Json(new { success = true });
            }

            return PartialView("EditStyleModal", model);
        }

        public ActionResult EditBlogStyle()
        {
            var currentBlog = _blogRepo.CurrentBlog();
            if (!currentBlog.StyleId.HasValue)
            {
                var blogStyle = new Style();

                _styleRepo.Create(blogStyle);

                currentBlog.StyleId = blogStyle.Id;
                _blogRepo.Update(currentBlog);
            }
            return RedirectToAction("Edit", new { id = currentBlog.StyleId });
        }
    }
}
