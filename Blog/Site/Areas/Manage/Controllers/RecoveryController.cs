using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using StaticVoid.Xml.DataContractSerialiser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Text;
using System.Net.Mime;
using System.Runtime.Serialization;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Blog.Site.Security;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    [CurrentBlogAuthorAuthorize]
    public class RecoveryController : ManageBaseController
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Redirect> _redirectRepository;
        private readonly IRepository<BlogTemplate> _styleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<TemporaryUploadedBlogBackup> _tempBlogBackupRepo;
        private readonly IRepository<Data.Blog> _blogRepository;
        private readonly ISecurityHelper _securityHelper;

        public RecoveryController(
            IRepository<Post> postRepository,
            IRepository<Redirect> redirectRepository,
            IRepository<BlogTemplate> styleRepository,
            IRepository<User> userRepository,
            IRepository<Securable> securableRepository,
            IRepository<TemporaryUploadedBlogBackup> tempBlogBackupRepo,
            IRepository<Data.Blog> blogRepository,
            ISecurityHelper securityHelper,
            IHttpContextService httpContext)
            : base(blogRepository, httpContext, securityHelper, userRepository, securableRepository)
        {
            _postRepository = postRepository;
            _redirectRepository = redirectRepository;
            _styleRepository = styleRepository;
            _tempBlogBackupRepo = tempBlogBackupRepo;
            _userRepository = userRepository;
            _blogRepository = blogRepository;
            _securityHelper = securityHelper;

            //Naieve clean of the collection to avoid leaks in long running instances of the application
            var toDelete = _tempBlogBackupRepo.GetAll().Where(b => b.UploadTime < DateTime.Now.AddHours(-1)).ToArray();
            foreach(var toDel in toDelete)
            {
                _tempBlogBackupRepo.Delete(toDel);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Recovery()
        {
            return PartialView("Recovery");
        }

        public ActionResult Restore()
        {
            return PartialView("Restore");
        }

        [HttpPost]
        public ActionResult Restore(string correlationToken, HttpPostedFileBase file)
        {
            var xml = Encoding.UTF8.GetString(file.InputStream.ToBytes());

            var backup = xml.Deserialise<BlogBackup>();

            _tempBlogBackupRepo.Create(new TemporaryUploadedBlogBackup
            {
                UploadTime = DateTime.Now,
                BlogId = CurrentBlog.Id,
                CorrelationToken = correlationToken,
                Backup = backup
            });

            var currentPosts = _postRepository.PostsForBlog(CurrentBlog.Id).ToList();

            return Json(new { posts = backup.Posts.Select(p => new { p.Title, p.Id, p.PostGuid }) });
        }

        [HttpPost]
        [ActionName("restore-posts")]
        public ActionResult RestorePosts(string correlationToken, List<Guid> postsToRestore)
        {
            var archive = _tempBlogBackupRepo.GetAll().FirstOrDefault(r => r.BlogId == CurrentBlog.Id && r.CorrelationToken == correlationToken);

            if (archive == null)
            {
                return Json(false);
            }

            foreach (var postGuid in postsToRestore)
            {
                var existingPost = _postRepository.GetByGuid(CurrentBlog.Id, postGuid);
                var toRestore = archive.Backup.Posts.Single(p => p.PostGuid == postGuid);
                if (existingPost != null)
                {
                    existingPost.Path = toRestore.Path;
                    existingPost.Posted = toRestore.Posted;
                    existingPost.Status = toRestore.Status;
                    existingPost.Title = toRestore.Title;
                    existingPost.Body = toRestore.Body;
                    existingPost.Canonical = toRestore.Canonical;
                    existingPost.Description = toRestore.Description;
                    existingPost.DraftBody = toRestore.DraftBody;
                    existingPost.DraftDescription = toRestore.DraftDescription;
                    existingPost.DraftTitle = toRestore.DraftTitle;
                    existingPost.ExcludeFromFeed = toRestore.ExcludeFromFeed;

                    _postRepository.Update(existingPost);
                }
                else
                {
                    _postRepository.Create(new Post
                    {
                        //TODO make this recover based on who origionally posted the article
                        AuthorId = _userRepository.GetCurrentUser(_securityHelper).Id,
                        BlogId = CurrentBlog.Id,
                        Path = toRestore.Path,
                        Posted = toRestore.Posted,
                        Status = toRestore.Status,
                        Title = toRestore.Title,
                        Body = toRestore.Body,
                        Canonical = toRestore.Canonical,
                        Description = toRestore.Description,
                        DraftBody = toRestore.DraftBody,
                        DraftDescription = toRestore.DraftDescription,
                        DraftTitle = toRestore.DraftTitle,
                        ExcludeFromFeed = toRestore.ExcludeFromFeed
                    });
                }
            }

            return Json(true);
        }

        public FileResult Backup()
        {
            var backup = new BlogBackup
            {
                Posts = _postRepository.PostsForBlog(CurrentBlog.Id).ToList(),
                Redirects = _redirectRepository.GetRedirects(CurrentBlog.Id).ToList()
            };

            if (CurrentBlog.BlogTemplateId.HasValue)
            {
                backup.BlogTemplate = _styleRepository.GetById(CurrentBlog.BlogTemplateId.Value);
            }

            var xml = backup.Serialise();

            return File(Encoding.UTF8.GetBytes(xml), MediaTypeNames.Application.Octet, "test.xml");
        }
    }
}
