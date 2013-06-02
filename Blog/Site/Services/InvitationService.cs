using StaticVoid.Blog.Data;
using StaticVoid.Blog.Email;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Services
{
    public interface IInvitationService
    {
        void Invite(int securableId, string inviteeEmail, string fromName);
    }

    public class InvitationService : IInvitationService
    {
        private readonly IRepository<Data.Securable> _securableRepo;
        private readonly IRepository<Data.Invitation> _invitationRepo;
        private readonly ISendEmail _emailSender;

        public InvitationService(
            IRepository<Data.Securable> securableRepo,
            IRepository<Data.Invitation> invitationRepo,
            ISendEmail emailSender)
        {
            _securableRepo = securableRepo;
            _invitationRepo = invitationRepo;
            _emailSender = emailSender;
        }

        public void Invite(int securableId, string inviteeEmail, string fromName)
        {
            var invitation = new Invitation
            {
                SecurableId = securableId,
                Email = inviteeEmail,
                Token = Guid.NewGuid().ToString(),
                InviteDate = DateTime.Now
            };

            _invitationRepo.Create(invitation);
            
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            _emailSender.Send(new PermissionOfferedEmail(inviteeEmail, fromName, _securableRepo.GetBy(s=>s.Id ==securableId).Name,
                urlHelper.Action("Register", "Account", new { Token = invitation.Token, Area = "" }, HttpContext.Current.Request.Url.Scheme)));
        }
    }
}