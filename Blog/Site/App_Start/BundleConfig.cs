using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace StaticVoid.Blog.Site
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-1.8.2.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
						"~/Scripts/jquery-ui-1.8.24.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.unobtrusive-ajax.js",
						"~/Scripts/jquery.validate.js",
						"~/Scripts/jquery.validate.unobtrusive.js"));

			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/prettify").Include(
						"~/Scripts/Prettify/prettify.js"));

			bundles.Add(new ScriptBundle("~/bundles/openid").Include(
						"~/Scripts/openid-jquery.js",
						"~/Scripts/openid-en.js"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
						"~/Scripts/bootstrap.js"));

			bundles.Add(new ScriptBundle("~/bundles/scrollto").Include(
						"~/Scripts/jquery.scrollTo.js"));

			bundles.Add(new ScriptBundle("~/bundles/editor").Include(
						"~/Scripts/MarkdownDeep.js",
						"~/Scripts/MarkdownDeepEditor.js",
						"~/Scripts/MarkdownDeepEditorUI.js",
						"~/Scripts/jquery.ba-resize.js",
						"~/Scripts/editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/modals").Include(
                        "~/Scripts/modals.js"));


            bundles.Add(new ScriptBundle("~/bundles/postscripts").Include(
                        "~/Scripts/jquery-1.8.2.js",
                        "~/Scripts/modernizr-*",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/Prettify/prettify.js",
                        "~/Scripts/jquery.scrollTo.js"));


			bundles.Add(new StyleBundle("~/Content/css/openid").Include(
						"~/Content/openid-shadow.css",
						"~/Content/openid.css"));

			var post = new StyleBundle("~/Content/post").Include(
						"~/Content/post.less",
						"~/Content/trendy-date.less",
						"~/Content/Prettify/prettify.css");
			post.Transforms.Clear();
			post.Transforms.Add(new LessTransform());
			post.Transforms.Add(new CssMinify());
			bundles.Add(post);

			bundles.Add(new StyleBundle("~/Content/style").Include(
						"~/Content/bootstrap.css",
						"~/Content/bootstrap-responsive.css",
						"~/Content/mdd_styles.css",
						"~/Content/Prettify/prettify.css",
						"~/Content/blog.css"));
		}
	}
}