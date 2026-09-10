using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    // Interface-based polymorphism: different payment methods know how to
    // describe and process themselves, without the caller needing to know which.
    public interface IPaymentMethod
    {
        string Name { get; }
        string ProcessPayment(decimal amount);
    }

    public class CashPayment : IPaymentMethod
    {
        public string Name => "Cash";
        public string ProcessPayment(decimal amount) => $"Received {amount:C} in cash.";
    }

    public class CardPayment : IPaymentMethod
    {
        public string CardLast4 { get; set; } = "0000";
        public string Name => "Card";
        public string ProcessPayment(decimal amount) => $"Charged {amount:C} to card ending {CardLast4}.";
    }

    public class InsurancePayment : IPaymentMethod
    {
        public string ProviderName { get; set; } = "Unknown Provider";
        public string Name => "Insurance";
        public string ProcessPayment(decimal amount) => $"Claimed {amount:C} from {ProviderName}.";
    }
}
