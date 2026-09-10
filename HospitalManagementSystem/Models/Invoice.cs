using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public static class PaymentType
    {
        public const string Cash = "Cash";
        public const string Card = "Card";
        public const string Insurance = "Insurance";
    }

    public class Invoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int? AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public decimal Amount { get; set; }
        public string PaymentType { get; set; } = HospitalManagementSystem.Models.PaymentType.Cash;
        public bool IsPaid { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public string PatientName => Patient?.Name ?? $"#{PatientId}";

        public Invoice() { }

        public Invoice(int patientId, int? appointmentId, decimal amount, string paymentType)
        {
            if (amount <= 0)
                throw new ArgumentException("Invoice amount must be greater than zero.");

            PatientId = patientId;
            AppointmentId = appointmentId;
            Amount = amount;
            PaymentType = paymentType;
        }

        // Polymorphism: resolves the right IPaymentMethod implementation at
        // runtime based on the stored PaymentType and delegates to it.
        [NotMapped]
        public IPaymentMethod Method
        {
            get
            {
                switch (PaymentType)
                {
                    case HospitalManagementSystem.Models.PaymentType.Card:
                        return new CardPayment();
                    case HospitalManagementSystem.Models.PaymentType.Insurance:
                        return new InsurancePayment();
                    default:
                        return new CashPayment();
                }
            }
        }
    }
}
