using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LumbrerasJanaLeiBSIT2A
{
    public partial class frmUsers : Form
    {
        public frmUsers()
        {
            InitializeComponent();
        }

        MyDatabase db = new MyDatabase();
        bool isUpdate = false;
        int selectedUserID = 0;
        int selectedLoginID = 0;

        private void frmUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmHome frm = new frmHome();
            frm.Show();
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            isUpdate = false;
            selectedUserID = 0;
            selectedLoginID = 0;

            string query = "SELECT tbluserinformation.userID, tbllogincredentials.LoginID, tbluserinformation.firstname, " +
                "tbluserinformation.middlename, tbluserinformation.lastname, tbluserinformation.emailAddress," +
                " tbluserinformation.homeAddress, tbluserinformation.birthDate, tbllogincredentials.user_username as 'Username'," +
                " tbllogincredentials.user_password as 'Password' FROM tbllogincredentials INNER JOIN tbluserinformation" +
                " ON tbllogincredentials.userID = tbluserinformation.userID;";

            DataTable dt = db.ExecuteReturnQuery(query);
            dgvUsers.DataSource = dt;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvUsers.Columns[0].Visible = false;
            dgvUsers.Columns[1].Visible = false;

            foreach (DataRow row in dt.Rows)
                row["Password"] = "*****";

        }

        private void LoadGrid()
        {
            isUpdate = false;
            selectedUserID = 0;
            selectedLoginID = 0;

            string query = "SELECT tbluserinformation.userID, tbllogincredentials.LoginID, tbluserinformation.firstname, " +
                "tbluserinformation.middlename, tbluserinformation.lastname, tbluserinformation.emailAddress," +
                " tbluserinformation.homeAddress, tbluserinformation.birthDate, tbllogincredentials.user_username as 'Username'," +
                " tbllogincredentials.user_password as 'Password' FROM tbllogincredentials INNER JOIN tbluserinformation" +
                " ON tbllogincredentials.userID = tbluserinformation.userID;";

            DataTable dt = db.ExecuteReturnQuery(query);
            dgvUsers.DataSource = dt;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvUsers.Columns[0].Visible = false;
            dgvUsers.Columns[1].Visible = false;

            foreach (DataRow row in dt.Rows)
                row["Password"] = "*****";

        }

        private void ClearFields()
        {
            tbFname.Text = "";
            tbMname.Text = "";
            tbLname.Text = "";
            tbEmailAdd.Text = "";
            tbHomeAdd.Text = "";
            dtpBirthDate.Text = DateTime.Today.ToString();
            tbUsername.Text = "";
            tbPassword.Text = "";
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {

                DialogResult result = MessageBox.Show("Are you sure you want to deactivate this account?", "Account Deactivation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    int id = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells[1].Value);
                    string query = "UPDATE tbllogincredentials SET is_active = 0 where LoginID = @id";

                    int affectedRows = db.ExecuteNoReturnQuery(query,
                        new MySqlParameter("@id", id));
                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Account is deactivated!");
                        LoadGrid();
                    }
                    else
                    {
                        MessageBox.Show("Deactivation failed. Please try again.");
                    }

                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to update this account?", "Update Account", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    isUpdate = true;
                    selectedUserID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells[0].Value);
                    selectedLoginID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells[1].Value);
                    tbFname.Text = dgvUsers.SelectedRows[0].Cells[2].Value.ToString();
                    tbMname.Text = dgvUsers.SelectedRows[0].Cells[3].Value.ToString();
                    tbLname.Text = dgvUsers.SelectedRows[0].Cells[4].Value.ToString();
                    tbEmailAdd.Text = dgvUsers.SelectedRows[0].Cells[5].Value.ToString();
                    tbHomeAdd.Text = dgvUsers.SelectedRows[0].Cells[6].Value.ToString();

                    if (DateTime.TryParse(dgvUsers.SelectedRows[0].Cells[7].Value.ToString(), out DateTime bDate)) ;
                    dtpBirthDate.Value = bDate;
                    tbUsername.Text = "";
                    tbPassword.Text = "";
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isUpdate == false)
            {
                
                
                string query = "INSERT INTO tbluserinformation (firstname, middlename, lastname, emailAddress, homeAddress, birthDate)" +
                " VALUES (@fname, @mname, @lname, @email, @hadd, @bDate);" +
                "SET @newUserID = LAST_INSERT_ID();" +
                "INSERT INTO tbllogincredentials (userID, user_username, user_password) VALUES (@newUserID, @username, @password);";

                int affectedRowCount = db.ExecuteNoReturnQuery(query,
                    new MySqlParameter("@fname", tbFname.Text),
                    new MySqlParameter("@mname", tbMname.Text),
                    new MySqlParameter("@lname", tbLname.Text),
                    new MySqlParameter("@email", tbEmailAdd.Text),
                    new MySqlParameter("@hadd", tbHomeAdd.Text),
                    new MySqlParameter("@bDate", dtpBirthDate.Value),
                    new MySqlParameter("@username", tbUsername.Text),
                    new MySqlParameter("@password", tbPassword.Text)
                    );

                if (affectedRowCount > 0)
                {
                    MessageBox.Show("Data Inserted!");
                    frmUsers_Load(null, null);
                }
            }
            else
            {
                //update process
                string credQuery;
                MySqlParameter[] credParams;

                if (!string.IsNullOrWhiteSpace(tbPassword.Text))
                {
                    credQuery = "UPDATE tbllogincredentials " +
                                 "SET user_username = @username, user_password = @password " +
                                 "WHERE LoginID = @loginID;";
                    credParams = new[]
                    {
                        new MySqlParameter("@username", tbUsername.Text),
                        new MySqlParameter("@password", tbPassword.Text),
                        new MySqlParameter("@loginID",  selectedLoginID)
                    };
                }
                else
                {
                    credQuery = "UPDATE tbllogincredentials " +
                                 "SET user_username = @username " +
                                 "WHERE LoginID = @loginID;";
                    credParams = new[]
                    {
                        new MySqlParameter("@username", tbUsername.Text),
                        new MySqlParameter("@loginID",  selectedLoginID)
                    };
                }

                string infoQuery =
                    "UPDATE tbluserinformation " +
                    "SET firstname = @fname, middlename = @mname, lastname = @lname, " +
                    "emailAddress = @email, homeAddress = @hadd, birthDate = @bDate " +
                    "WHERE userID = @userID;";

                int affectedInfo = db.ExecuteNoReturnQuery(infoQuery,
                    new MySqlParameter("@fname", tbFname.Text),
                    new MySqlParameter("@mname", tbMname.Text),
                    new MySqlParameter("@lname", tbLname.Text),
                    new MySqlParameter("@email", tbEmailAdd.Text),
                    new MySqlParameter("@hadd", tbHomeAdd.Text),
                    new MySqlParameter("@bDate", dtpBirthDate.Value),
                    new MySqlParameter("@userID", selectedUserID));

                int affectedCred = db.ExecuteNoReturnQuery(credQuery, credParams);

                if (affectedInfo > 0 || affectedCred > 0)
                {
                    MessageBox.Show("User updated successfully.");
                    ClearFields();
                    LoadGrid();
                }
                else
                {
                    MessageBox.Show("Update failed. Please try again.");
                }
            }

            
            
        }
    }
}
