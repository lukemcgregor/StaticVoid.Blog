using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Site.Gravitar;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Controllers
{
    public class IndexController : BlogBaseController
    {
        private readonly IRepository<Post> _postRepo;

        public IndexController(IRepository<Post> postRepo, IRepository<Data.Blog> blogRepo, IHttpContextService httpContext)
            : base(blogRepo, httpContext)
        {
            _postRepo = postRepo;
        }
        public ActionResult Posts()
        {
            return View(_postRepo
                .PublishedPosts(CurrentBlog.Id, p => p.Author)
                .OrderByDescending(p=>p.Posted)
                .AsEnumerable()
                .Select(p => new PostRowIndexModel
            {
                Title = p.Title,
                Description = p.Description,
                Url = p.Path,
                Posted = p.Posted,
                Author = new PostAuthor
                {
                    GravatarUrl = p.Author.Email.GravitarUrlFromEmail(),
                    Name = p.Author.FullName(),
                    GooglePlusProfileUrl = p.Author.GooglePlusProfileUrl
                }
            }));
        }
    }
}
