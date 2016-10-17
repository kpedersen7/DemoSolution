using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces 
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ChinookSystem.Security;
using System.ComponentModel;
#endregion
namespace ChinookSystem.Security
{
    [DataObject]
    public class RoleManager:RoleManager<IdentityRole>
    {
        public RoleManager():base (new RoleStore<IdentityRole>(new ApplicationDbContext()))
        {

        }

        public void AddChinookRoles()
        {
            foreach(string rolename in SecurityRoles.ChinookSecurityRoles)
            {
                //check the security tables of the Chinook system
                //to see if any new roles needs to be added to the security table data

                //the first time your application runs, all the roles
                //will be added to the security table data

                //all other times your application runs (start), only new roles
                //in the ChinookSecurityRoles will be added to the
                //security table data
                if (!Roles.Any(r => r.Name.Equals(rolename)))
                {
                    this.Create(new IdentityRole(rolename));
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<string> ListAllRoleNames()
        {
            return this.Roles.Select(r => r.Name).ToList(); 
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RoleProfile> ListAllRoles()
        {
            var um = new UserManager();
            var results = from role in Roles.ToList()
                          select new RoleProfile()
                          {
                               RoleId = role.Id,                   //security
                               RoleName = role.Name,               //security
                               UserNames = role.Users.Select(r => um.FindById(r.UserId).UserName)    //security

                          };
            return results.ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Insert,true)]
        public void AddRole(RoleProfile role)
        {
            if(!this.RoleExists(role.RoleName))
            {
                this.Create(new IdentityRole(role.RoleName));
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void RemoveRole(RoleProfile role)
        {
            this.Delete(this.FindById(role.RoleId));
        }
    }
}
