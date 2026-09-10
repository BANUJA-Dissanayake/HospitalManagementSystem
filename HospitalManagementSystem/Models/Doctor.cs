using System;

namespace HospitalManagementSystem.Models
{
    public class Doctor : Person
    {
        private string _specialization;

        public string Specialization
        {
            get => _specialization;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Specialization cannot be empty.");
                _specialization = value.Trim();
            }
        }

        protected Doctor() { }

        public Doctor(string name, string contactNumber, string specialization)
            : base(name, contactNumber)
        {
            Specialization = specialization;
        }

        // Polymorphism: overrides base method with doctor-specific behaviour.
        public override string PerformDuties() =>
            $"Dr. {Name} treats patients in {Specialization}.";
    }
}
