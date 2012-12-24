using StaticVoid.Blog.Site.Security;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    [AuthorAuthorize]
    public class HacksController : Controller
    {
        private readonly IRepository<Data.User> _userRepo;
        private readonly IRepository<Data.Post> _postRepo;

        public HacksController(IRepository<Data.User> userRepo, IRepository<Data.Post> postRepo)
        {
            _userRepo = userRepo;
            _postRepo = postRepo;
        }

        public ActionResult FixDuplicateAuthors()
        {
            foreach (var duplicates in _userRepo.GetAll().GroupBy(g => g.ClaimedIdentifier).Where(g => g.Count() > 1))
            {
                var real = duplicates.First();
                var toRemove = duplicates.Skip(1);
                foreach(var u in toRemove.ToList())
                {
                    foreach (var post in _postRepo.GetAll().Where(p => p.AuthorId == u.Id))
                    {
                        post.AuthorId = real.Id;
                        _postRepo.Update(post);
                    }
                    _userRepo.Delete(u);
                }
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
