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

			bundles.Add(new ScriptBundle("~/bundles/openid").Include(
						"~/Scripts/openid-jquery.js",
						"~/Scripts/openid-en.js"));

            bundles.Add(new ScriptBundle("~/bundles/blog-admin").Include(
                        "~/Scripts/jquery-1.8.2.js",
                        "~/Scripts/modernizr-*",
                        "~/Scripts/Prettify/prettify.js",
                        "~/Scripts/Pure/pure.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/modals.js",
                        "~/Scripts/knockout-2.2.1.js",
                        "~/Scripts/knockout.mapping-latest.js"));

			bundles.Add(new ScriptBundle("~/bundles/editor").Include(
						"~/Scripts/MarkdownDeep.js",
						"~/Scripts/MarkdownDeepEditor.js",
						"~/Scripts/MarkdownDeepEditorUI.js",
						"~/Scripts/jquery.ba-resize.js",
                        "~/Scripts/editor.js", 
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/blog").Include(
                        "~/Scripts/jquery-1.8.2.js",
                        "~/Scripts/modernizr-*",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/modals.js",
                        "~/Scripts/Prettify/prettify.js",
                        "~/Scripts/jquery.scrollTo.js"));

			bundles.Add(new StyleBundle("~/Content/css/openid").Include(
						"~/Content/openid-shadow.css",
						"~/Content/openid.css"));

			var post = new StyleBundle("~/Content/post_less").Include(
						"~/Content/post.less",
						"~/Content/trendy-date.less",
						"~/Content/Prettify/prettify.css");
			post.Transforms.Clear();
			post.Transforms.Add(new LessTransform());
			post.Transforms.Add(new CssMinify());
			bundles.Add(post);

            var error = new StyleBundle("~/Content/error").Include(
                        "~/Content/error.less");
            error.Transforms.Clear();
            error.Transforms.Add(new LessTransform());
            error.Transforms.Add(new CssMinify());
            bundles.Add(error);

            var dashboard = new StyleBundle("~/Content/dashboard").Include(
                        "~/Content/dashboard.less");
            dashboard.Transforms.Clear();
            dashboard.Transforms.Add(new LessTransform());
            dashboard.Transforms.Add(new CssMinify());
            bundles.Add(dashboard);

			bundles.Add(new StyleBundle("~/Content/post").Include(
						"~/Content/bootstrap.css",
						"~/Content/bootstrap-responsive.css",
						"~/Content/Prettify/prettify.css",
						"~/Content/blog.css"));

            bundles.Add(new StyleBundle("~/Content/blog-admin").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/bootstrap-responsive.css",
                        "~/Content/mdd_styles.css",
                        "~/Content/blog.css"));
		}
	}
}