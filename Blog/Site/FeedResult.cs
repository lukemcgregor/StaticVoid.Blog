using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace StaticVoid.Blog.Site
{
	public class FeedResult : ActionResult
	{
		public Encoding ContentEncoding { get; set; }
		public string ContentType { get; set; }

		private readonly SyndicationFeedFormatter _feed;
		public SyndicationFeedFormatter Feed
		{
			get { return _feed; }
		}

		public FeedResult(SyndicationFeedFormatter feed)
		{
			this._feed = feed;
		}

        public FeedResult(SyndicationFeedFormatter feed, string contentType) : this(feed)
        {
            ContentType = contentType;
        }

		public override void ExecuteResult(ControllerContext context)
		{
            if (ContentType != null)
            {
                context.HttpContext.Response.ContentType = ContentType;
            }
            else
            {
                context.HttpContext.Response.ContentType = "application/rss+xml";
            }

            if (_feed != null)
            {
                using (var xmlWriter = new XmlTextWriter(context.HttpContext.Response.Output))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    _feed.WriteTo(xmlWriter);
                }
            }
		}
	}
}