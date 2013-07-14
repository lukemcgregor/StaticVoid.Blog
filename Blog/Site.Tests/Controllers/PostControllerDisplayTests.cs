using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Models;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using StaticVoid.Blog.Site.Gravitar;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Tests.Controllers
{
    [TestClass]
    public class PostControllerDisplayTests
    {
        private IRepository<Data.Blog> _blogRepo;
        private IRepository<BlogTemplate> _templateRepo;
        private Mock<IHttpContextService> _mockHttpContext;

        [TestInitialize]
        public void Initialise()
        {
            _blogRepo = new SimpleRepository<Data.Blog>(new InMemoryRepositoryDataSource<Data.Blog>(new List<Data.Blog> { new Data.Blog { Id=1, AuthoritiveUrl = "http://blog.test.con" } }));
            _templateRepo = new SimpleRepository<BlogTemplate>(new InMemoryRepositoryDataSource<BlogTemplate>());
            _mockHttpContext = new Mock<IHttpContextService>();
            _mockHttpContext.Setup(h => h.RequestUrl).Returns(new Uri("http://blog.test.con/blah"));
        }

        [TestMethod]
        public void DisplayPostTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Status = PostStatus.Published, 
                    Path ="2013/04/10/some-other-post", 
                    Posted = new DateTime(2013,4,10), 
                    Author = new User{ Email = "" },
                    BlogId = 1
                },
                new Post { 
                    Status = PostStatus.Published, 
                    Path ="2013/04/14/some-post", 
                    Posted = new DateTime(2013,4,14), 
                    Author = new User{ Email = "" },
                    BlogId = 1
                }
            }));

            PostController sut = new PostController(postRepo, _blogRepo, _templateRepo, _mockHttpContext.Object);
            var result = (ViewResult)sut.Display("2013/04/14/some-post");

            Assert.IsNotNull(result);
            var model = result.Model as PostModel;
            Assert.IsNotNull(model);

            Assert.AreEqual(new DateTime(2013, 4, 14), model.Posted);
        }

        [TestMethod]
        public void DisplayPostAuthorDetailsTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Status = PostStatus.Published, 
                    Path ="2013/04/10/some-other-post", 
                    Posted = new DateTime(2013,4,10), 
                    Author = new User{ Email = "" },
                    BlogId = 1 
                },
                new Post { 
                    Status = PostStatus.Published, 
                    Path ="2013/04/14/some-post", 
                    Posted = new DateTime(2013,4,14), 
                    Author = new User{ 
                        Id = 1, 
                        GooglePlusProfileUrl = "https://plus.google.com/u/0/1234567890", 
                        Email = "joe@bloggs.con",
                        FirstName = "Joe",
                        LastName = "Bloggs"
                    },
                    BlogId = 1
                }
            }));

            PostController sut = new PostController(postRepo, _blogRepo, _templateRepo, _mockHttpContext.Object);
            var result = (ViewResult)sut.Display("2013/04/14/some-post");

            Assert.IsNotNull(result);
            var model = result.Model as PostModel;
            Assert.IsNotNull(model);

            Assert.AreEqual("Joe Bloggs", model.Author.Name);
            Assert.AreEqual("joe@bloggs.con".GravitarUrlFromEmail(), model.Author.GravatarUrl);
            Assert.AreEqual("https://plus.google.com/u/0/1234567890", model.Author.GooglePlusProfileUrl);
        }

        [TestMethod]
        public void DisplayPostContentTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Status = PostStatus.Published, 
                    Path ="2013/04/10/some-other-post", 
                    Posted = new DateTime(2013,4,10), 
                    Author = new User{ Email = "" },
                    BlogId = 1
                },
                new Post { 
                    Title = "Test Title",
                    DraftTitle = "Draft Title",
                    Body = "Test Body",
                    DraftBody = "Draft Title",
                    Description = "Test Description",
                    DraftDescription = "Draft Description",
                    Status = PostStatus.Published, 
                    Path ="2013/04/14/some-post", 
                    Posted = new DateTime(2013,4,14), 
                    Author = new User{ Email = "joe@bloggs.con" },
                    BlogId = 1
                }
            }));

            PostController sut = new PostController(postRepo, _blogRepo, _templateRepo, _mockHttpContext.Object);
            var result = (ViewResult)sut.Display("2013/04/14/some-post");

            Assert.IsNotNull(result);
            var model = result.Model as PostModel;
            Assert.IsNotNull(model);
            var md = new MarkdownDeep.Markdown();
            Assert.AreEqual("Test Title", model.Title);
            Assert.AreEqual(md.Transform("Test Body"), model.Body);
            Assert.AreEqual("Test Description", model.Description);
        }

        [TestMethod]
        public void DisplayPostNavigationTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { 
                    Status = PostStatus.Published, 
                    Title = "some-other-post", 
                    Path ="2013/04/9/some-other-post", 
                    Posted = new DateTime(2013,4,9), 
                    Author = new User{ Email = "" },
                    BlogId = 1
                },
                new Post { 
                    Status = PostStatus.Published, 
                    Title = "some-other-post2", 
                    Path ="2013/04/10/some-other-post2", 
                    Posted = new DateTime(2013,4,10), 
                    Author = new User{ Email = "" },
                    BlogId = 1 
                },
                new Post { 
                    Canonical = "http://blog.con/2013/04/14/canonical",
                    Status = PostStatus.Published, 
                    Title = "some-post",
                    Path ="2013/04/14/some-post", 
                    Posted = new DateTime(2013,4,14), 
                    Author = new User{ Email = "joe@bloggs.con" } ,
                    BlogId = 1
                },
                new Post { 
                    Status = PostStatus.Published, 
                    Title = "some-other-post3", 
                    Path ="2013/04/15/some-other-post3", 
                    Posted = new DateTime(2013,4,15), 
                    Author = new User{ Email = "" },
                    BlogId = 1
                },
            }));

            PostController sut = new PostController(postRepo, _blogRepo, _templateRepo, _mockHttpContext.Object);
            var result = (ViewResult)sut.Display("2013/04/14/some-post");

            Assert.IsNotNull(result);
            var model = result.Model as PostModel;
            Assert.IsNotNull(model);

            Assert.AreEqual("http://blog.con/2013/04/14/canonical", model.CanonicalUrl);

            Assert.AreEqual(new DateTime(2013, 4, 15), model.NextPost.Date);
            Assert.AreEqual("2013/04/15/some-other-post3", model.NextPost.Link);
            Assert.AreEqual("some-other-post3", model.NextPost.Title);

            Assert.AreEqual(new DateTime(2013, 4, 10), model.PreviousPost.Date);
            Assert.AreEqual("2013/04/10/some-other-post2", model.PreviousPost.Link);
            Assert.AreEqual("some-other-post2", model.PreviousPost.Title);

            Assert.AreEqual(4, model.OtherPosts.Count());

            Assert.AreEqual("2013/04/15/some-other-post3", model.OtherPosts[0].Link);
            Assert.AreEqual("2013/04/14/some-post", model.OtherPosts[1].Link);
            Assert.AreEqual("2013/04/10/some-other-post2", model.OtherPosts[2].Link);
            Assert.AreEqual("2013/04/9/some-other-post", model.OtherPosts[3].Link);

            Assert.IsFalse(model.OtherPosts[0].IsCurrentPost);
            Assert.IsTrue(model.OtherPosts[1].IsCurrentPost);
            Assert.IsFalse(model.OtherPosts[2].IsCurrentPost);
            Assert.IsFalse(model.OtherPosts[3].IsCurrentPost);

            Assert.AreEqual("some-other-post3", model.OtherPosts[0].Title);
            Assert.AreEqual("some-post", model.OtherPosts[1].Title);
            Assert.AreEqual("some-other-post2", model.OtherPosts[2].Title);
            Assert.AreEqual("some-other-post", model.OtherPosts[3].Title);
        }
    }
}
