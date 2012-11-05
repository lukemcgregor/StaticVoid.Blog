using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Core.Repository;

namespace StaticVoid.Blog.Site.Controllers
{
    public class FeedController : Controller
    {
		private IRepository<Post> _postRepository;
        private string _siteUrl;

		public FeedController(IRepository<Post> postRepository, IAuthoritiveUrl authoritiveUrl)
		{
			_postRepository = postRepository;
            _siteUrl = authoritiveUrl.Url;
		}

		private SyndicationFeed GenerateFeed()
        {
            var md = new MarkdownDeep.Markdown();
            List<SyndicationItem> posts = new List<SyndicationItem>();
            foreach (var post in _postRepository.PublishedPosts().OrderByDescending(p => p.Posted).Take(25).AsEnumerable())
            {
                var item = new SyndicationItem(post.Title, post.Body, new Uri(_siteUrl + post.Path));

                item.Title = new TextSyndicationContent(post.Title);
                item.Content = new TextSyndicationContent(md.Transform(post.Body), TextSyndicationContentKind.Html);
                item.PublishDate = new DateTimeOffset(post.Posted);
                item.LastUpdatedTime = new DateTimeOffset(post.Posted);
                                
                posts.Add(item);
            }

            return new SyndicationFeed("StaticVoid", "A blog on .Net", new Uri(_siteUrl), posts)
			{
				Language = "en-US",
                LastUpdatedTime = posts.Max(p=>p.LastUpdatedTime)
			};
		}

		public ActionResult Atom()
		{
			return new FeedResult(new Atom10FeedFormatter(GenerateFeed()));
		}
    }
}
