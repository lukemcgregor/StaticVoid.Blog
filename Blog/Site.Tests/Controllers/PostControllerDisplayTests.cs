using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Site.Tests.Controllers
{
    [TestClass]
    public class PostControllerDisplayTests
    {
        [TestMethod]
        public void DisplayPostTest()
        {
            IRepository<Post> postRepo = new SimpleRepository<Post>(new InMemoryRepositoryDataSource<Post>(new List<Post> { 
                new Post { Status = PostStatus.Published, Path ="2013/04/10/some-other-post", Posted = new DateTime(2013,4,10) },
                new Post { Status = PostStatus.Published, Path ="2013/04/14/some-post", Posted = new DateTime(2013,4,14) }
            }));
            IRepository<Data.Blog> blogRepo = new SimpleRepository<Data.Blog>(new InMemoryRepositoryDataSource<Data.Blog>(new List<Data.Blog> { new Data.Blog { } }));

            Mock<VisitLoggerService> mockVisitLoggerService = new Mock<VisitLoggerService>();

            PostController sut = new PostController(postRepo, mockVisitLoggerService.Object, blogRepo);
            var result = sut.Display("2013/04/14/some-post");
        }
    }
}
