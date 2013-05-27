using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Site.Gravitar;

namespace StaticVoid.Blog.Site.Controllers
{
    public class IndexController : BlogBaseController
    {
        private readonly IRepository<Post> _postRepo;
        private readonly IRepository<Data.Blog> _blogRepo;

        public IndexController(IRepository<Post> postRepo, IRepository<Data.Blog> blogRepo)
            : base(blogRepo)
        {
            _postRepo = postRepo;
            _blogRepo = blogRepo;
        }
        public ActionResult Posts()
        {
            return View(_postRepo
                .PublishedPosts(p => p.Author)
                .OrderByDescending(p=>p.Posted)
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
