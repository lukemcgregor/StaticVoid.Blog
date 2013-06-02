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
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Tests.Controllers
{
    [TestClass]
    public class PostAuthoringControllerUnPublishTests
    {
        private IRepository<Data.Blog> _blogRepo;
        private IRepository<PostModification> _postModificationRepo;
        private Mock<ISecurityHelper> _mockSecurityHelper;
        private IRepository<Data.User> _userRepo;
        private IRepository<Data.Redirect> _redirectRepo;
        private Mock<IHttpContextService> _mockHttpContext;
        
        [TestInitialize]
        public void Initialise()
        {
            _blogRepo = new SimpleRepository<Data.Blog>(new InMemoryRepositoryDataSource<Data.Blog>(new List<Data.Blog> { 
                new Data.Blog { Id = 1, AuthoritiveUrl = "http://blog.test.con" },
                new Data.Blog { Id = 2, AuthoritiveUrl = "http://anotherblog.test.con" } 
            }));
            _postModificationRepo = new SimpleRepository<Data.PostModification>(new InMemoryRepositoryDataSource<Data.PostModification>());
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
        public void UnPublishPublishPostIsPublishedNoDraftContent()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Published, 
                    Title = "Test Title", 
                    Description = "Test Description", 
                    Body = "Test Body", 
                    Path ="2013/04/9/some-other-post", 
                    Posted = new DateTime(2013,4,9), 
                    Author = new User{ Email = "" },
                    BlogId = 1 
                }}));

            PostAuthoringController sut = new PostAuthoringController(
                postRepo, 
                _postModificationRepo, 
                _userRepo, 
                _redirectRepo, 
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            var result = sut.ConfirmUnPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.AreEqual(PostStatus.Unpublished, postRepo.GetAll().First().Status);
            Assert.AreEqual("Test Title", postRepo.GetAll().First().DraftTitle);
            Assert.AreEqual("Test Description", postRepo.GetAll().First().DraftDescription);
            Assert.AreEqual("Test Body", postRepo.GetAll().First().DraftBody);
        }

        [TestMethod]
        public void UnPublishPublishPostIsPublishedNoDraftContent_PostModification()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Published, 
                    Title = "Test Title", 
                    Description = "Test Description", 
                    Body = "Test Body", 
                    Path ="2013/04/9/some-other-post", 
                    Posted = new DateTime(2013,4,9), 
                    Author = new User{ Email = "" },
                    BlogId = 1
                }}));

            PostAuthoringController sut = new PostAuthoringController(
                postRepo,
                _postModificationRepo,
                _userRepo,
                _redirectRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            var result = sut.ConfirmUnPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, _postModificationRepo.GetAll().Count());
            Assert.IsTrue(_postModificationRepo.GetAll().First().StatusModified);
            Assert.AreEqual(PostStatus.Unpublished, _postModificationRepo.GetAll().First().NewStatus);
        }

        [TestMethod]
        public void UnPublishPublishPostIsPublishedDraftContent()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Published, 
                    Title = "Test Title", 
                    DraftTitle = "Draft Title", 
                    Description = "Test Description", 
                    DraftDescription = "Draft Description", 
                    Body = "Test Body", 
                    DraftBody = "Draft Body", 
                    Path ="2013/04/9/some-other-post", 
                    Posted = new DateTime(2013,4,9), 
                    Author = new User{ Email = "" },
                    BlogId = 1 
                }}));

            PostAuthoringController sut = new PostAuthoringController(
                postRepo,
                _postModificationRepo,
                _userRepo,
                _redirectRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            var result = sut.ConfirmUnPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.AreEqual(PostStatus.Unpublished, postRepo.GetAll().First().Status);
            Assert.AreEqual("Draft Title", postRepo.GetAll().First().DraftTitle);
            Assert.AreEqual("Draft Description", postRepo.GetAll().First().DraftDescription);
            Assert.AreEqual("Draft Body", postRepo.GetAll().First().DraftBody);
        }

        [TestMethod]
        public void UnPublishPublishPostIsPublishedDraftContent_PostModification()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Published, 
                    Title = "Test Title", 
                    DraftTitle = "Draft Title", 
                    Description = "Test Description", 
                    DraftDescription = "Draft Description", 
                    Body = "Test Body", 
                    DraftBody = "Draft Body", 
                    Path ="2013/04/9/some-other-post", 
                    Posted = new DateTime(2013,4,9), 
                    Author = new User{ Email = "" },
                    BlogId = 1 
                }}));

            PostAuthoringController sut = new PostAuthoringController(
                postRepo,
                _postModificationRepo,
                _userRepo,
                _redirectRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            var result = sut.ConfirmUnPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, _postModificationRepo.GetAll().Count());
            Assert.IsTrue(_postModificationRepo.GetAll().First().StatusModified);
            Assert.AreEqual(PostStatus.Unpublished, _postModificationRepo.GetAll().First().NewStatus);
        }

        [TestMethod]
        public void CannotUnPublishWhenNotInCurrentBlog()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Published, 
                    Title = "Test Title", 
                    DraftTitle = "Draft Title", 
                    Description = "Test Description", 
                    DraftDescription = "Draft Description", 
                    Body = "Test Body", 
                    DraftBody = "Draft Body", 
                    Path ="2013/04/9/some-other-post", 
                    Posted = new DateTime(2013,4,9), 
                    Author = new User{ Email = "" },
                    BlogId = 2 
                }}));

            PostAuthoringController sut = new PostAuthoringController(
                postRepo,
                _postModificationRepo,
                _userRepo,
                _redirectRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            try
            {
                sut.ConfirmUnPublish(1, new ConfirmPublishModel());
                Assert.Fail("Was expecting an exception when trying to edit");
            }
            catch { }
            
            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot

            Assert.AreEqual(0, _postModificationRepo.GetAll().Count());
            Assert.AreEqual(PostStatus.Published, postRepo.GetAll().First().Status);
        }
    }
}
