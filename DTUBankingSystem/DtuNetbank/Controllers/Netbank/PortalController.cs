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

        protected IIdentity GetCurrentUser()
        {
            return User.Identity;
        }

    }
}