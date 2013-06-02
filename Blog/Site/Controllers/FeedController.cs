using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Repository;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Controllers
{
    public class FeedController : BlogBaseController
    {
		private IRepository<Post> _postRepository;

        public FeedController(IRepository<Post> postRepository, IRepository<Data.Blog> blogRepository, IHttpContextService httpContext) : base(blogRepository, httpContext)
		{
			_postRepository = postRepository;
		}

		private SyndicationFeed GenerateFeed()        
        {
            var currentBlog = this.CurrentBlog;

            var md = new MarkdownDeep.Markdown();
            List<SyndicationItem> posts = new List<SyndicationItem>();
            foreach (var post in _postRepository.FeedPosts(currentBlog.Id).OrderByDescending(p => p.Posted).Take(25).AsEnumerable())
            {
                var item = new SyndicationItem(post.Title, post.Body, new Uri(currentBlog.AuthoritiveUrl.TrimEnd('/') + "/" + post.Path.TrimStart('/')));

                item.Title = new TextSyndicationContent(post.Title);
                item.Content = new TextSyndicationContent(md.Transform(post.Body), TextSyndicationContentKind.Html);
                item.PublishDate = new DateTimeOffset(post.Posted);
                item.LastUpdatedTime = new DateTimeOffset(post.Posted);
                item.Id = post.PostGuid.ToString();
                                
                posts.Add(item);
            }


            return new SyndicationFeed("StaticVoid", "A blog on .Net", new Uri(currentBlog.AuthoritiveUrl), posts)
			{
				Language = "en-US",
                LastUpdatedTime = posts.Any() ? posts.Max(p=>p.LastUpdatedTime) : new DateTime(2012,12,21),
                Id = currentBlog.BlogGuid.ToString()
			};
		}

		public ActionResult Atom()
		{
			return new FeedResult(new Atom10FeedFormatter(GenerateFeed()), "application/atom+xml");
		}
    }
}
