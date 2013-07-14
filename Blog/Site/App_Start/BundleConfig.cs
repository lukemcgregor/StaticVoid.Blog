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
			bundles.Add(new ScriptBundle("~/bundles/openid").Include(
						"~/Scripts/openid-jquery.js",
						"~/Scripts/openid-en.js"));

            bundles.Add(new ScriptBundle("~/bundles/blog-admin").Include(
                        "~/Scripts/modals.js",
                        "~/Scripts/validation-rules.js",
                        "~/Scripts/guid.js",
                        "~/Scripts/jquery.fileupload.js",
                        "~/Scripts/admin-layout.js"));

			bundles.Add(new ScriptBundle("~/bundles/editor").Include(
						"~/Scripts/MarkdownDeep.js",
						"~/Scripts/MarkdownDeepEditor.js",
						"~/Scripts/MarkdownDeepEditorUI.js",
                        "~/Scripts/editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/blog").Include(
                        "~/Scripts/modals.js"));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                        "~/Scripts/dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/style-editor").Include(
                        "~/Scripts/style-editor.js"));

			bundles.Add(new StyleBundle("~/Content/css/openid").Include(
						"~/Content/openid-shadow.css",
						"~/Content/openid.css"));

			var post = new StyleBundle("~/Content/post_less").Include(
						"~/Content/blog.css",
						"~/Content/post.less",
						"~/Content/trendy-date.less");
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

            var admin = new StyleBundle("~/Content/blog-admin").Include(
                        "~/Content/mdd_styles.css",
                        "~/Content/blog.css",
                        "~/Content/blog-admin.less");

            admin.Transforms.Clear();
            admin.Transforms.Add(new LessTransform());
            admin.Transforms.Add(new CssMinify());
            bundles.Add(admin);
		}
	}
}