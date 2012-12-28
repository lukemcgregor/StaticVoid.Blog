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
    public class IndexController : Controller
    {
        private readonly IRepository<Post> _postRepo;

        public IndexController(IRepository<Post> postRepo)
        {
            _postRepo = postRepo;
        }
        public ActionResult Posts()
        {
            return View(_postRepo
                .PublishedPosts(p => p.Author)
                .OrderByDescending(p=>p.Posted)
                .Select(p => new PostRowIndexModel
            {
                Title = p.Title,
                Url = p.Path,
                Posted = p.Posted
            }));
        }
    }
}
