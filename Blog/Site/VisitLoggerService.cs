using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StaticVoid.Blog.Data;
using StaticVoid.Repository;

namespace StaticVoid.Blog.Site
{
    public class VisitLoggerService : IVisitLoggerService
	{
		private readonly IRepository<Visit> _visitRepository;
        private readonly ISecurityHelper _securityHelper;
		public VisitLoggerService(IRepository<Visit> visitRepository, ISecurityHelper securityHelper)
		{
			_visitRepository = visitRepository;
            _securityHelper = securityHelper;
		}


		public void LogCurrentRequest()
		{
			try
			{
                string currentUser = null;
                if (_securityHelper.CurrentUser != null)
                {
                    currentUser = _securityHelper.CurrentUser.ClaimedIdentifier;
                }
				_visitRepository.Create(new Visit
				{
					Browser = String.Format("{0} ({1})", HttpContext.Current.Request.Browser.Browser, HttpContext.Current.Request.Browser.Version),
					UserAgent = HttpContext.Current.Request.UserAgent,
					IpAddress = HttpContext.Current.Request.UserHostAddress,
					Url = HttpContext.Current.Request.RawUrl,
					Languages = String.Join(", ", HttpContext.Current.Request.UserLanguages),
					Referrer = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.OriginalString : null,
					Timestamp = DateTime.Now,
                    AuthenticatedUser = currentUser
				});
			}
			catch//we definitally dont want to stop serving pages if the browser doesnt provide deets.
			{
				//todo log.
			}
		}
	}
}