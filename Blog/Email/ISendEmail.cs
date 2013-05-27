using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Email
{
    public interface ISendEmail
    {
        void Send(IEmailMessage message);
    }
}
