using System;
using System.Windows.Forms;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Forms
{
    public partial class MainMenuForm : Form
    {
        private readonly User _currentUser;

        public MainMenuForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            lblWelcome.Text = $"Signed in as {_currentUser.Username} ({_currentUser.Role})";
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            using (var form = new PatientForm()) form.ShowDialog();
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            using (var form = new DoctorForm()) form.ShowDialog();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            using (var form = new StaffForm()) form.ShowDialog();
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            using (var form = new AppointmentForm()) form.ShowDialog();
        }

        private void btnMedicalRecords_Click(object sender, EventArgs e)
        {
            using (var form = new MedicalRecordForm()) form.ShowDialog();
        }

        private void btnBilling_Click(object sender, EventArgs e)
        {
            using (var form = new BillingForm()) form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
