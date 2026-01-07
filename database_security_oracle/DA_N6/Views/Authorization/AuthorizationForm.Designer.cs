namespace DA_N6.Views.Authorization
{
    partial class AuthorizationForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControlAuth = new System.Windows.Forms.TabControl();
            this.tabPageUser = new System.Windows.Forms.TabPage();
            this.tabPageRole = new System.Windows.Forms.TabPage();
            
            // User Tab Controls
            this.lblUser = new System.Windows.Forms.Label();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.cboTables = new System.Windows.Forms.ComboBox();
            this.grpPermissions = new System.Windows.Forms.GroupBox();
            this.chkSelect = new System.Windows.Forms.CheckBox();
            this.chkInsert = new System.Windows.Forms.CheckBox();
            this.chkUpdate = new System.Windows.Forms.CheckBox();
            this.chkDelete = new System.Windows.Forms.CheckBox();
            this.btnGrant = new System.Windows.Forms.Button();
            this.btnRevoke = new System.Windows.Forms.Button();

            // Role Tab Controls
            this.grpCreateRole = new System.Windows.Forms.GroupBox();
            this.lblNewRole = new System.Windows.Forms.Label();
            this.txtRoleName = new System.Windows.Forms.TextBox();
            this.btnCreateRole = new System.Windows.Forms.Button();
            
            this.grpRolePerms = new System.Windows.Forms.GroupBox();
            this.lblSelectRole = new System.Windows.Forms.Label();
            this.cboRoles = new System.Windows.Forms.ComboBox();
            this.lblRoleTable = new System.Windows.Forms.Label();
            this.cboRoleTables = new System.Windows.Forms.ComboBox();
            this.chkRoleSelect = new System.Windows.Forms.CheckBox();
            this.chkRoleInsert = new System.Windows.Forms.CheckBox();
            this.chkRoleUpdate = new System.Windows.Forms.CheckBox();
            this.chkRoleDelete = new System.Windows.Forms.CheckBox();
            this.btnGrantRolePerm = new System.Windows.Forms.Button();
            this.btnRevokeRolePerm = new System.Windows.Forms.Button();
            
            this.grpAssignRole = new System.Windows.Forms.GroupBox();
            this.lblAssignUser = new System.Windows.Forms.Label();
            this.cboAssignUsers = new System.Windows.Forms.ComboBox();
            this.lblAssignRole = new System.Windows.Forms.Label();
            this.cboAssignRoles = new System.Windows.Forms.ComboBox();
            this.btnAssignRole = new System.Windows.Forms.Button();
            this.btnRevokeRoleFromUser = new System.Windows.Forms.Button();

            // 
            // tabControlAuth
            // 
            this.tabControlAuth.Controls.Add(this.tabPageUser);
            this.tabControlAuth.Controls.Add(this.tabPageRole);
            this.tabControlAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlAuth.Location = new System.Drawing.Point(0, 0);
            this.tabControlAuth.Name = "tabControlAuth";
            this.tabControlAuth.SelectedIndex = 0;
            this.tabControlAuth.Size = new System.Drawing.Size(800, 600);
            this.tabControlAuth.TabIndex = 0;

            // 
            // tabPageUser
            // 
            this.tabPageUser.Controls.Add(this.btnRevoke);
            this.tabPageUser.Controls.Add(this.btnGrant);
            this.tabPageUser.Controls.Add(this.grpPermissions);
            this.tabPageUser.Controls.Add(this.cboTables);
            this.tabPageUser.Controls.Add(this.lblTable);
            this.tabPageUser.Controls.Add(this.cboUsers);
            this.tabPageUser.Controls.Add(this.lblUser);
            this.tabPageUser.Location = new System.Drawing.Point(4, 22);
            this.tabPageUser.Name = "tabPageUser";
            this.tabPageUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUser.Size = new System.Drawing.Size(792, 574);
            this.tabPageUser.Text = "Phân Quyền User";
            this.tabPageUser.UseVisualStyleBackColor = true;

            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(30, 30);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(60, 13);
            this.lblUser.Text = "Chọn User:";

            // 
            // cboUsers
            // 
            this.cboUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsers.FormattingEnabled = true;
            this.cboUsers.Location = new System.Drawing.Point(100, 27);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(200, 21);
            this.cboUsers.SelectedIndexChanged += new System.EventHandler(this.cboUsers_SelectedIndexChanged);

            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(30, 70);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(63, 13);
            this.lblTable.Text = "Chọn Bảng:";

            // 
            // cboTables
            // 
            this.cboTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTables.FormattingEnabled = true;
            this.cboTables.Location = new System.Drawing.Point(100, 67);
            this.cboTables.Name = "cboTables";
            this.cboTables.Size = new System.Drawing.Size(200, 21);
            this.cboTables.SelectedIndexChanged += new System.EventHandler(this.cboTables_SelectedIndexChanged);

            // 
            // grpPermissions
            // 
            this.grpPermissions.Controls.Add(this.chkDelete);
            this.grpPermissions.Controls.Add(this.chkUpdate);
            this.grpPermissions.Controls.Add(this.chkInsert);
            this.grpPermissions.Controls.Add(this.chkSelect);
            this.grpPermissions.Location = new System.Drawing.Point(33, 120);
            this.grpPermissions.Name = "grpPermissions";
            this.grpPermissions.Size = new System.Drawing.Size(300, 150);
            this.grpPermissions.Text = "Các quyền (Privileges)";

            // 
            // chkSelect
            // 
            this.chkSelect.AutoSize = true;
            this.chkSelect.Location = new System.Drawing.Point(20, 30);
            this.chkSelect.Text = "SELECT (Xem)";
            this.chkSelect.Name = "chkSelect";

            // 
            // chkInsert
            // 
            this.chkInsert.AutoSize = true;
            this.chkInsert.Location = new System.Drawing.Point(20, 60);
            this.chkInsert.Text = "INSERT (Thêm)";
            this.chkInsert.Name = "chkInsert";

            // 
            // chkUpdate
            // 
            this.chkUpdate.AutoSize = true;
            this.chkUpdate.Location = new System.Drawing.Point(150, 30);
            this.chkUpdate.Text = "UPDATE (Sửa)";
            this.chkUpdate.Name = "chkUpdate";

            // 
            // chkDelete
            // 
            this.chkDelete.AutoSize = true;
            this.chkDelete.Location = new System.Drawing.Point(150, 60);
            this.chkDelete.Text = "DELETE (Xóa)";
            this.chkDelete.Name = "chkDelete";

            // 
            // btnGrant
            // 
            this.btnGrant.Location = new System.Drawing.Point(100, 290);
            this.btnGrant.Name = "btnGrant";
            this.btnGrant.Size = new System.Drawing.Size(100, 30);
            this.btnGrant.Text = "GRANT (Cấp)";
            this.btnGrant.UseVisualStyleBackColor = true;
            this.btnGrant.Click += new System.EventHandler(this.btnGrant_Click);

            // 
            // btnRevoke
            // 
            this.btnRevoke.Location = new System.Drawing.Point(210, 290);
            this.btnRevoke.Name = "btnRevoke";
            this.btnRevoke.Size = new System.Drawing.Size(100, 30);
            this.btnRevoke.Text = "REVOKE (Thu hồi)";
            this.btnRevoke.UseVisualStyleBackColor = true;
            this.btnRevoke.Click += new System.EventHandler(this.btnRevoke_Click);

            // 
            // tabPageRole
            // 
            this.tabPageRole.Controls.Add(this.grpAssignRole);
            this.tabPageRole.Controls.Add(this.grpRolePerms);
            this.tabPageRole.Controls.Add(this.grpCreateRole);
            this.tabPageRole.Location = new System.Drawing.Point(4, 22);
            this.tabPageRole.Name = "tabPageRole";
            this.tabPageRole.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRole.Size = new System.Drawing.Size(792, 574);
            this.tabPageRole.Text = "Quản lý Role (Nhóm quyền)";
            this.tabPageRole.UseVisualStyleBackColor = true;

            // 
            // grpCreateRole
            // 
            this.grpCreateRole.Controls.Add(this.btnCreateRole);
            this.grpCreateRole.Controls.Add(this.txtRoleName);
            this.grpCreateRole.Controls.Add(this.lblNewRole);
            this.grpCreateRole.Location = new System.Drawing.Point(20, 20);
            this.grpCreateRole.Name = "grpCreateRole";
            this.grpCreateRole.Size = new System.Drawing.Size(700, 80);
            this.grpCreateRole.Text = "Tạo Role mới";

            // 
            // lblNewRole
            // 
            this.lblNewRole.AutoSize = true;
            this.lblNewRole.Location = new System.Drawing.Point(20, 30);
            this.lblNewRole.Text = "Tên Role:";
            this.lblNewRole.Location = new System.Drawing.Point(20, 33);

            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(80, 30);
            this.txtRoleName.Size = new System.Drawing.Size(200, 20);

            // 
            // btnCreateRole
            // 
            this.btnCreateRole.Location = new System.Drawing.Point(300, 28);
            this.btnCreateRole.Size = new System.Drawing.Size(100, 25);
            this.btnCreateRole.Text = "Tạo Role";
            this.btnCreateRole.Click += new System.EventHandler(this.btnCreateRole_Click);

            // 
            // grpRolePerms
            // 
            this.grpRolePerms.Controls.Add(this.btnRevokeRolePerm);
            this.grpRolePerms.Controls.Add(this.btnGrantRolePerm);
            this.grpRolePerms.Controls.Add(this.chkRoleDelete);
            this.grpRolePerms.Controls.Add(this.chkRoleUpdate);
            this.grpRolePerms.Controls.Add(this.chkRoleInsert);
            this.grpRolePerms.Controls.Add(this.chkRoleSelect);
            this.grpRolePerms.Controls.Add(this.cboRoleTables);
            this.grpRolePerms.Controls.Add(this.lblRoleTable);
            this.grpRolePerms.Controls.Add(this.cboRoles);
            this.grpRolePerms.Controls.Add(this.lblSelectRole);
            this.grpRolePerms.Location = new System.Drawing.Point(20, 120);
            this.grpRolePerms.Name = "grpRolePerms";
            this.grpRolePerms.Size = new System.Drawing.Size(700, 180);
            this.grpRolePerms.Text = "Cấp quyền cho Role";

            // 
            // lblSelectRole
            // 
            this.lblSelectRole.AutoSize = true;
            this.lblSelectRole.Location = new System.Drawing.Point(20, 30);
            this.lblSelectRole.Text = "Chọn Role:";

            // 
            // cboRoles
            // 
            this.cboRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoles.Location = new System.Drawing.Point(90, 27);
            this.cboRoles.Size = new System.Drawing.Size(190, 21);
            this.cboRoles.SelectedIndexChanged += new System.EventHandler(this.cboRoles_SelectedIndexChanged);

            // 
            // lblRoleTable
            // 
            this.lblRoleTable.AutoSize = true;
            this.lblRoleTable.Location = new System.Drawing.Point(300, 30);
            this.lblRoleTable.Text = "Chọn Bảng:";

            // 
            // cboRoleTables
            // 
            this.cboRoleTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoleTables.Location = new System.Drawing.Point(370, 27);
            this.cboRoleTables.Size = new System.Drawing.Size(190, 21);
            this.cboRoleTables.SelectedIndexChanged += new System.EventHandler(this.cboRoleTables_SelectedIndexChanged);

            // 
            // chkRoleSelect
            // 
            this.chkRoleSelect.Location = new System.Drawing.Point(90, 70);
            this.chkRoleSelect.Text = "SELECT";
            this.chkRoleSelect.Name = "chkRoleSelect";

            // 
            // chkRoleInsert
            // 
            this.chkRoleInsert.Location = new System.Drawing.Point(200, 70);
            this.chkRoleInsert.Text = "INSERT";
             this.chkRoleInsert.Name = "chkRoleInsert";

            // 
            // chkRoleUpdate
            // 
            this.chkRoleUpdate.Location = new System.Drawing.Point(310, 70);
            this.chkRoleUpdate.Text = "UPDATE";
            this.chkRoleUpdate.Name = "chkRoleUpdate";

            // 
            // chkRoleDelete
            // 
            this.chkRoleDelete.Location = new System.Drawing.Point(420, 70);
            this.chkRoleDelete.Text = "DELETE";
            this.chkRoleDelete.Name = "chkRoleDelete";

            // 
            // btnGrantRolePerm
            // 
            this.btnGrantRolePerm.Location = new System.Drawing.Point(90, 110);
            this.btnGrantRolePerm.Size = new System.Drawing.Size(130, 30);
            this.btnGrantRolePerm.Text = "Cấp quyền Role";
            this.btnGrantRolePerm.Click += new System.EventHandler(this.btnGrantRolePerm_Click);
            // 
            // btnRevokeRolePerm
            // 
            this.btnRevokeRolePerm.Location = new System.Drawing.Point(230, 110);
            this.btnRevokeRolePerm.Size = new System.Drawing.Size(130, 30);
            this.btnRevokeRolePerm.Text = "Thu hồi quyền Role";
            this.btnRevokeRolePerm.Click += new System.EventHandler(this.btnRevokeRolePerm_Click);

            // 
            // grpAssignRole
            // 
            this.grpAssignRole.Controls.Add(this.btnRevokeRoleFromUser);
            this.grpAssignRole.Controls.Add(this.btnAssignRole);
            this.grpAssignRole.Controls.Add(this.cboAssignRoles);
            this.grpAssignRole.Controls.Add(this.lblAssignRole);
            this.grpAssignRole.Controls.Add(this.cboAssignUsers);
            this.grpAssignRole.Controls.Add(this.lblAssignUser);
            this.grpAssignRole.Location = new System.Drawing.Point(20, 320);
            this.grpAssignRole.Name = "grpAssignRole";
            this.grpAssignRole.Size = new System.Drawing.Size(700, 100);
            this.grpAssignRole.Text = "Gán Role cho User";

            // 
            // lblAssignUser
            // 
            this.lblAssignUser.Location = new System.Drawing.Point(20, 35);
            this.lblAssignUser.Text = "Chọn User:";
            this.lblAssignUser.AutoSize = true;

            // 
            // cboAssignUsers
            // 
            this.cboAssignUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAssignUsers.Location = new System.Drawing.Point(90, 32);
            this.cboAssignUsers.Size = new System.Drawing.Size(190, 21);

            // 
            // lblAssignRole
            // 
            this.lblAssignRole.Location = new System.Drawing.Point(300, 35);
            this.lblAssignRole.Text = "Chọn Role:";
            this.lblAssignRole.AutoSize = true;

            // 
            // cboAssignRoles
            // 
            this.cboAssignRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAssignRoles.Location = new System.Drawing.Point(370, 32);
            this.cboAssignRoles.Size = new System.Drawing.Size(190, 21);

            // 
            // btnAssignRole
            // 
            this.btnAssignRole.Location = new System.Drawing.Point(580, 28);
            this.btnAssignRole.Size = new System.Drawing.Size(100, 30);
            this.btnAssignRole.Text = "Gán Role";
            this.btnAssignRole.Click += new System.EventHandler(this.btnAssignRole_Click);
            // 
            // btnRevokeRoleFromUser
            // 
            this.btnRevokeRoleFromUser.Location = new System.Drawing.Point(580, 60);
            this.btnRevokeRoleFromUser.Size = new System.Drawing.Size(100, 30);
            this.btnRevokeRoleFromUser.Text = "Gỡ Role";
            this.btnRevokeRoleFromUser.Click += new System.EventHandler(this.btnRevokeRoleFromUser_Click);

            // 
            // AuthorizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tabControlAuth);
            this.Name = "AuthorizationForm";
            this.Text = "Quản lý Phân quyền (Authorization)";
            this.tabControlAuth.ResumeLayout(false);
            this.tabPageUser.ResumeLayout(false);
            this.tabPageUser.PerformLayout();
            this.grpPermissions.ResumeLayout(false);
            this.grpPermissions.PerformLayout();
            this.tabPageRole.ResumeLayout(false);
            this.grpCreateRole.ResumeLayout(false);
            this.grpCreateRole.PerformLayout();
            this.grpRolePerms.ResumeLayout(false);
            this.grpRolePerms.PerformLayout();
            this.grpAssignRole.ResumeLayout(false);
            this.grpAssignRole.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControlAuth;
        private System.Windows.Forms.TabPage tabPageUser;
        private System.Windows.Forms.TabPage tabPageRole;
        
        // User Tab
        private System.Windows.Forms.ComboBox cboUsers;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ComboBox cboTables;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.GroupBox grpPermissions;
        private System.Windows.Forms.CheckBox chkDelete;
        private System.Windows.Forms.CheckBox chkUpdate;
        private System.Windows.Forms.CheckBox chkInsert;
        private System.Windows.Forms.CheckBox chkSelect;
        private System.Windows.Forms.Button btnRevoke;
        private System.Windows.Forms.Button btnGrant;

        // Role Tab
        private System.Windows.Forms.GroupBox grpCreateRole;
        private System.Windows.Forms.Button btnCreateRole;
        private System.Windows.Forms.TextBox txtRoleName;
        private System.Windows.Forms.Label lblNewRole;
        
        private System.Windows.Forms.GroupBox grpRolePerms;
        private System.Windows.Forms.Button btnGrantRolePerm;
        private System.Windows.Forms.CheckBox chkRoleDelete;
        private System.Windows.Forms.CheckBox chkRoleUpdate;
        private System.Windows.Forms.CheckBox chkRoleInsert;
        private System.Windows.Forms.CheckBox chkRoleSelect;
        private System.Windows.Forms.ComboBox cboRoleTables;
        private System.Windows.Forms.Label lblRoleTable;
        private System.Windows.Forms.ComboBox cboRoles;
        private System.Windows.Forms.Label lblSelectRole;

        private System.Windows.Forms.GroupBox grpAssignRole;
        private System.Windows.Forms.Button btnAssignRole;
        private System.Windows.Forms.ComboBox cboAssignRoles;
        private System.Windows.Forms.Label lblAssignRole;
        private System.Windows.Forms.ComboBox cboAssignUsers;
        private System.Windows.Forms.Label lblAssignUser;
        private System.Windows.Forms.Button btnRevokeRolePerm;
        private System.Windows.Forms.Button btnRevokeRoleFromUser;
    }
}
