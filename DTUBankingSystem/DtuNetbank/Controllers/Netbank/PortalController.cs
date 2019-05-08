using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using DtuNetbank.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Logging;

namespace DtuNetbank.Controllers.Netbank
{
    [Authorize]
    public class PortalController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        protected ApplicationUser GetCurrentUser()
        {
            var users = UserManager.Users;
            var currentUserName = User.Identity.GetUserName();
            var applicationUser = users.Single(user => user.UserName == currentUserName);
            return applicationUser;
        }
    }
}