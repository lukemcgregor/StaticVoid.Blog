using StaticVoid.Blog.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Site
{
    public interface ISecurityHelper
    {
        OpenIdUser CurrentUser { get; }
    }
}
