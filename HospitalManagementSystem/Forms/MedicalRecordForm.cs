using System;
using System.Windows.Forms;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Forms
{
    public partial class MedicalRecordForm : Form
    {
        private readonly IMedicalRecordRepository _repo;
        private readonly IPatientRepository _patients;
        private MedicalRecord _selected;
        private bool _loadingFilter;

        public MedicalRecordForm()
        {
            InitializeComponent();
            var context = new HospitalContext();
            _repo = new MedicalRecordRepository(context);
            _patients = new PatientRepository(context);

            SetupGridColumns();
            LoadLookups();
            LoadGrid(_repo.GetAll());
        }

        private void LoadLookups()
        {
            cmbPatient.DataSource = _patients.GetAll();

            _loadingFilter = true;
            cmbFilterPatient.Items.Clear();
            cmbFilterPatient.Items.Add("-- All Patients --");
            foreach (var p in _patients.GetAll())
                cmbFilterPatient.Items.Add(p);
            cmbFilterPatient.SelectedIndex = 0;
            _loadingFilter = false;
        }

        private void SetupGridColumns()
        {
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("PatientName", "Patient");
            dgv.Columns.Add("VisitDate", "Visit Date");
            dgv.Columns.Add("Diagnosis", "Diagnosis");
            dgv.Columns.Add("Notes", "Notes");
            dgv.Columns["Id"].DataPropertyName = "Id";
            dgv.Columns["PatientName"].DataPropertyName = "PatientName";
            dgv.Columns["VisitDate"].DataPropertyName = "VisitDate";
            dgv.Columns["Diagnosis"].DataPropertyName = "Diagnosis";
            dgv.Columns["Notes"].DataPropertyName = "Notes";
            dgv.Columns["VisitDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
            dgv.Columns["Notes"].Width = 250;
        }

        private void LoadGrid(System.Collections.Generic.List<MedicalRecord> records) => dgv.DataSource = records;

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow?.DataBoundItem is MedicalRecord r)
            {
                _selected = r;
                txtId.Text = r.Id.ToString();
                for (int i = 0; i < cmbPatient.Items.Count; i++)
                {
                    if (cmbPatient.Items[i] is Patient p && p.Id == r.PatientId)
                    {
                        cmbPatient.SelectedIndex = i;
                        break;
                    }
                }
                dtpVisitDate.Value = r.VisitDate;
                txtDiagnosis.Text = r.Diagnosis;
                txtNotes.Text = r.Notes;
            }
        }

        private void cmbFilterPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingFilter) return;

            if (cmbFilterPatient.SelectedItem is Patient p)
                LoadGrid(_repo.GetByPatientId(p.Id));
            else
                LoadGrid(_repo.GetAll());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!(cmbPatient.SelectedItem is Patient patient))
            {
                MessageBox.Show("Choose a patient.", "Missing information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var record = new MedicalRecord(patient.Id, dtpVisitDate.Value, txtDiagnosis.Text, txtNotes.Text);
                _repo.Add(record);
                MessageBox.Show("Medical record added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Select a record from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!(cmbPatient.SelectedItem is Patient patient))
            {
                MessageBox.Show("Choose a patient.", "Missing information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDiagnosis.Text))
            {
                MessageBox.Show("Diagnosis cannot be empty.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _selected.PatientId = patient.Id;
                _selected.VisitDate = dtpVisitDate.Value;
                _selected.Diagnosis = txtDiagnosis.Text.Trim();
                _selected.Notes = txtNotes.Text.Trim();
                _repo.Update(_selected);
                MessageBox.Show("Medical record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid(_repo.GetAll());
                ClearFields();
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
                MessageBox.Show("Select a record from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Delete this medical record?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            if (cmbPatient.Items.Count > 0) cmbPatient.SelectedIndex = 0;
            dtpVisitDate.Value = DateTime.Now;
            txtDiagnosis.Clear();
            txtNotes.Clear();
            dgv.ClearSelection();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cmbFilterPatient.SelectedIndex = 0;
            LoadLookups();
            LoadGrid(_repo.GetAll());
        }

        private void btnBack_Click(object sender, EventArgs e) => Close();

        private static string DescribeError(Exception ex) =>
            ex.InnerException != null ? ex.Message + "\n\nDetails: " + ex.InnerException.Message : ex.Message;
    }
}
