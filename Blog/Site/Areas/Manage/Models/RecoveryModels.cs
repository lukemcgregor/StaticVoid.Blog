using StaticVoid.Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Areas.Manage.Models
{
    /// <summary>
    /// Stored temporarially when someone uploads an archive.
    /// </summary>
    public class TemporaryUploadedBlogBackup
    {
        public string CorrelationToken { get; set; }
        public int BlogId { get; set; }
        public DateTime UploadTime { get; set; }
        public BlogBackup Backup { get; set; }
    }

    /// <summary>
    /// Used to serialise/deserialise an archive file
    /// </summary>
    public class BlogBackup
    {
        public List<Post> Posts { get; set; }
        public List<Redirect> Redirects { get; set; }
        public BlogTemplate BlogTemplate { get; set; }
    }
}