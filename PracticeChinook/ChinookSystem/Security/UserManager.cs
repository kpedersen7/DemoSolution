using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Microsoft.AspNet.Identity.EntityFramework;    //UserStore, ApplicationDbcontext
using Microsoft.AspNet.Identity;                    //UserManager
using System.ComponentModel;                        //ODS
using ChinookSystem.DAL;                            //Chinook Context
using ChinookSystem.Data.Entities;                  //Entities
#endregion
namespace ChinookSystem.Security
{
    [DataObject]
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager()
            : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
        }

        #region
        //the basic minimum information need for a asp.net user is
        //username, password, email
        private const string STR_WEBMASTER_USERNAME = "WebMaster";
        private const string STR_DEFAULT_PASSWORD = "Pa$$word1";
        //the {0} will be replaced us the respective username
        private const string STR_EMAIL_FORMAT = "{0}@Chinook.ab.ca";
        //the generic username will be made up of entity's firstname and lastname
        private const string STR_USERNAME_FORMAT = "{0}.{1}";
        #endregion

        //code to add a generic webmaster for the application
        public void AddWebMaster()
        {
            if (!Users.Any(u => u.UserName.Equals(STR_WEBMASTER_USERNAME)))
            {
                var webmasterAccount = new ApplicationUser()
                {
                    UserName = STR_WEBMASTER_USERNAME,
                    Email = string.Format(STR_EMAIL_FORMAT, STR_WEBMASTER_USERNAME)
                };
                //adds to the User table
                this.Create(webmasterAccount, STR_DEFAULT_PASSWORD);
                //add to appropriate role
                this.AddToRole(webmasterAccount.Id, SecurityRoles.WebsiteAdmins);
            }
        }

      
    }
}
