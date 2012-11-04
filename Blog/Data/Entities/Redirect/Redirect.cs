using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
    public class Redirect
    {
        public int Id { get; set; }
        public bool IsPermanent { get; set; }
        public string OldRoute { get; set; }
        public string NewRoute { get; set; }
    }
}
