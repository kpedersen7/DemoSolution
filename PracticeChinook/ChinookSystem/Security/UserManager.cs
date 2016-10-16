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

        #region Standard CRUD Operations

        //this will be the source data for the Users tab 
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<UserProfile> ListAllUsers()
        {
            var rm = new RoleManager();
            var results = from person in Users.ToList()
                          select new UserProfile()
                          {
                              UserId = person.Id,                       //security
                              UserName = person.UserName,               //security
                              Email = person.Email,                     //security
                              EmailConfirmed = person.EmailConfirmed,   //security
                              CustomerId = person.CustomerId,           //ApplicationUser
                              EmployeeId = person.EmployeeId,           //ApplicationUser
                              Rolememberships = person.Roles.Select(r => rm.FindById(r.RoleId).Name)    //security

                          };

            //get any first name of users
            using (var context = new ChinookContext())
            {
                Employee temp;
                foreach (var person in results)
                {
                    if (person.EmployeeId.HasValue)
                    {
                        temp = context.Employees.Find(person.EmployeeId);
                        person.FirstName = temp.FirstName;
                        person.LastName = temp.LastName;
                    }
                }
            }
            return results.ToList();
        }

        //this method will be used to Add a new user
        [DataObjectMethod(DataObjectMethodType.Insert,true)]
        public void AddUser(UserProfile userinfo)
        {
            var userAccount = new ApplicationUser()
            {
                UserName = userinfo.UserName,
                Email = userinfo.Email
            };
            this.Create(userAccount, STR_DEFAULT_PASSWORD);
            foreach(var roleName in userinfo.Rolememberships)
            {
                this.AddToRole(userAccount.Id, roleName);
            }
        }

        //this method will be used to Remove an existing user
        [DataObjectMethod(DataObjectMethodType.Delete,true)]
        public void RemoveUser(UserProfile userinfo)
        {
            if(userinfo.UserName.Equals(STR_WEBMASTER_USERNAME))
            {
                throw new Exception("the webmaster account cannot be removed");
            }
            this.Delete(this.FindById(userinfo.UserId));
        }
        #endregion

        #region Business Process Operation: Register unregistered users
        //Create a list of unregistered users (combined employees and customers) for display and processing
        //in the 3rd tab

            //list the unregistered users
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List <UnregisteredUser> ListAllUnregisteredUsers()
        {
            using (var context = new ChinookContext())
            {
                //make an in-memory list of employees who have login accounts
                var registeredEmployees = (from emp in Users
                                           where emp.EmployeeId.HasValue
                                           select emp.EmployeeId).ToList();
                //make an in-memoey list of employees who do not have login accounts
                var unregisteredEmployees = (from emp in context.Employees
                                             where !registeredEmployees.Any(eid => emp.EmployeeId == eid)
                                             select new UnregisteredUser
                                             {
                                                 Id= emp.EmployeeId,
                                                 FirstName = emp.FirstName,
                                                 LastName = emp.LastName,
                                                 UserType = UnregisteredUserType.Employee
                                             }).ToList();

                //make an in-memory list of customers who have login accounts
                var registeredCustomers = (from cus in Users
                                           where cus.CustomerId.HasValue
                                           select cus.CustomerId).ToList();
                //make an in-memoey list of customers who do not have login accounts
                var unregisteredCustomers = (from cus in context.Customers
                                             where !registeredCustomers.Any(cid => cus.CustomerId == cid)
                                             select new UnregisteredUser
                                             {
                                                 Id = cus.CustomerId,
                                                 FirstName = cus.FirstName,
                                                 LastName = cus.LastName,
                                                 UserType = UnregisteredUserType.Customer
                                             }).ToList();
                // use Linq Union to merge the two unregistered lists
                return unregisteredEmployees.Union(unregisteredCustomers).ToList();
            }
        }

        //register the unregistered user
        public void RegisterUser(UnregisteredUser userinfo)
        {
            //one could create a random password for each unregistered user
            //possibly using GUID

            //to have a registered user, you need to create an Applicatiionuser instance
            //then you will do a Create() with this instance
            var newuseraccount = new ApplicationUser()
            {
                UserName = userinfo.AssignedUserName,
                Email = userinfo.AssignedEmail
            };
            //determine if you are creating an employee or customer account
            switch (userinfo.UserType)
            {
                case UnregisteredUserType.Customer:
                    {
                        newuseraccount.CustomerId = userinfo.Id;
                        break;
                    }
                case UnregisteredUserType.Employee:
                    {
                        newuseraccount.EmployeeId = userinfo.Id;
                        break;
                    }
            }
            this.Create(newuseraccount, STR_DEFAULT_PASSWORD);

            //create the AspNetUserRole record
            //this must wait until the AspNetUser record exits
            switch (userinfo.UserType)
            {
                case UnregisteredUserType.Customer:
                    {
                        this.AddToRole(newuseraccount.Id, SecurityRoles.RegisteredUsers);
                        break;
                    }
                case UnregisteredUserType.Employee:
                    {
                        this.AddToRole(newuseraccount.Id, SecurityRoles.Staff);
                        break;
                    }
            }
        }
        #endregion
    }
}
