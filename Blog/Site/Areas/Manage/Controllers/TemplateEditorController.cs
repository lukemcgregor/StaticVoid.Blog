using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    [CurrentBlogAuthorAuthorize]
    public class TemplateEditorController : ManageBaseController
    {
        private readonly IRepository<BlogTemplate> _styleRepo;
        private readonly IRepository<Data.Blog> _blogRepo;
        private readonly IHttpContextService _httpContext;

        public TemplateEditorController(
            IRepository<BlogTemplate> styleRepo, 
            IRepository<Data.Blog> blogRepo,
            IHttpContextService httpContext,
            IRepository<User> userRepository,
            IRepository<Securable> securableRepository,
            ISecurityHelper securityHelper)
            : base(blogRepo, httpContext, securityHelper, userRepository, securableRepository)
        {
            _styleRepo = styleRepo;
            _blogRepo = blogRepo;
            _httpContext = httpContext;
        }

        public ActionResult Index(Guid? id)
        {
            if (!id.HasValue)
            {
                var latestTemplate = _styleRepo.GetLatestEditForBlog(CurrentBlog.Id);
                
                if (latestTemplate != null)
                {
                    return RedirectToAction("Index", new { id = latestTemplate.Id });
                }

                //TODO move to some kinda shared thingimibob
                var defaultHtmlTemplate = "";
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StaticVoid.Blog.Site.Defaults.DefaultTemplate.html"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    defaultHtmlTemplate = reader.ReadToEnd();
                }

                var defaultBlogTemplate =new BlogTemplate
                {
                    //todo
                    BlogId = CurrentBlog.Id,
                    TemplateMode = Data.TemplateMode.NoDomCustomisation,
                    HtmlTemplate = defaultHtmlTemplate,
                    LastModified = DateTime.Now
                };

                _styleRepo.Create(defaultBlogTemplate);

                return RedirectToAction("Index", new { id = defaultBlogTemplate.Id });
            }
            var style = _styleRepo.GetBy(s => s.Id == id);

            return View(new StyleModel { Css = style.Css, HtmlTemplate = style.HtmlTemplate, TemplateMode = style.TemplateMode });
        }

        [HttpPost]
        [ActionName("save-blog-template")]
        public ActionResult SaveBlogTemplate(StyleModel model)
        {
            if (ModelState.IsValid)
            {
                var blogTemplate = _styleRepo.GetLatestEditForBlog(CurrentBlog.Id);
                if (CurrentBlog.BlogTemplateId == blogTemplate.Id)
                {
                    if (blogTemplate.Css != model.Css ||
                        blogTemplate.HtmlTemplate != model.HtmlTemplate ||
                        blogTemplate.TemplateMode != model.TemplateMode)
                    {
                        //create a new version
                        _styleRepo.Create(new BlogTemplate
                        {
                            Css = model.Css,
                            HtmlTemplate = model.HtmlTemplate,
                            TemplateMode = model.TemplateMode,
                            LastModified = DateTime.Now,
                            BlogId = CurrentBlog.Id
                        });
                    }
                }
                else
                {

                    blogTemplate.Css = model.Css;
                    blogTemplate.HtmlTemplate = model.HtmlTemplate;
                    blogTemplate.TemplateMode = model.TemplateMode;
                    blogTemplate.LastModified = DateTime.Now;
                    _styleRepo.Update(blogTemplate);
                }

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
