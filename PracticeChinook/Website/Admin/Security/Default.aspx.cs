
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using ChinookSystem.Security;
#endregion

public partial class Admin_Security_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RefreshAll(object sender, EventArgs e)
    {
        DataBind();
    }

    protected void UserListView_ItemInserting(object sender, ListViewInsertEventArgs e)
    {
        //collect roles the user will be assigned 
        var addtoroles = new List<string>();
        //this will point to the checkboxlist in the listview inserttemplate
        var roles = e.Item.FindControl("RoleMemberships") as CheckBoxList;
        //control exists?
        if (roles !=null)
        {
            //cycle through the checkboxlist
            foreach(ListItem item in roles.Items)
            {
                if (item.Selected)
                {
                    addtoroles.Add(item.Value);
                }
                e.Values["RoleMemberships"] = addtoroles;
            }
        }

    }

    protected void UnregisteredUsersGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        UnregisteredUsersGridView.SelectedIndex = e.NewSelectedIndex;
        GridViewRow agvrow = UnregisteredUsersGridView.SelectedRow;
        if (agvrow !=null)
        {
            string username = (agvrow.FindControl("AssignedUsername") as TextBox).Text;
            string email = (agvrow.FindControl("AssignedEmail") as TextBox).Text;

            UnregisteredUserType usertype = (UnregisteredUserType)Enum.Parse(typeof(UnregisteredUserType), agvrow.Cells[1].Text);
            UnregisteredUser user = new UnregisteredUser()
            {
                Id = int.Parse(UnregisteredUsersGridView.SelectedDataKey.Value.ToString()),
                UserType=usertype,
                FirstName = agvrow.Cells[2].Text,
                LastName = agvrow.Cells[3].Text,
                AssignedUserName = username,
                AssignedEmail = email
            };

            //Register an unregistered used
            UserManager sysmgr = new UserManager();
            sysmgr.RegisterUser(user);
            DataBind();
        }
    }
}