<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Security_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row jumbotron">
        <h1>Site Administration</h1>
    </div>

    <div class="row">
        <div class="col-md-12">
            <!-- navigation tabs-->
            <ul class="nav nav-tabs">
                <li class="active"><a href="#users" data-toggle="tab">Users</a></li>
                <li><a href="#roles" data-toggle="tab">Roles</a></li>
                <li><a href="#unregistered" data-toggle="tab">Unregistered Users</a></li>
            </ul>
            <!-- create the needed controls for the designated tab -->
            <div class="tab-content">
                <!--  user tab  -->
                <div class="tab-pane fade in active" id="users">
                    <h2>User CRUD</h2>
                </div>
                <!--  user tab  -->
                <div class="tab-pane fade" id="roles">
                    <asp:ListView ID="RoleListView" runat="server">
                        <EmptyDataTemplate>
                            <span>No Security roles have been set up.</span>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <div class="row bginfo">
                                <div class="col-sm-3 h4">Action</div>
                                <div class="col-sm-3 h4">Role</div>
                                <div class="col-sm-6 h4">Members</div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="col-sm-3">
                                <asp:LinkButton ID="RemoveRole" runat="server" 
                                    CommandName="Delete">Remove</asp:LinkButton>
                            </div>
                            <div class="col-sm-3">
                               <%# Item.RoleName %>
                            </div>
                            <div class="col-sm-6">
                                <asp:Repeater ID="RoleUserReapter" runat="server"
                                    DataSource="<%# Item.UserNames%>"
                                    ItemType="System.String">
                                    <ItemTemplate>
                                         <%# Item %>
                                    </ItemTemplate>
                                    <SeparatorTemplate>, </SeparatorTemplate>
                                </asp:Repeater>
                            </div>
                        </ItemTemplate>
                        <InsertItemTemplate>
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="InsertRole" runat="server" 
                                    CommandName="Insert">Insert</asp:LinkButton>
                                    <asp:LinkButton ID="CancelButton" runat="server" 
                                    CommandName="Cancel">Cancel</asp:LinkButton>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="RoleName" runat="server"
                                        text='<%# BindItem.RoleName %>' 
                                        placeholder="Role Name"></asp:TextBox>
                                </div>
                            </div>
                        </InsertItemTemplate>
                    </asp:ListView>
                    <asp:ObjectDataSource ID="RoleListViewODS" runat="server">

                    </asp:ObjectDataSource>
                </div>
                <!--  user tab  -->
                <div class="tab-pane fade" id="unregistered">
                    <h2>UnRegistered CRUD</h2>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

