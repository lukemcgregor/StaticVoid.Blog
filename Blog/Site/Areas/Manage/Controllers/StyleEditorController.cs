using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    public class StyleEditorController : BlogBaseController
    {
        private readonly IRepository<BlogTemplate> _styleRepo;
        private readonly IRepository<Data.Blog> _blogRepo;
        private readonly IHttpContextService _httpContext;

        public StyleEditorController(
            IRepository<BlogTemplate> styleRepo, 
            IRepository<Data.Blog> blogRepo,
            IHttpContextService httpContext) : base(blogRepo, httpContext)
        {
            _styleRepo = styleRepo;
            _blogRepo = blogRepo;
            _httpContext = httpContext;
        }

        public ActionResult Edit(Guid? id)
        {
            if (!id.HasValue)
            {
                var latestTemplate = _styleRepo.GetLatestEditForBlog(CurrentBlog.Id);

                if (latestTemplate != null)
                {
                    return RedirectToAction("Edit", new { id = latestTemplate.Id });
                }

                var defaultTemplate =new BlogTemplate
                {
                    //todo
                    BlogId = CurrentBlog.Id,
                    TemplateMode = Data.TemplateMode.NoDomCustomisation,
                    LastModified = DateTime.Now
                };

                _styleRepo.Create(defaultTemplate);

                return RedirectToAction("Edit", new { id = defaultTemplate.Id });
            }
            var style = _styleRepo.GetBy(s => s.Id == id);

            return View(new StyleModel { Css = style.Css });
        }

        [HttpPost]
        [ActionName("save-blog-template")]
        public ActionResult SaveBlogTemplate(StyleModel model)
        {
            if (ModelState.IsValid)
            {
                var blogTemplate = _styleRepo.GetLatestEditForBlog(CurrentBlog.Id);
                if(CurrentBlog.BlogTemplateId == blogTemplate.Id)
                {
                    if( blogTemplate.Css== model.Css &&
                        blogTemplate.HtmlTemplate == model.HtmlTemplate &&
                        blogTemplate.TemplateMode == model.TemplateMode)
                    {
                        return Json(new { success = true }); //no modifications so no need to save
                    }

                    //create a new version
                    throw new NotImplementedException();
                }

                blogTemplate.Css = model.Css;
                blogTemplate.HtmlTemplate = model.HtmlTemplate;
                blogTemplate.TemplateMode = model.TemplateMode;
                blogTemplate.LastModified = DateTime.Now;
                _styleRepo.Update(blogTemplate);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        
        [HttpPost]
        [ActionName("apply-blog-template")]
        public ActionResult ApplyBlogTemplate(StyleModel model)
        {
            var currentBlog = _blogRepo.GetCurrentBlog(_httpContext);

            if (ModelState.IsValid)
            {
                var blogTemplate = _styleRepo.GetLatestEditForBlog(currentBlog.Id);
                if (currentBlog.BlogTemplateId == blogTemplate.Id)
                {
                    //no need to apply we already are
                    return Json(new { success = true });
                }

                blogTemplate.Css = model.Css;
                blogTemplate.HtmlTemplate = model.HtmlTemplate;
                blogTemplate.TemplateMode = model.TemplateMode;
                blogTemplate.LastModified = DateTime.Now;
                _styleRepo.Update(blogTemplate);

                currentBlog.BlogTemplateId = blogTemplate.Id;
                _blogRepo.Update(currentBlog);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}
