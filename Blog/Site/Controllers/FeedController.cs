﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Repository;

namespace StaticVoid.Blog.Site.Controllers
{
    public class FeedController : Controller
    {
		private IRepository<Post> _postRepository;
        private string _siteUrl;
        private Guid _blogGuid;

        public FeedController(IRepository<Post> postRepository, IRepository<Data.Blog> blogRepository)
		{
			_postRepository = postRepository;
            _siteUrl = blogRepository.GetCurrentBlog().AuthoritiveUrl;
            _blogGuid = blogRepository.GetCurrentBlog().BlogGuid;
		}

		private SyndicationFeed GenerateFeed()        {

            var md = new MarkdownDeep.Markdown();
            List<SyndicationItem> posts = new List<SyndicationItem>();
            foreach (var post in _postRepository.FeedPosts().OrderByDescending(p => p.Posted).Take(25).AsEnumerable())
            {
                var item = new SyndicationItem(post.Title, post.Body, new Uri(_siteUrl.TrimEnd('/') +"/"+ post.Path.TrimStart('/')));

                item.Title = new TextSyndicationContent(post.Title);
                item.Content = new TextSyndicationContent(md.Transform(post.Body), TextSyndicationContentKind.Html);
                item.PublishDate = new DateTimeOffset(post.Posted);
                item.LastUpdatedTime = new DateTimeOffset(post.Posted);
                item.Id = post.PostGuid.ToString();
                                
                posts.Add(item);
            }


            return new SyndicationFeed("StaticVoid", "A blog on .Net", new Uri(_siteUrl), posts)
			{
				Language = "en-US",
                LastUpdatedTime = posts.Any() ? posts.Max(p=>p.LastUpdatedTime) : new DateTime(2012,12,21),
                Id= _blogGuid.ToString()
			};
		}

		public ActionResult Atom()
		{
			return new FeedResult(new Atom10FeedFormatter(GenerateFeed()), "application/atom+xml");
		}
    }
}
