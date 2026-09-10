using System;
using System.Windows.Forms;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Forms
{
    public partial class LoginForm : Form
    {
        private readonly IUserRepository _users;
        private int _failedAttempts;
        private const int MaxAttempts = 3;

        public LoginForm()
        {
            InitializeComponent();
            var context = new HospitalContext();
            DbInitializer.Initialize(context);
            _users = new UserRepository(context);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMessage.Text = "Enter both username and password.";
                return;
            }

            try
            {
                User user = _users.Authenticate(txtUsername.Text.Trim(), txtPassword.Text);
                Hide();
                using (var menu = new MainMenuForm(user))
                {
                    menu.ShowDialog();
                }
                Close();
            }
            catch (InvalidCredentialsException ex)
            {
                _failedAttempts++;
                lblMessage.Text = ex.Message;
                txtPassword.Clear();
                txtPassword.Focus();

                if (_failedAttempts >= MaxAttempts)
                {
                    btnLogin.Enabled = false;
                    lblMessage.Text = "Too many failed attempts. Please restart the application.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect to the database.\n\n" + ex.Message, "Startup error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
