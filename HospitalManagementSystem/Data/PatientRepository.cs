using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();
        Patient GetById(int id);
        List<Patient> Search(string keyword);
        void Add(Patient patient);
        void Update(Patient patient);
        void Delete(Patient patient);
    }

    public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
        public PatientRepository(HospitalContext context) : base(context) { }

        public Patient GetById(int id) => Context.Patients.Find(id);

        public List<Patient> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
            keyword = keyword.Trim().ToLower();
            return Context.Patients
                .Where(p => p.Name.ToLower().Contains(keyword) || p.ContactNumber.Contains(keyword))
                .ToList();
        }

        public void Add(Patient patient)
        {
            Context.Patients.Add(patient);
            Context.SaveChanges();
        }

        public void Update(Patient patient)
        {
            Context.Patients.Update(patient);
            Context.SaveChanges();
        }
    }
}
