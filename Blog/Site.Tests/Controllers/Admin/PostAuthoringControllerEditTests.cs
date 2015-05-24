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
	public class PostAuthoringControllerEditTests
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
			_postModificationRepo = new SimpleRepository<Data.PostModification>(
				new InMemoryRepositoryDataSource<Data.PostModification>(new List<Data.PostModification> { 
                    new Data.PostModification { } 
                }));
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
		public void EditPostContentTest()
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
				_securableRepo,
				_blogRepo,
				_mockSecurityHelper.Object,
				new DateTimeProvider(),
				_mockHttpContext.Object);

			var result = sut.Edit(1, new Areas.Manage.Models.PostEditModel
			{
				Title = "New Title",
				Body = "New Body",
				Description = "New Description",
				Reposted = true,
				CanonicalUrl = "http://blog.con/new-post"
			}) as RedirectToRouteResult;

			Assert.IsNotNull(result);

			Assert.AreEqual("Index", result.RouteValues["action"]);
			Assert.AreEqual("Dashboard", result.RouteValues["controller"]);

			Assert.AreEqual(1, postRepo.GetAll().Count());
			Assert.IsTrue(postRepo.GetAll().Last().HasDraftContent());
			Assert.AreEqual("Test Title", postRepo.GetAll().Last().Title);
			Assert.AreEqual("Test Body", postRepo.GetAll().Last().Body);
			Assert.AreEqual("Test Description", postRepo.GetAll().Last().Description);
			Assert.AreEqual("New Title", postRepo.GetAll().Last().DraftTitle);
			Assert.AreEqual("New Body", postRepo.GetAll().Last().DraftBody);
			Assert.AreEqual("New Description", postRepo.GetAll().Last().DraftDescription);
			Assert.AreEqual("http://blog.con/new-post", postRepo.GetAll().Last().Canonical);//TODO Change this so that its also draft
		}

		[TestMethod]
		public void EditPostReturnCanonicalToDefault()
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
                    Canonical = "http://blog.con/new-post",
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

			var result = sut.Edit(1, new Areas.Manage.Models.PostEditModel
			{
				Title = "New Title",
				Body = "New Body",
				Description = "New Description",
				Reposted = false,
			}) as RedirectToRouteResult;

			Assert.IsNotNull(result);

			Assert.AreEqual("Index", result.RouteValues["action"]);
			Assert.AreEqual("Dashboard", result.RouteValues["controller"]);

			Assert.AreEqual(1, postRepo.GetAll().Count());
			Assert.AreEqual("/2013/04/9/some-other-post", postRepo.GetAll().Last().Canonical);
		}

		[TestMethod]
		public void EditPostNoChangeDoesNotTriggerDraftStatus()
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
				_securableRepo,
				_blogRepo,
				_mockSecurityHelper.Object,
				new DateTimeProvider(),
				_mockHttpContext.Object);

			var result = sut.Edit(1, new Areas.Manage.Models.PostEditModel
			{
				Title = "Test Title",
				Body = "Test Body",
				Description = "Test Description",
				Reposted = false,
			}) as RedirectToRouteResult;

			Assert.IsNotNull(result);

			Assert.AreEqual("Index", result.RouteValues["action"]);
			Assert.AreEqual("Dashboard", result.RouteValues["controller"]);

			Assert.AreEqual(1, postRepo.GetAll().Count());
			Assert.IsFalse(postRepo.GetAll().Last().HasDraftContent());
		}

		[TestMethod]
		public void CannotEditPostWhenNotInCurrentBlog()
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
				new DateTimeProvider(),
				_mockHttpContext.Object);

			try
			{
				sut.Edit(1, new Areas.Manage.Models.PostEditModel
				{
					Title = "New Title",
					Body = "New Body",
					Description = "New Description",
					Reposted = true,
					CanonicalUrl = "http://blog.con/new-post"
				});
				Assert.Fail("Was expecting an exception when trying to edit");
			}
			catch { }

			Assert.AreEqual(1, postRepo.GetAll().Count());
			Assert.IsFalse(postRepo.GetAll().Last().HasDraftContent());
			Assert.AreEqual("Test Title", postRepo.GetAll().Last().Title);
			Assert.AreEqual("Test Body", postRepo.GetAll().Last().Body);
			Assert.AreEqual("Test Description", postRepo.GetAll().Last().Description);
			Assert.IsNull(postRepo.GetAll().Last().DraftTitle);
			Assert.IsNull(postRepo.GetAll().Last().DraftBody);
			Assert.IsNull(postRepo.GetAll().Last().DraftDescription);
			Assert.IsNull(postRepo.GetAll().Last().Canonical);
		}
	}
}
