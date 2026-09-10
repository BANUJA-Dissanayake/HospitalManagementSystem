using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    // Abstraction: base class defines the shared shape of everyone in the hospital
    // and forces every subclass to implement PerformDuties() (polymorphism).
    public abstract class Person
    {
        // Encapsulation: private fields, exposed only via validated properties.
        private string _name;
        private string _contactNumber;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                _name = value.Trim();
            }
        }

        public string ContactNumber
        {
            get => _contactNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Trim().Length < 7)
                    throw new ArgumentException("Enter a valid contact number (at least 7 digits).");
                _contactNumber = value.Trim();
            }
        }

        // Parameterless constructor kept protected so only EF Core (materialization)
        // and subclasses can use it; application code must go through the validating
        // constructor below.
        protected Person() { }

        protected Person(string name, string contactNumber)
        {
            Name = name;
            ContactNumber = contactNumber;
        }

        // Abstract method: every subclass must define its own duties (polymorphism).
        public abstract string PerformDuties();

        public override string ToString() => $"{Name} (ID: {Id})";
    }
}
