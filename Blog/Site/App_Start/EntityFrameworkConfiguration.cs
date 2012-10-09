[assembly: WebActivator.PreApplicationStartMethod(typeof(StaticVoid.Blog.Site.EntityFrameworkConfiguration), "Start")]

namespace StaticVoid.Blog.Site
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Web;
	using StaticVoid.Blog.Data.Migrations;
	using StaticVoid.Blog.Data;

	public class EntityFrameworkConfiguration
	{
		/// <summary>
		/// Starts the application
		/// </summary>
		public static void Start()
		{
			BlogContext.ConfigureInitializer();
		}

	}
}