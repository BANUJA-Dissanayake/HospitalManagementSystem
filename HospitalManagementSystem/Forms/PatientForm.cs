using System;
using System.Windows.Forms;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Forms
{
    public partial class PatientForm : Form
    {
        private readonly IPatientRepository _repo;
        private Patient _selected;

        public PatientForm()
        {
            InitializeComponent();
            _repo = new PatientRepository(new HospitalContext());
            SetupGridColumns();
            LoadGrid(_repo.GetAll());
        }

        private void SetupGridColumns()
        {
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Name", "Name");
            dgv.Columns.Add("ContactNumber", "Contact Number");
            dgv.Columns.Add("DateOfBirth", "Date of Birth");
            dgv.Columns.Add("Age", "Age");
            dgv.Columns["Id"].DataPropertyName = "Id";
            dgv.Columns["Name"].DataPropertyName = "Name";
            dgv.Columns["ContactNumber"].DataPropertyName = "ContactNumber";
            dgv.Columns["DateOfBirth"].DataPropertyName = "DateOfBirth";
            dgv.Columns["Age"].DataPropertyName = "Age";
            dgv.Columns["DateOfBirth"].DefaultCellStyle.Format = "yyyy-MM-dd";
        }

        private void LoadGrid(System.Collections.Generic.List<Patient> patients)
        {
            dgv.DataSource = patients;
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow?.DataBoundItem is Patient p)
            {
                _selected = p;
                txtId.Text = p.Id.ToString();
                txtName.Text = p.Name;
                txtContact.Text = p.ContactNumber;
                dtpDob.Value = p.DateOfBirth;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var patient = new Patient(txtName.Text, txtContact.Text, dtpDob.Value);
                _repo.Add(patient);
                MessageBox.Show("Patient added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(_repo.GetAll());
                ClearFields();
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
                MessageBox.Show("Select a patient from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _selected.Name = txtName.Text;
                _selected.ContactNumber = txtContact.Text;
                _selected.DateOfBirth = dtpDob.Value;
                _repo.Update(_selected);
                MessageBox.Show("Patient updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(_repo.GetAll());
                ClearFields();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                MessageBox.Show("Select a patient from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Delete patient '{_selected.Name}'? Patients with existing appointments cannot be deleted.",
                "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _repo.Delete(_selected);
                LoadGrid(_repo.GetAll());
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not delete this patient. They may still have appointments, medical records or invoices on file.\n\n" + DescribeError(ex),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearFields();

        private void ClearFields()
        {
            _selected = null;
            txtId.Clear();
            txtName.Clear();
            txtContact.Clear();
            dtpDob.Value = DateTime.Now;
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e) => LoadGrid(_repo.Search(txtSearch.Text));

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadGrid(_repo.GetAll());
        }

        private void btnBack_Click(object sender, EventArgs e) => Close();

        private static string DescribeError(Exception ex) =>
            ex.InnerException != null ? ex.Message + "\n\nDetails: " + ex.InnerException.Message : ex.Message;
    }
}
