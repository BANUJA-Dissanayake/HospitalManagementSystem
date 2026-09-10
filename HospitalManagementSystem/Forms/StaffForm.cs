using System;
using System.Windows.Forms;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Forms
{
    public partial class StaffForm : Form
    {
        private readonly IStaffRepository _repo;
        private Staff _selected;

        public StaffForm()
        {
            InitializeComponent();
            _repo = new StaffRepository(new HospitalContext());
            SetupGridColumns();
            LoadGrid(_repo.GetAll());
        }

        private void SetupGridColumns()
        {
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Name", "Name");
            dgv.Columns.Add("ContactNumber", "Contact Number");
            dgv.Columns.Add("Role", "Role");
            dgv.Columns["Id"].DataPropertyName = "Id";
            dgv.Columns["Name"].DataPropertyName = "Name";
            dgv.Columns["ContactNumber"].DataPropertyName = "ContactNumber";
            dgv.Columns["Role"].DataPropertyName = "Role";
        }

        private void LoadGrid(System.Collections.Generic.List<Staff> staff) => dgv.DataSource = staff;

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow?.DataBoundItem is Staff s)
            {
                _selected = s;
                txtId.Text = s.Id.ToString();
                txtName.Text = s.Name;
                txtContact.Text = s.ContactNumber;
                txtRole.Text = s.Role;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var staff = new Staff(txtName.Text, txtContact.Text, txtRole.Text);
                _repo.Add(staff);
                MessageBox.Show("Staff member added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Select a staff member from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _selected.Name = txtName.Text;
                _selected.ContactNumber = txtContact.Text;
                _selected.Role = txtRole.Text;
                _repo.Update(_selected);
                MessageBox.Show("Staff member updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Select a staff member from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Delete staff member '{_selected.Name}'?",
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
                MessageBox.Show(DescribeError(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearFields();

        private void ClearFields()
        {
            _selected = null;
            txtId.Clear();
            txtName.Clear();
            txtContact.Clear();
            txtRole.Clear();
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
