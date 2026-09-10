using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Forms
{
    public partial class BillingForm : Form
    {
        private readonly IInvoiceRepository _repo;
        private readonly IPatientRepository _patients;
        private readonly IAppointmentRepository _appointments;
        private Invoice _selected;

        public BillingForm()
        {
            InitializeComponent();
            var context = new HospitalContext();
            _repo = new InvoiceRepository(context);
            _patients = new PatientRepository(context);
            _appointments = new AppointmentRepository(context);

            SetupGridColumns();
            cmbPaymentType.Items.AddRange(new object[] { PaymentType.Cash, PaymentType.Card, PaymentType.Insurance });
            cmbPaymentType.SelectedIndex = 0;

            cmbPatient.DataSource = _patients.GetAll();
            RefreshAppointmentChoices();
            LoadGrid();
        }

        private void SetupGridColumns()
        {
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("PatientName", "Patient");
            dgv.Columns.Add("Amount", "Amount");
            dgv.Columns.Add("PaymentType", "Payment Type");
            dgv.Columns.Add("IsPaid", "Paid");
            dgv.Columns.Add("IssuedAt", "Issued At");
            dgv.Columns["Id"].DataPropertyName = "Id";
            dgv.Columns["PatientName"].DataPropertyName = "PatientName";
            dgv.Columns["Amount"].DataPropertyName = "Amount";
            dgv.Columns["PaymentType"].DataPropertyName = "PaymentType";
            dgv.Columns["IsPaid"].DataPropertyName = "IsPaid";
            dgv.Columns["IssuedAt"].DataPropertyName = "IssuedAt";
            dgv.Columns["Amount"].DefaultCellStyle.Format = "C";
            dgv.Columns["IssuedAt"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
        }

        private void LoadGrid() => dgv.DataSource = _repo.GetAll();

        private void RefreshAppointmentChoices()
        {
            cmbAppointment.Items.Clear();
            cmbAppointment.Items.Add("-- No linked appointment --");

            if (cmbPatient.SelectedItem is Patient patient)
            {
                var patientAppointments = _appointments.GetAll().Where(a => a.PatientId == patient.Id);
                foreach (var a in patientAppointments)
                    cmbAppointment.Items.Add(a);
            }
            cmbAppointment.SelectedIndex = 0;
        }

        private void cmbPatient_SelectedIndexChanged(object sender, EventArgs e) => RefreshAppointmentChoices();

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow?.DataBoundItem is Invoice inv)
            {
                _selected = inv;
                txtId.Text = inv.Id.ToString();

                for (int i = 0; i < cmbPatient.Items.Count; i++)
                {
                    if (cmbPatient.Items[i] is Patient p && p.Id == inv.PatientId)
                    {
                        cmbPatient.SelectedIndex = i;
                        break;
                    }
                }

                if (inv.AppointmentId.HasValue)
                {
                    for (int i = 0; i < cmbAppointment.Items.Count; i++)
                    {
                        if (cmbAppointment.Items[i] is Appointment a && a.Id == inv.AppointmentId.Value)
                        {
                            cmbAppointment.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    cmbAppointment.SelectedIndex = 0;
                }

                txtAmount.Text = inv.Amount.ToString(CultureInfo.InvariantCulture);
                cmbPaymentType.SelectedItem = inv.PaymentType;
                chkIsPaid.Checked = inv.IsPaid;
            }
        }

        private bool TryReadAmount(out decimal amount)
        {
            if (decimal.TryParse(txtAmount.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out amount) && amount > 0)
                return true;

            MessageBox.Show("Enter a valid amount greater than zero.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!(cmbPatient.SelectedItem is Patient patient))
            {
                MessageBox.Show("Choose a patient.", "Missing information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!TryReadAmount(out decimal amount)) return;

            try
            {
                int? appointmentId = cmbAppointment.SelectedItem is Appointment appt ? appt.Id : (int?)null;
                var invoice = new Invoice(patient.Id, appointmentId, amount, cmbPaymentType.SelectedItem?.ToString() ?? PaymentType.Cash)
                {
                    IsPaid = chkIsPaid.Checked
                };
                _repo.Add(invoice);
                MessageBox.Show("Invoice generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid();
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
                MessageBox.Show("Select an invoice from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!(cmbPatient.SelectedItem is Patient patient) || !TryReadAmount(out decimal amount)) return;

            try
            {
                _selected.PatientId = patient.Id;
                _selected.AppointmentId = cmbAppointment.SelectedItem is Appointment appt ? appt.Id : (int?)null;
                _selected.Amount = amount;
                _selected.PaymentType = cmbPaymentType.SelectedItem?.ToString() ?? PaymentType.Cash;
                _selected.IsPaid = chkIsPaid.Checked;
                _repo.Update(_selected);
                MessageBox.Show("Invoice updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(DescribeError(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Demonstrates interface-based polymorphism: Invoice.Method returns whichever
        // IPaymentMethod implementation matches PaymentType, and we call it without
        // knowing (or caring) which concrete class it is.
        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                MessageBox.Show("Select an invoice from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IPaymentMethod method = _selected.Method;
            string result = method.ProcessPayment(_selected.Amount);

            _selected.IsPaid = true;
            _repo.Update(_selected);
            LoadGrid();

            MessageBox.Show($"[{method.Name}] {result}", "Payment processed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                MessageBox.Show("Select an invoice from the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Delete this invoice?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            RefreshAppointmentChoices();
            txtAmount.Clear();
            cmbPaymentType.SelectedIndex = 0;
            chkIsPaid.Checked = false;
            dgv.ClearSelection();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cmbPatient.DataSource = null;
            cmbPatient.DataSource = _patients.GetAll();
            RefreshAppointmentChoices();
            LoadGrid();
        }

        private void btnBack_Click(object sender, EventArgs e) => Close();

        private static string DescribeError(Exception ex) =>
            ex.InnerException != null ? ex.Message + "\n\nDetails: " + ex.InnerException.Message : ex.Message;
    }
}
