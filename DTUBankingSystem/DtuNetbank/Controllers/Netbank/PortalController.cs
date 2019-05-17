using System.Linq;
using System.Web;
using System.Web.Mvc;
using DtuNetbank.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading;
using System.Globalization;
using System;

namespace DtuNetbank.Controllers.Netbank
{
    [Authorize]
    [RequireHttps]
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

        /// <summary>
        /// Gets the default culture from userdata and sets value to the current thread
        /// If no value is stored then en-US is default
        /// </summary>
        protected void SetContextCulture()
        {
            var userDefLanguage = GetCurrentUser().DefaultCulture ?? "en-US";
            SetThreadCulture(userDefLanguage);
        }

        [HttpPost]
        public bool SetUserDefaultCulture(string culture, string returnUrl)
        {
            var req = HttpContext.Request.Path;
            var user = GetCurrentUser();
            using(var db = new ApplicationDbContext())
            {
                var userToUpdate = db.Users.Single(u => u.Id == user.Id);
                userToUpdate.DefaultCulture = culture;
                db.SaveChanges();
            }
            SetThreadCulture(culture);
            return true;
        }

        protected void SetThreadCulture(string language)
        {
            CultureInfo culture;
            try
            {
                culture = new CultureInfo(language);
            }
            catch (Exception e)
            {
                culture = new CultureInfo("da-DK");
            }
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

    }
}