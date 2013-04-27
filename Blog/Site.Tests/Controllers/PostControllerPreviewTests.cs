using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Models;
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
        [TestMethod]
        public void PreviewForUnpublishedPostTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { Status = PostStatus.Published, Path ="2013/04/10/some-other-post", Posted = new DateTime(2013,4,10), Author = new User{ Email = "" } },
                new Post { 
                    Id = 1,
                    Status = PostStatus.Unpublished, 
                    Path ="2013/04/14/some-post", 
                    Posted = new DateTime(2013,4,14), 
                    Author = new User{ Email = "", FirstName = "Joe", LastName = "Bloggs" },
                    DraftBody = "asdf",
                    DraftTitle = "qwerty"
                }
            }));
            IRepository<Data.Blog> blogRepo = new SimpleRepository<Data.Blog>(new InMemoryRepositoryDataSource<Data.Blog>(new List<Data.Blog> { new Data.Blog { } }));


            PostController sut = new PostController(postRepo, null, blogRepo);
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
                new Post { Status = PostStatus.Published, Path ="2013/04/10/some-other-post", Posted = new DateTime(2013,4,10), Author = new User{ Email = "" } },
                new Post { 
                    Id = 1,
                    Status = PostStatus.Draft, 
                    Path ="2013/04/14/some-post", 
                    Posted = new DateTime(2013,4,14), 
                    Author = new User{ Email = "", FirstName = "Joe", LastName = "Bloggs" },
                    DraftBody = "asdf",
                    DraftTitle = "qwerty"
                }
            }));
            IRepository<Data.Blog> blogRepo = new SimpleRepository<Data.Blog>(new InMemoryRepositoryDataSource<Data.Blog>(new List<Data.Blog> { new Data.Blog { } }));


            PostController sut = new PostController(postRepo, null, blogRepo);
            var result = (ViewResult)sut.Preview(1);

            Assert.IsNotNull(result);
            var model = result.Model as PostModel;
            Assert.IsNotNull(model);

            var md = new MarkdownDeep.Markdown();
            Assert.AreEqual("Joe Bloggs", model.Author.Name);
            Assert.AreEqual(md.Transform("asdf"), model.Body);
            Assert.AreEqual("qwerty", model.Title);
        }
    }
}
