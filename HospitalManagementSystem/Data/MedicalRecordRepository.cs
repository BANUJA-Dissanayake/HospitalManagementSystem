using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data
{
    public interface IMedicalRecordRepository
    {
        List<MedicalRecord> GetAll();
        List<MedicalRecord> GetByPatientId(int patientId);
        void Add(MedicalRecord record);
        void Update(MedicalRecord record);
        void Delete(MedicalRecord record);
    }

    public class MedicalRecordRepository : RepositoryBase<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(HospitalContext context) : base(context) { }

        public override List<MedicalRecord> GetAll() =>
            Context.MedicalRecords.Include(m => m.Patient).OrderByDescending(m => m.VisitDate).ToList();

        public List<MedicalRecord> GetByPatientId(int patientId) =>
            Context.MedicalRecords
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.VisitDate)
                .ToList();

        public void Add(MedicalRecord record)
        {
            Context.MedicalRecords.Add(record);
            Context.SaveChanges();
        }

        public void Update(MedicalRecord record)
        {
            Context.MedicalRecords.Update(record);
            Context.SaveChanges();
        }
    }
}
