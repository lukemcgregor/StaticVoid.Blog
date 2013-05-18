using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
    public class Securable
    {
        public int Id { get; set; }
        public ICollection<User> Members { get; set; }
    }
}
