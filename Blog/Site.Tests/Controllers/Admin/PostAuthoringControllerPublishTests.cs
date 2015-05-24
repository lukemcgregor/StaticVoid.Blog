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
    public class PostAuthoringControllerPublishTests
    {
        private IRepository<Data.Blog> _blogRepo;
        private IRepository<PostModification> _postModificationRepo;
        private Mock<ISecurityHelper> _mockSecurityHelper;
        private IRepository<Data.User> _userRepo;
        private IRepository<Data.Redirect> _redirectRepo;
        private IRepository<Data.Securable> _securableRepo;
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
			_mockSecurityHelper.Setup(s => s.CurrentUser).Returns(new User()
			{
				Email = "joe@bloggs.com"
			});

            _userRepo = new SimpleRepository<Data.User>(new InMemoryRepositoryDataSource<Data.User>(new List<Data.User> { new Data.User { 
                //ClaimedIdentifier = "zzz", 
                Email = "joe@bloggs.com"
            }}));
            _redirectRepo = new SimpleRepository<Data.Redirect>(new InMemoryRepositoryDataSource<Data.Redirect>(new List<Data.Redirect> { new Data.Redirect { } }));
            _mockHttpContext = new Mock<IHttpContextService>();
            _mockHttpContext.Setup(h => h.RequestUrl).Returns(new Uri("http://blog.test.con/blah"));
            _securableRepo = new SimpleRepository<Data.Securable>(new InMemoryRepositoryDataSource<Data.Securable>());

        }

        [TestMethod]
        public void PublishPostIsDraftContentNulledTest()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Draft, 
                    DraftTitle = "Test Title", 
                    DraftDescription = "Test Description", 
                    DraftBody = "Test Body", 
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
                _securableRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            var result = sut.ConfirmPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.IsNull(postRepo.GetAll().First().DraftTitle);
            Assert.IsNull(postRepo.GetAll().First().DraftDescription);
            Assert.IsNull(postRepo.GetAll().First().DraftBody);
        }

        [TestMethod]
        public void PublishPostIsContentSetTest()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Draft, 
                    DraftTitle = "Test Title", 
                    DraftDescription = "Test Description", 
                    DraftBody = "Test Body", 
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
                _securableRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            var result = sut.ConfirmPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.AreEqual("Test Title",postRepo.GetAll().First().Title);
            Assert.AreEqual("Test Description", postRepo.GetAll().First().Description);
            Assert.AreEqual("Test Body", postRepo.GetAll().First().Body);
        }

        [TestMethod]
        public void PublishPostIsStatusSetTest()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Draft, 
                    DraftTitle = "Test Title", 
                    DraftDescription = "Test Description", 
                    DraftBody = "Test Body", 
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
                _securableRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new DateTimeProvider(),
                _mockHttpContext.Object);

            var result = sut.ConfirmPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.AreEqual(PostStatus.Published, postRepo.GetAll().First().Status);
        }

        [TestMethod]
        public void PublishPostIsDateUpdateWhenDraftTest()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Draft, 
                    DraftTitle = "Test Title", 
                    DraftDescription = "Test Description", 
                    DraftBody = "Test Body", 
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
                _securableRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new MockDateTimeProvider(new DateTime(2013, 4, 27)),
                _mockHttpContext.Object);

            var result = sut.ConfirmPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.AreEqual(new DateTime(2013, 4, 27), postRepo.GetAll().First().Posted);
        }

        [TestMethod]
        public void PublishPostIsDateLeftAloneWhenPublishedAlreadyTest()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Published, 
                    DraftTitle = "Test Title", 
                    DraftDescription = "Test Description", 
                    DraftBody = "Test Body", 
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
                _securableRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new MockDateTimeProvider(new DateTime(2013, 4, 27)),
                _mockHttpContext.Object);

            var result = sut.ConfirmPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.AreEqual(new DateTime(2013, 4, 9), postRepo.GetAll().First().Posted);
        }

        [TestMethod]
        public void PublishPostIsDateLeftAloneWhenUnPublishedTest()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Unpublished, 
                    DraftTitle = "Test Title", 
                    DraftDescription = "Test Description", 
                    DraftBody = "Test Body", 
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
                _securableRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new MockDateTimeProvider(new DateTime(2013, 4, 27)),
                _mockHttpContext.Object);

            var result = sut.ConfirmPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.AreEqual(new DateTime(2013, 4, 9), postRepo.GetAll().First().Posted);
        }

        [TestMethod]
        public void PublishPostIsPostModificationRecordCreatedDraftTest()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Draft, 
                    DraftTitle = "Test Title", 
                    DraftDescription = "Test Description", 
                    DraftBody = "Test Body", 
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
                _securableRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new MockDateTimeProvider(new DateTime(2013, 4, 27, 1, 2, 3)),
                _mockHttpContext.Object);

            var result = sut.ConfirmPublish(1, new ConfirmPublishModel()) as JsonResult;

            Assert.IsNotNull(result);

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot
            Assert.IsTrue(((dynamic)result.Data).success);

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.AreEqual(1, _postModificationRepo.GetAll().Count());
            Assert.AreEqual(new DateTime(2013, 4, 27, 1, 2, 3), _postModificationRepo.GetAll().First().Timestamp);
            Assert.IsFalse(_postModificationRepo.GetAll().First().TitleModified);
            Assert.IsFalse(_postModificationRepo.GetAll().First().DescriptionModified);
            Assert.IsFalse(_postModificationRepo.GetAll().First().BodyModified);
            Assert.IsTrue(_postModificationRepo.GetAll().First().StatusModified);
            Assert.AreEqual(PostStatus.Published, _postModificationRepo.GetAll().First().NewStatus);
            Assert.IsFalse(_postModificationRepo.GetAll().First().CannonicalModified);
        }

        [TestMethod]
        public void CantPublishPostWhenItIsntInTheCurrentBlog()
        {
            var postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Id=1, 
                    Status = PostStatus.Draft, 
                    DraftTitle = "Test Title", 
                    DraftDescription = "Test Description", 
                    DraftBody = "Test Body", 
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
                _securableRepo,
                _blogRepo,
                _mockSecurityHelper.Object,
                new MockDateTimeProvider(new DateTime(2013, 4, 27, 1, 2, 3)),
                _mockHttpContext.Object);

            try
            {
                sut.ConfirmPublish(1, new ConfirmPublishModel());
                Assert.Fail("Was expecting an exception when trying to edit");
            }
            catch { }

            // This requires [assembly: InternalsVisibleTo("StaticVoid.Blog.Site.Tests")] in StaticVoid.Blog.Site so that we can read anon types, needing this is kinda lame
            // dynamic should just jams it in there by itself. I mean heck the debugger can see it, why cant dynamic? 
            // cf http://stackoverflow.com/questions/2630370/c-sharp-dynamic-cannot-access-properties-from-anonymous-types-declared-in-anot

            Assert.AreEqual(1, postRepo.GetAll().Count());
            Assert.AreEqual(0, _postModificationRepo.GetAll().Count());
            Assert.AreEqual(PostStatus.Draft, postRepo.GetAll().First().Status);
        }
    }
}
