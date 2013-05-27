using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
    public class Invitation
    {
        public int Id { get; set; }
        public int SecurableId { get; set; }
        public Securable Securable { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime InviteDate { get; set; }
    }
}
