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
    public class UserProfileController : Controller
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Data.Blog> _blogRepo;
        private readonly ISecurityHelper _securityHelper;

        public UserProfileController(IRepository<User> userRepo, IRepository<Data.Blog> blogRepo, ISecurityHelper securityHelper)
        {
            _userRepo = userRepo;
            _blogRepo = blogRepo;
            _securityHelper = securityHelper;
        }

        public ActionResult EditMyProfile()
        {
            var currentUser = _userRepo.GetCurrentUser(_securityHelper);

            return PartialView("EditMyProfileModal", new MyProfileModel { 
                GooglePlusProfileUrl = currentUser.GooglePlusProfileUrl
            });
        }

        [HttpPost]
        public ActionResult EditMyProfile(MyProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _userRepo.GetCurrentUser(_securityHelper);

                currentUser.GooglePlusProfileUrl = model.GooglePlusProfileUrl;

                _userRepo.Update(currentUser);

                return Json(new { success = true });
            }

            return PartialView("EditMyProfileModal", model);
        }

    }
}
