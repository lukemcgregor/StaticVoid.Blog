using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Email
{
    public interface IEmailMessage
    {
        string To { get; }
        string From { get; }
        string FromName { get; }
        string Subject { get; }
        string Body { get; }
    }
}
