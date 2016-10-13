﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces 
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
#endregion
namespace ChinookSystem.Security
{
    public class RoleManager:RoleManager<IdentityRole>
    {
        public RoleManager():base (new RoleStore<IdentityRole>(new ApplicationDbContext()))
        {

        }
    }
}
