using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site
{
    public interface IAuthoritiveUrl 
    {
        string Url { get; }
    }

    public class AuthoritiveUrl
    {
        string Url
        {
            get
            {
                return ConfigurationManager.AppSettings["AuthoritiveUrl"];
            }
        }
    }
}