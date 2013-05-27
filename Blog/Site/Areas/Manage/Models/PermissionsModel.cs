using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Areas.Manage.Models
{
    public class EditPermissionsModel
    {
        public List<PermissionModel> PermissionModels { get; set; }
    }

    public class PermissionModel
    {
        public int SecurableId { get; set; }
        public string PermissionName { get; set; }
        public List<PermissionMemberModel> Members { get; set; }
    }

    public class PermissionMemberModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool CanChange { get; set; }
        public string Email { get; set; }
    }

    public class InviteModel
    {
        public string Email { get; set; }
        public int SecurableId { get; set; }
        public string PermissionName { get; set; }
    }

    public class RevokeModel
    {
        public string Email { get; set; }
        public int UserId { get; set; }
        public int SecurableId { get; set; }
    }
}