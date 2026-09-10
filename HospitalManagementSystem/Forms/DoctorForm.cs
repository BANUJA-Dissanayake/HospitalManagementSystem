using System;
using System.Windows.Forms;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Forms
{
    public partial class DoctorForm : Form
    {
        private readonly IDoctorRepository _repo;
        private Doctor _selected;

        public DoctorForm()
        {
            InitializeComponent();
            _repo = new DoctorRepository(new HospitalContext());
            SetupGridColumns();
            LoadGrid(_repo.GetAll());
        }

        private void SetupGridColumns()
        {
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Name", "Name");
            dgv.Columns.Add("ContactNumber", "Contact Number");
            dgv.Columns.Add("Specialization", "Specialization");
            dgv.Columns["Id"].DataPropertyName = "Id";
            dgv.Columns["Name"].DataPropertyName = "Name";
            dgv.Columns["ContactNumber"].DataPropertyName = "ContactNumber";
            dgv.Columns["Specialization"].DataPropertyName = "Specialization";
        }

        private void LoadGrid(System.Collections.Generic.List<Doctor> doctors) => dgv.DataSource = doctors;

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow?.DataBoundItem is Doctor d)
            {
                _selected = d;
                txtId.Text = d.Id.ToString();
                txtName.Text = d.Name;
                txtContact.Text = d.ContactNumber;
                txtSpecialization.Text = d.Specialization;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var doctor = new Doctor(txtName.Text, txtContact.Text, txtSpecialization.Text);
                _repo.Add(doctor);
                MessageBox.Show("Doctor added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Select a doctor from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _selected.Name = txtName.Text;
                _selected.ContactNumber = txtContact.Text;
                _selected.Specialization = txtSpecialization.Text;
                _repo.Update(_selected);
                MessageBox.Show("Doctor updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Select a doctor from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Delete doctor '{_selected.Name}'? Doctors with existing appointments cannot be deleted.",
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
                MessageBox.Show("Could not delete this doctor. They may still have appointments on file.\n\n" + DescribeError(ex),
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
            txtSpecialization.Clear();
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
