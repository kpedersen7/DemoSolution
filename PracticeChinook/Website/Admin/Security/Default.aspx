<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Security_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row jumbotron">
        <h1>Site Administration</h1>
    </div>

    <div class ="row">
        <div class="col-md-12">
            <!--Nav Tabs-->
            <ul class="nav nav-tabs">
                <li class="active"><a href="#users" data-toggle="tab">Users</a></li>
                <li><a href="#roles" data-toggle="tab">Roles</a></li>
                <li><a href="#unregistered" data-toggle="tab">Unregistered Users</a></li>
            </ul>
            <!-- create tab div area for all tabs-->
            <div class="tab-content">
                <!-- User management tab-->
                <div class="tab-pane fade in active" id="users">
                    <asp:ListView ID="UserListView" runat="server"
                         DataSourceID="UserProfileODS"
                         ItemType="ChinookSystem.Security.UserProfile"
                         DataKeyNames="UserId"
                         InsertItemPosition="LastItem"
                         OnItemInserting="UserListView_ItemInserting"
                         OnItemDeleted="RefreshAll"
                         OnItemInserted="RefreshAll"
                        >
                         <EmptyDataTemplate>
                            <span>No Users have been set up.</span>
                        </EmptyDataTemplate>
                       
                        <LayoutTemplate>
                            <div class="row bg-info">
                                <div class="col-sm-2 h4">Action</div>
                                <div class="col-sm-2 h4">User Name</div>
                                <div class="col-sm-5 h4">Profile</div>
                                <div class="col-sm-3 h4">Roles</div>
                            </div>
                            <div runat="server" id="itemPlaceholder"></div>
                        </LayoutTemplate>
                       <%--  set up of display query template--%>
                        <ItemTemplate>
                            <div class="row">
                                <div class="col-sm-2">
                                    <asp:LinkButton ID="DeleteButton" runat="server" 
                                        text="Delete" CommandName="Delete"></asp:LinkButton>
                               </div>
                                <div class="col-sm-2">
                                    <%# Item.UserName %>
                                </div>
                                <div class="col-sm-5">
                                    <%# Item.Email %>
                                    <%# Item.FirstName + " " + Item.LastName %>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Repeater ID="RoleUserRepeater" runat="server"
                                        DataSource="<%# Item.Rolememberships %>"
                                         ItemType="System.String">
                                        <ItemTemplate> <%# Item %></ItemTemplate>
                                        <SeparatorTemplate>, </SeparatorTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </ItemTemplate>
                        <%--  set up of display Insert template--%>
                        <InsertItemTemplate>
                            <div class="row">
                                <div class="col-sm-2">
                                    <asp:LinkButton ID="InsertButton" runat="server" 
                                        text="Insert" CommandName="Insert"></asp:LinkButton>
                                    <asp:LinkButton ID="CancelButton" runat="server" 
                                        text="Cancel" CommandName="Cancel"></asp:LinkButton>
                               </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="UserNameTextBox" runat="server"
                                        Text='<%# BindItem.UserName %>' placeholder="User Name"></asp:TextBox> 
                                </div>
                                <div class="col-sm-5">
                                
                                     <asp:TextBox ID="EmailTextBox" runat="server"
                                        Text='<%# BindItem.Email %>' placeholder="Email"
                                        Textmode="Email"></asp:TextBox> 
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBoxList ID="RoleMemberships" runat="server"
                                         DataSourceID="RoleNameODS"
                                        ></asp:CheckBoxList>
                                </div>
                            </div>
                        </InsertItemTemplate>
                    </asp:ListView>
                    <asp:ObjectDataSource ID="UserProfileODS" runat="server" 
                        DataObjectTypeName="ChinookSystem.Security.UserProfile" 
                        DeleteMethod="RemoveUser" 
                        InsertMethod="AddUser"  
                        SelectMethod="ListAllUsers"
                        OldValuesParameterFormatString="original_{0}" 
                        TypeName="ChinookSystem.Security.UserManager">
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="RoleNameODS" runat="server"  
                        SelectMethod="ListAllRoleNames" 
                        OldValuesParameterFormatString="original_{0}" 
                        TypeName="ChinookSystem.Security.RoleManager">
                    </asp:ObjectDataSource>
                </div> <%--eop--%>
                <div class="tab-pane fade" id="roles">
                   <%-- <asp:ListView ID="RoleListView" runat="server">
                        <EmptyDataTemplate>
                            <span>No Security roles have been set up.</span>
                        </EmptyDataTemplate>
                    
                        <!-- layout template of 4 columns in view -->
                        <LayoutTemplate>
                            <div class="row bginfo">
                                <div class="col-sm-3 h4">Action</div>
                                <div class="col-sm-3 h4">Role</div>
                                <div class="col-sm-6 h4">Members</div>
                            </div>
                            <div runat="server" id="itemPlaceholder"></div>
                        </LayoutTemplate>
                        <!-- set up of display query template-->
                        <ItemTemplate>
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="DeleteButton" runat="server" 
                                        text="Delete" CommandName="Delete"></asp:LinkButton>
                               </div>
                                <div class="col-sm-3">
                                    <%# Item.RoleName %>
                                </div>
                                <div class="col-sm-6">
                                    <asp:Repeater ID="RoleUserRepeater" runat="server"
                                            DataSource="<%# Item.UserNames %>">
                                        <ItemTemplate> <%# Item %></ItemTemplate>
                                        <SeparatorTemplate>, </SeparatorTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </ItemTemplate>
                        <!-- set up of display Insert template-->
                        <InsertItemTemplate>
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="InsertButton" runat="server" 
                                        text="Insert" CommandName="Insert"></asp:LinkButton>
                                    <asp:LinkButton ID="CancelButton" runat="server" 
                                        text="Cancel" CommandName="Cancel"></asp:LinkButton>
                               </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="RoleNameTextBox" runat="server"
                                        Text='<%# BindItem.RoleName %>' placeholder="Role Name"></asp:TextBox> 
                                </div>
                            </div>
                        </InsertItemTemplate>
                    </asp:ListView>
                    <asp:ObjectDataSource ID="RoleODS" runat="server"></asp:ObjectDataSource>--%>
                </div> <%--eop--%>
                <div class="tab-pane fade" id="unregistered">
                    <asp:GridView ID="UnregisteredUsersGridView" runat="server"
                        CssClass="table table-hover" AutoGenerateColumns="False" 
                        DataSourceID="UnregisteredUsersODS" DataKeyNames="Id"
                         ItemType="ChinookSystem.Security.UnregisteredUser" OnSelectedIndexChanging="UnregisteredUsersGridView_SelectedIndexChanging">
                        <Columns>
                            <asp:CommandField SelectText="Register" ShowSelectButton="True"></asp:CommandField>
                            <asp:BoundField DataField="UserType" HeaderText="User Type" 
                                SortExpression="UserType"></asp:BoundField>
                            <asp:BoundField DataField="FirstName" HeaderText="First Name" 
                                SortExpression="FirstName"></asp:BoundField>
                               <asp:BoundField DataField="LastName" HeaderText="Last Name" 
                                SortExpression="LastName"></asp:BoundField>
                            <asp:TemplateField HeaderText="Assigned UserName" 
                                SortExpression="AssignedUserName">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("AssignedUserName") %>' 
                                        ID="AssignedUserName"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assigned Email" SortExpression="AssignedEmail">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("AssignedEmail") %>' 
                                        ID="AssignedEmail"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="UnregisteredUsersODS" runat="server" 
                        DataObjectTypeName="ChinookSystem.Security.UserProfile" 
                        SelectMethod="ListAllUnregisteredUsers"
                        OldValuesParameterFormatString="original_{0}"  
                        TypeName="ChinookSystem.Security.UserManager">
                    </asp:ObjectDataSource>
                </div> <%--eop--%>
            </div>
        </div>
    </div>
</asp:Content>

