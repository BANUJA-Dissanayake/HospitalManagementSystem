using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data
{
    public interface IInvoiceRepository
    {
        List<Invoice> GetAll();
        List<Invoice> GetByPatientId(int patientId);
        void Add(Invoice invoice);
        void Update(Invoice invoice);
        void Delete(Invoice invoice);
    }

    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(HospitalContext context) : base(context) { }

        public override List<Invoice> GetAll() =>
            Context.Invoices
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .OrderByDescending(i => i.IssuedAt)
                .ToList();

        public List<Invoice> GetByPatientId(int patientId) =>
            Context.Invoices.Where(i => i.PatientId == patientId).ToList();

        public void Add(Invoice invoice)
        {
            Context.Invoices.Add(invoice);
            Context.SaveChanges();
        }

        public void Update(Invoice invoice)
        {
            Context.Invoices.Update(invoice);
            Context.SaveChanges();
        }
    }
}
