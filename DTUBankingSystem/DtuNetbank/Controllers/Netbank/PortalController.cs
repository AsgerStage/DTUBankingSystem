using System.Linq;
using System.Web;
using System.Web.Mvc;
using DtuNetbank.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading;
using System.Globalization;

namespace DtuNetbank.Controllers.Netbank
{
    [Authorize]
    public class PortalController : Controller
    {
        private ApplicationUserManager _userManager;


        public PortalController()
        {
            SetThreadCulture(new CultureInfo("da-DK"));
        }

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

        protected void SetThreadCulture(CultureInfo newCulture)
        {
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
        }

    }
}