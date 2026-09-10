using System;

namespace HospitalManagementSystem.Models
{
    public class Staff : Person
    {
        private string _role;

        public string Role
        {
            get => _role;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Role cannot be empty.");
                _role = value.Trim();
            }
        }

        protected Staff() { }

        public Staff(string name, string contactNumber, string role)
            : base(name, contactNumber)
        {
            Role = role;
        }

        // Polymorphism: overrides base method with staff-specific behaviour.
        public override string PerformDuties() =>
            $"{Name} manages front-desk operations as a {Role}.";
    }
}
