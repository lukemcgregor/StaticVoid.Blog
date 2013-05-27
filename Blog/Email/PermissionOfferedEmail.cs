using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Email
{
    public class PermissionOfferedEmail : IEmailMessage
    {
        private readonly string _nameOfOfferer;
        private readonly string _permissionName;
        private readonly string _registrationUrl;

        public PermissionOfferedEmail(string to, string nameOfOfferer, string permissionName, string registrationUrl)
        {
            To = to;
            _nameOfOfferer = nameOfOfferer;
            _permissionName = permissionName;
            _registrationUrl = registrationUrl;
        }

        public string To { get; private set; }

        public string From
        {
            get { return "noreply@blog.staticvoid.co.nz"; }
        }

        public string FromName
        {
            get { return _nameOfOfferer; }
        }

        public string Subject
        {
            get { return String.Format("{0} offered {1} permissions to you", _nameOfOfferer, _permissionName); }
        }

        public string Body
        {
            get { return String.Format("Click <a href='{0}'>here</a> to retrieve {1}", _registrationUrl, _permissionName ); }
        }


    }
}
