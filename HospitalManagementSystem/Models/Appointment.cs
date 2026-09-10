using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public static class AppointmentStatus
    {
        public const string Scheduled = "Scheduled";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
    }

    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public DateTime ScheduledAt { get; set; }
        public string Status { get; set; } = AppointmentStatus.Scheduled;

        [NotMapped]
        public string PatientName => Patient?.Name ?? $"#{PatientId}";

        [NotMapped]
        public string DoctorName => Doctor?.Name ?? $"#{DoctorId}";

        public Appointment() { }

        public Appointment(Patient patient, Doctor doctor, DateTime scheduledAt)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            if (scheduledAt < DateTime.Now)
                throw new ArgumentException("Cannot schedule an appointment in the past.");

            Patient = patient;
            PatientId = patient.Id;
            Doctor = doctor;
            DoctorId = doctor.Id;
            ScheduledAt = scheduledAt;
        }
    }
}
