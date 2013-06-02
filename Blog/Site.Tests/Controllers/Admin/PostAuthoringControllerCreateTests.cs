using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using StaticVoid.Blog.Site.Gravitar;
using StaticVoid.Blog.Site.Areas.Manage.Controllers;
using StaticVoid.Mockable;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Tests.Controllers
{
    [TestClass]
    public class PostAuthoringControllerCreateTests
    {
        private IRepository<Post> _postRepo;
        private IRepository<Data.Blog> _blogRepo;
        private IRepository<PostModification> _postModificationRepo;
        private Mock<ISecurityHelper> _mockSecurityHelper;
        private IRepository<Data.User> _userRepo;
        private IRepository<Data.Redirect> _redirectRepo;
        private Mock<IHttpContextService> _mockHttpContext;
        
        [TestInitialize]
        public void Initialise()
        {
            _postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { Id=1, Status = PostStatus.Published, Title = "some-other-post", Path ="2013/04/9/some-other-post", Posted = new DateTime(2013,4,9), Author = new User{ Email = "" } }
            }));
            _blogRepo  = new SimpleRepository<Data.Blog>(new InMemoryRepositoryDataSource<Data.Blog>(new List<Data.Blog> { 
                new Data.Blog { Id =1, AuthoritiveUrl = "http://anotherblog.test.con" }, 
                new Data.Blog { Id =2, AuthoritiveUrl = "http://blog.test.con" } }));
            _postModificationRepo = new SimpleRepository<Data.PostModification>(
                new InMemoryRepositoryDataSource<Data.PostModification>(new List<Data.PostModification> { 
                    new Data.PostModification { } 
                }));
            _mockSecurityHelper = new Mock<ISecurityHelper>();
            _mockSecurityHelper.Setup(s => s.CurrentUser).Returns(new OpenIdUser("") { ClaimedIdentifier = "zzz" });

            _userRepo = new SimpleRepository<Data.User>(new InMemoryRepositoryDataSource<Data.User>(new List<Data.User> { new Data.User { 
                ClaimedIdentifier = "zzz", 
                Email = "joe@bloggs.com"
            }}));
            _redirectRepo = new SimpleRepository<Data.Redirect>(new InMemoryRepositoryDataSource<Data.Redirect>(new List<Data.Redirect> { new Data.Redirect { } }));
            _mockHttpContext = new Mock<IHttpContextService>();
            _mockHttpContext.Setup(h => h.RequestUrl).Returns(new Uri("http://blog.test.con/blah"));

        }

        [TestMethod]
        public void CreatePostContentTest()
        {
            PostAuthoringController sut = new PostAuthoringController(
                _postRepo, 
                _postModificationRepo, 
                _userRepo, 
                _redirectRepo, 
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            var result = sut.Create(new Areas.Manage.Models.PostEditModel { 
                Title = "Test Title", 
                Body = "Test Body", 
                Description = "Test Description", 
                Reposted = false }) as RedirectToRouteResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Dashboard", result.RouteValues["controller"]);

            Assert.AreEqual(2, _postRepo.GetAll().Count());
            Assert.IsNull(_postRepo.GetAll().Last().Title);
            Assert.IsNull(_postRepo.GetAll().Last().Body);
            Assert.IsNull(_postRepo.GetAll().Last().Description);
            Assert.AreEqual("Test Title", _postRepo.GetAll().Last().DraftTitle);
            Assert.AreEqual("Test Body", _postRepo.GetAll().Last().DraftBody);
            Assert.AreEqual("Test Description", _postRepo.GetAll().Last().DraftDescription);
        }

        [TestMethod]
        public void CreatePostAttachedToCurrentBlog()
        {
            PostAuthoringController sut = new PostAuthoringController(
                _postRepo,
                _postModificationRepo,
                _userRepo,
                _redirectRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            var result = sut.Create(new Areas.Manage.Models.PostEditModel
            {
                Title = "Test Title",
                Body = "Test Body",
                Description = "Test Description",
                Reposted = false
            }) as RedirectToRouteResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Dashboard", result.RouteValues["controller"]);

            Assert.AreEqual(2, _postRepo.GetAll().Count());
            Assert.AreEqual(2, _postRepo.GetAll().Last().BlogId);
        }

        [TestMethod]
        public void CreatePostAuthorTest()
        {
            var userRepo = new SimpleRepository<Data.User>(new InMemoryRepositoryDataSource<Data.User>(new List<Data.User> { new Data.User { 
                Id = 1,
                ClaimedIdentifier = "zzz", 
                Email = "joe@bloggs.com"
            }}));

            PostAuthoringController sut = new PostAuthoringController(
                _postRepo,
                _postModificationRepo,
                userRepo,
                _redirectRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new MockDateTimeProvider(new DateTime(2013, 1, 2)),
                _mockHttpContext.Object);

            var result = sut.Create(new Areas.Manage.Models.PostEditModel
            {
                Title = "Test Title",
                Body = "Test Body",
                Description = "Test Description",
                Reposted = true,
                CanonicalUrl = "http://someotherblog.com/post"
            }) as RedirectToRouteResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Dashboard", result.RouteValues["controller"]);

            Assert.AreEqual(2, _postRepo.GetAll().Count());
            Assert.AreEqual(1, _postRepo.GetAll().Last().AuthorId);
        }

        [TestMethod]
        public void CreatePostUrlNotRepostedTest()
        {
            PostAuthoringController sut = new PostAuthoringController(
                _postRepo,
                _postModificationRepo,
                _userRepo,
                _redirectRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new MockDateTimeProvider(new DateTime(2013, 1, 2)),
                _mockHttpContext.Object);

            var result = sut.Create(new Areas.Manage.Models.PostEditModel
            {
                Title = "Test Title",
                Body = "Test Body",
                Description = "Test Description",
                Reposted = false,
                CanonicalUrl = "http://someotherblog.com/post"
            }) as RedirectToRouteResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Dashboard", result.RouteValues["controller"]);

            Assert.AreEqual(2, _postRepo.GetAll().Count());
            Assert.AreEqual("2013/1/2/test_title", _postRepo.GetAll().Last().Path);
            Assert.AreEqual("/2013/1/2/test_title", _postRepo.GetAll().Last().Canonical);
        }

        [TestMethod]
        public void CreatePostRepostedTest()
        {
            PostAuthoringController sut = new PostAuthoringController(
                _postRepo,
                _postModificationRepo,
                _userRepo,
                _redirectRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new MockDateTimeProvider(new DateTime(2013, 1, 2)),
                _mockHttpContext.Object);

            var result = sut.Create(new Areas.Manage.Models.PostEditModel
            {
                Title = "Test Title",
                Body = "Test Body",
                Description = "Test Description",
                Reposted = true,
                CanonicalUrl = "http://someotherblog.com/post"
            }) as RedirectToRouteResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Dashboard", result.RouteValues["controller"]);

            Assert.AreEqual(2, _postRepo.GetAll().Count());
            Assert.AreEqual("http://someotherblog.com/post", _postRepo.GetAll().Last().Canonical);
        }
    }
}
