﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Models;
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
    public class PostControllerPreviewTests
    {
        private IRepository<Data.Blog> _blogRepo;
        private IRepository<BlogTemplate> _templateRepo;
        private Mock<IHttpContextService> _mockHttpContext;

        [TestInitialize]
        public void Initialise()
        {
            _blogRepo = new SimpleRepository<Data.Blog>(new InMemoryRepositoryDataSource<Data.Blog>(new List<Data.Blog> { new Data.Blog { Id = 1, AuthoritiveUrl = "http://blog.test.con" } }));
            _templateRepo = new SimpleRepository<BlogTemplate>(new InMemoryRepositoryDataSource<BlogTemplate>());
            _mockHttpContext = new Mock<IHttpContextService>();
            _mockHttpContext.Setup(h => h.RequestUrl).Returns(new Uri("http://blog.test.con/blah"));
        }

        [TestMethod]
        public void PreviewForUnpublishedPostTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { Status = PostStatus.Published, Path ="2013/04/10/some-other-post", Posted = new DateTime(2013,4,10), Author = new User{ Email = "" }, BlogId = 1 },
                new Post { 
                    Id = 1,
                    Status = PostStatus.Unpublished, 
                    Path ="2013/04/14/some-post", 
                    Posted = new DateTime(2013,4,14), 
                    Author = new User{ Email = "", FirstName = "Joe", LastName = "Bloggs" },
                    DraftBody = "asdf",
                    DraftTitle = "qwerty", 
                    BlogId = 1
                }
            }));

            PostController sut = new PostController(postRepo, _blogRepo,_templateRepo, _mockHttpContext.Object);
            var result = (ViewResult)sut.Preview(1);

            Assert.IsNotNull(result);
            var model = result.Model as PostModel;
            Assert.IsNotNull(model);

            var md = new MarkdownDeep.Markdown();
            Assert.AreEqual("Joe Bloggs", model.Author.Name);
            Assert.AreEqual(md.Transform("asdf"), model.Body);
            Assert.AreEqual("qwerty", model.Title);
        }

        [TestMethod]
        public void PreviewForDraftPostTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { Status = PostStatus.Published, Path ="2013/04/10/some-other-post", Posted = new DateTime(2013,4,10), Author = new User{ Email = "" }, BlogId = 1 },
                new Post { 
                    Id = 1,
                    Status = PostStatus.Draft, 
                    Path ="2013/04/14/some-post", 
                    Posted = new DateTime(2013,4,14), 
                    Author = new User{ Email = "", FirstName = "Joe", LastName = "Bloggs" },
                    DraftBody = "asdf",
                    DraftTitle = "qwerty", 
                    BlogId = 1
                }
            }));

            PostController sut = new PostController(postRepo, _blogRepo, _templateRepo, _mockHttpContext.Object);
            var result = (ViewResult)sut.Preview(1);

            Assert.IsNotNull(result);
            var model = result.Model as PostModel;
            Assert.IsNotNull(model);

            var md = new MarkdownDeep.Markdown();
            Assert.AreEqual("Joe Bloggs", model.Author.Name);
            Assert.AreEqual(md.Transform("asdf"), model.Body);
            Assert.AreEqual("qwerty", model.Title);
        }

        [TestMethod]
        public void CantPreviewPostFromAnotherBlog()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { Status = PostStatus.Published, Path ="2013/04/10/some-other-post", Posted = new DateTime(2013,4,10), Author = new User{ Email = "" }, BlogId = 1 },
                new Post { 
                    Id = 1,
                    Status = PostStatus.Draft, 
                    Path ="2013/04/14/some-post", 
                    Posted = new DateTime(2013,4,14), 
                    Author = new User{ Email = "", FirstName = "Joe", LastName = "Bloggs" },
                    DraftBody = "asdf",
                    DraftTitle = "qwerty", 
                    BlogId = 2
                }
            }));

            PostController sut = new PostController(postRepo, _blogRepo, _templateRepo, _mockHttpContext.Object);
            var result = sut.Preview(1);

            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.AreEqual((int)HttpStatusCode.NotFound,((HttpStatusCodeResult)result).StatusCode);
        }
    }
}
