namespace HospitalManagementSystem.Forms
{
    partial class MainMenuForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnPatients = new System.Windows.Forms.Button();
            this.btnDoctors = new System.Windows.Forms.Button();
            this.btnStaff = new System.Windows.Forms.Button();
            this.btnAppointments = new System.Windows.Forms.Button();
            this.btnMedicalRecords = new System.Windows.Forms.Button();
            this.btnBilling = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(440, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Hospital Management System";
            //
            // lblWelcome
            //
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.ForeColor = System.Drawing.Color.DimGray;
            this.lblWelcome.Location = new System.Drawing.Point(33, 62);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(120, 16);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome";
            //
            // btnPatients
            //
            this.btnPatients.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnPatients.Location = new System.Drawing.Point(40, 110);
            this.btnPatients.Name = "btnPatients";
            this.btnPatients.Size = new System.Drawing.Size(200, 55);
            this.btnPatients.TabIndex = 2;
            this.btnPatients.Text = "Patients";
            this.btnPatients.UseVisualStyleBackColor = true;
            this.btnPatients.Click += new System.EventHandler(this.btnPatients_Click);
            //
            // btnDoctors
            //
            this.btnDoctors.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnDoctors.Location = new System.Drawing.Point(260, 110);
            this.btnDoctors.Name = "btnDoctors";
            this.btnDoctors.Size = new System.Drawing.Size(200, 55);
            this.btnDoctors.TabIndex = 3;
            this.btnDoctors.Text = "Doctors";
            this.btnDoctors.UseVisualStyleBackColor = true;
            this.btnDoctors.Click += new System.EventHandler(this.btnDoctors_Click);
            //
            // btnStaff
            //
            this.btnStaff.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnStaff.Location = new System.Drawing.Point(40, 175);
            this.btnStaff.Name = "btnStaff";
            this.btnStaff.Size = new System.Drawing.Size(200, 55);
            this.btnStaff.TabIndex = 4;
            this.btnStaff.Text = "Staff";
            this.btnStaff.UseVisualStyleBackColor = true;
            this.btnStaff.Click += new System.EventHandler(this.btnStaff_Click);
            //
            // btnAppointments
            //
            this.btnAppointments.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnAppointments.Location = new System.Drawing.Point(260, 175);
            this.btnAppointments.Name = "btnAppointments";
            this.btnAppointments.Size = new System.Drawing.Size(200, 55);
            this.btnAppointments.TabIndex = 5;
            this.btnAppointments.Text = "Appointments";
            this.btnAppointments.UseVisualStyleBackColor = true;
            this.btnAppointments.Click += new System.EventHandler(this.btnAppointments_Click);
            //
            // btnMedicalRecords
            //
            this.btnMedicalRecords.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnMedicalRecords.Location = new System.Drawing.Point(40, 240);
            this.btnMedicalRecords.Name = "btnMedicalRecords";
            this.btnMedicalRecords.Size = new System.Drawing.Size(200, 55);
            this.btnMedicalRecords.TabIndex = 6;
            this.btnMedicalRecords.Text = "Medical Records";
            this.btnMedicalRecords.UseVisualStyleBackColor = true;
            this.btnMedicalRecords.Click += new System.EventHandler(this.btnMedicalRecords_Click);
            //
            // btnBilling
            //
            this.btnBilling.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnBilling.Location = new System.Drawing.Point(260, 240);
            this.btnBilling.Name = "btnBilling";
            this.btnBilling.Size = new System.Drawing.Size(200, 55);
            this.btnBilling.TabIndex = 7;
            this.btnBilling.Text = "Billing";
            this.btnBilling.UseVisualStyleBackColor = true;
            this.btnBilling.Click += new System.EventHandler(this.btnBilling_Click);
            //
            // btnLogout
            //
            this.btnLogout.Location = new System.Drawing.Point(40, 320);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(420, 40);
            this.btnLogout.TabIndex = 8;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            //
            // MainMenuForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 390);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnBilling);
            this.Controls.Add(this.btnMedicalRecords);
            this.Controls.Add(this.btnAppointments);
            this.Controls.Add(this.btnStaff);
            this.Controls.Add(this.btnDoctors);
            this.Controls.Add(this.btnPatients);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainMenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Menu";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnPatients;
        private System.Windows.Forms.Button btnDoctors;
        private System.Windows.Forms.Button btnStaff;
        private System.Windows.Forms.Button btnAppointments;
        private System.Windows.Forms.Button btnMedicalRecords;
        private System.Windows.Forms.Button btnBilling;
        private System.Windows.Forms.Button btnLogout;
    }
}
