using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    // Demonstrates a proper one-to-many relationship (Patient -> MedicalRecords)
    // persisted to the database, replacing the old in-memory-only history list.
    public class MedicalRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }

        [NotMapped]
        public string PatientName => Patient?.Name ?? $"#{PatientId}";

        public MedicalRecord() { }

        public MedicalRecord(int patientId, DateTime visitDate, string diagnosis, string notes)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
                throw new ArgumentException("Diagnosis cannot be empty.");

            PatientId = patientId;
            VisitDate = visitDate;
            Diagnosis = diagnosis.Trim();
            Notes = notes?.Trim() ?? string.Empty;
        }
    }
}
