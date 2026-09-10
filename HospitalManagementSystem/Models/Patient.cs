using System;
using System.Collections.Generic;

namespace HospitalManagementSystem.Models
{
    public class Patient : Person
    {
        private DateTime _dateOfBirth;

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Date of birth cannot be in the future.");
                _dateOfBirth = value;
            }
        }

        // Navigation property: one patient has many medical records (real DB relationship).
        public List<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

        protected Patient() { }

        public Patient(string name, string contactNumber, DateTime dateOfBirth)
            : base(name, contactNumber)
        {
            DateOfBirth = dateOfBirth;
        }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public override string PerformDuties() =>
            $"{Name} is registered for care ({MedicalRecords.Count} recorded visit(s)).";
    }
}
