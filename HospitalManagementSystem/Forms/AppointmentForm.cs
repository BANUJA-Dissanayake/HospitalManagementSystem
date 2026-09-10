using System;
using System.Linq;
using System.Windows.Forms;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Forms
{
    public partial class AppointmentForm : Form
    {
        private readonly IAppointmentRepository _repo;
        private readonly IPatientRepository _patients;
        private readonly IDoctorRepository _doctors;
        private Appointment _selected;

        public AppointmentForm()
        {
            InitializeComponent();
            var context = new HospitalContext();
            _repo = new AppointmentRepository(context);
            _patients = new PatientRepository(context);
            _doctors = new DoctorRepository(context);

            SetupGridColumns();
            cmbStatus.Items.AddRange(new object[] { AppointmentStatus.Scheduled, AppointmentStatus.Completed, AppointmentStatus.Cancelled });
            cmbStatus.SelectedIndex = 0;
            dtpScheduledAt.Value = DateTime.Now.AddHours(1);

            LoadLookups();
            LoadGrid();
        }

        private void LoadLookups()
        {
            cmbPatient.DataSource = _patients.GetAll();
            cmbDoctor.DataSource = _doctors.GetAll();
        }

        private void SetupGridColumns()
        {
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("PatientName", "Patient");
            dgv.Columns.Add("DoctorName", "Doctor");
            dgv.Columns.Add("ScheduledAt", "Scheduled At");
            dgv.Columns.Add("Status", "Status");
            dgv.Columns["Id"].DataPropertyName = "Id";
            dgv.Columns["PatientName"].DataPropertyName = "PatientName";
            dgv.Columns["DoctorName"].DataPropertyName = "DoctorName";
            dgv.Columns["ScheduledAt"].DataPropertyName = "ScheduledAt";
            dgv.Columns["Status"].DataPropertyName = "Status";
            dgv.Columns["ScheduledAt"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
        }

        private void LoadGrid() => dgv.DataSource = _repo.GetAll();

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow?.DataBoundItem is Appointment a)
            {
                _selected = a;
                txtId.Text = a.Id.ToString();
                SelectComboItem(cmbPatient, p => ((Patient)p).Id == a.PatientId);
                SelectComboItem(cmbDoctor, d => ((Doctor)d).Id == a.DoctorId);
                dtpScheduledAt.Value = a.ScheduledAt;
                cmbStatus.SelectedItem = a.Status;
            }
        }

        private static void SelectComboItem(ComboBox combo, Func<object, bool> match)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (match(combo.Items[i]))
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!(cmbPatient.SelectedItem is Patient patient) || !(cmbDoctor.SelectedItem is Doctor doctor))
            {
                MessageBox.Show("Choose a patient and a doctor.", "Missing information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var appointment = new Appointment(patient, doctor, dtpScheduledAt.Value);
                _repo.Add(appointment);
                MessageBox.Show("Appointment booked successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid();
                ClearFields();
            }
            catch (DoctorUnavailableException ex)
            {
                MessageBox.Show(ex.Message, "Doctor unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(DescribeError(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                MessageBox.Show("Select an appointment from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!(cmbPatient.SelectedItem is Patient patient) || !(cmbDoctor.SelectedItem is Doctor doctor))
            {
                MessageBox.Show("Choose a patient and a doctor.", "Missing information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _selected.PatientId = patient.Id;
                _selected.DoctorId = doctor.Id;
                _selected.ScheduledAt = dtpScheduledAt.Value;
                _selected.Status = cmbStatus.SelectedItem?.ToString() ?? AppointmentStatus.Scheduled;
                _repo.Update(_selected);
                MessageBox.Show("Appointment updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid();
                ClearFields();
            }
            catch (DoctorUnavailableException ex)
            {
                MessageBox.Show(ex.Message, "Doctor unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(DescribeError(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                MessageBox.Show("Select an appointment from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Remove this appointment from the schedule?",
                "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _repo.Delete(_selected);
                LoadGrid();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(DescribeError(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearFields();

        private void ClearFields()
        {
            _selected = null;
            txtId.Clear();
            if (cmbPatient.Items.Count > 0) cmbPatient.SelectedIndex = 0;
            if (cmbDoctor.Items.Count > 0) cmbDoctor.SelectedIndex = 0;
            dtpScheduledAt.Value = DateTime.Now.AddHours(1);
            cmbStatus.SelectedIndex = 0;
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            var all = _repo.GetAll();
            dgv.DataSource = string.IsNullOrEmpty(keyword)
                ? all
                : all.Where(a => a.PatientName.ToLower().Contains(keyword) || a.DoctorName.ToLower().Contains(keyword)).ToList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadLookups();
            LoadGrid();
        }

        private void btnBack_Click(object sender, EventArgs e) => Close();

        private static string DescribeError(Exception ex) =>
            ex.InnerException != null ? ex.Message + "\n\nDetails: " + ex.InnerException.Message : ex.Message;
    }
}
