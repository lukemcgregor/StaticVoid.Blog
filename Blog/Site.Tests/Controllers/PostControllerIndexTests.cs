using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Tests.Controllers
{
    [TestClass]
    public class PostControllerIndexTests
    {
        private IRepository<Data.Blog> _blogRepo;
        private IRepository<BlogTemplate> _templateRepo;
        private Mock<IHttpContextService> _mockHttpContext;

        [TestInitialize]
        public void Initialise()
        {
            _blogRepo = new SimpleRepository<Data.Blog>(new InMemoryRepositoryDataSource<Data.Blog>(new List<Data.Blog> { new Data.Blog { Id = 1, AuthoritiveUrl = "http://blog.test.con" } }));
            _templateRepo =  new SimpleRepository<BlogTemplate>(new InMemoryRepositoryDataSource<BlogTemplate>());
            _mockHttpContext = new Mock<IHttpContextService>();
            _mockHttpContext.Setup(h => h.RequestUrl).Returns(new Uri("http://blog.test.con/blah"));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpException))]
        public void RedirectToLatestPostWithNoPostsTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>());

            PostController sut = new PostController(postRepo, _blogRepo, _templateRepo, _mockHttpContext.Object);
            try
            {
                sut.Index();
            }
            catch(HttpException ex)
            {
                Assert.AreEqual((int)HttpStatusCode.NotFound, ex.GetHttpCode());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(HttpException))]
        public void RedirectToLatestPostWithNoPublishedPostsTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { Status = PostStatus.Draft }, 
                new Post { Status = PostStatus.Unpublished } 
            }));

            PostController sut = new PostController(postRepo, _blogRepo,_templateRepo, _mockHttpContext.Object);
            try
            {
                sut.Index();
            }
            catch (HttpException ex)
            {
                Assert.AreEqual((int)HttpStatusCode.NotFound, ex.GetHttpCode());
                throw;
            }
        }

        [TestMethod]
        public void RedirectToLatestPostWithOnePublishedPostsTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { Status = PostStatus.Published, Path ="2013/04/14/some-post", Posted = new DateTime(2013,4,14), BlogId = 1 }
            }));

            PostController sut = new PostController(postRepo, _blogRepo,_templateRepo, _mockHttpContext.Object);
            var result = sut.Index();

            Assert.AreEqual("/2013/04/14/some-post", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void RedirectToLatestPostWithMultiplePublishedPostsTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { Status = PostStatus.Published, Path ="2013/04/10/some-other-post", Posted = new DateTime(2013,4,10), BlogId = 1 },
                new Post { Status = PostStatus.Published, Path ="2013/04/14/some-post", Posted = new DateTime(2013,4,14), BlogId = 1 }
            }));

            PostController sut = new PostController(postRepo, _blogRepo,_templateRepo, _mockHttpContext.Object);
            var result = sut.Index();

            Assert.AreEqual("/2013/04/14/some-post", ((RedirectResult)result).Url);
        }
    }
}
