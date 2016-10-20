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
        }//eom

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnRegisteredUserProfile> ListAllUnRegisteredUsers()
        {
            using (var context = new ChinookContext())
            {
                //get the list of registered employees
                //this will come from the AspNetUser table <ApplicationUser>
                //where the int? Employee attribute has a value
                var registeredemployees = from emp in Users
                                          where emp.EmployeeId.HasValue
                                          select emp.EmployeeId;
                //compare the registeredemployee list to the user table Employees
                //extract the Employees that are not registered
                var unregisteredemployees = from emp in context.Employees
                                            where !registeredemployees.Any(eid => emp.EmployeeId == eid)
                                            select new UnRegisteredUserProfile()
                                            {
                                                id = emp.EmployeeId,
                                                FirstName = emp.FirstName,
                                                Lastname = emp.LastName,
                                                UserType = UnregisteredUserType.Employee
                                            };

                //get the list of registered customers
                //this will come from the AspNetUser table <ApplicationUser>
                //where the int? CustomerId attribute has a value
                var registeredcustomers = from cus in Users
                                          where cus.CustomerId.HasValue
                                          select cus.CustomerId;
                //compare the registeredcustomer list to the user table Customer
                //extract the Customers that are not registered
                var unregisteredcustomers = from cus in context.Customers
                                            where !registeredcustomers.Any(cid => cus.CustomerId == cid)
                                            select new UnRegisteredUserProfile()
                                            {
                                                id = cus.CustomerId,
                                                FirstName = cus.FirstName,
                                                Lastname = cus.LastName,
                                                UserType = UnregisteredUserType.Customer
                                            };
                //make one dataset out of the two unregistered user types
                return unregisteredemployees.Union(unregisteredcustomers).ToList();
            }
        }//eom
      
    }//eoc
}//eon
