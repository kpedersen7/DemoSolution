using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Data.Entities; //entity classes
using ChinookSystem.DAL;            //context class
using System.ComponentModel;        //ODS
#endregion
namespace ChinookSystem.BLL
{
    [DataObject]
    public class ArtistController
    {
        //dump the entire artist table
        //this will use EntityFramework access
        //Entity classes will be used to define the data

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Artist> Artist_ListAll()
        {
            //set up the transaction area
            using (var context = new ChinookContext())
            {
                return context.Artists.ToList();
            }
        }
    }
}
