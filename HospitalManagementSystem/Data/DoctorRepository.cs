using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data
{
    public interface IDoctorRepository
    {
        List<Doctor> GetAll();
        Doctor GetById(int id);
        List<Doctor> Search(string keyword);
        void Add(Doctor doctor);
        void Update(Doctor doctor);
        void Delete(Doctor doctor);
    }

    public class DoctorRepository : RepositoryBase<Doctor>, IDoctorRepository
    {
        public DoctorRepository(HospitalContext context) : base(context) { }

        public Doctor GetById(int id) => Context.Doctors.Find(id);

        public List<Doctor> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
            keyword = keyword.Trim().ToLower();
            return Context.Doctors
                .Where(d => d.Name.ToLower().Contains(keyword) || d.Specialization.ToLower().Contains(keyword))
                .ToList();
        }

        public void Add(Doctor doctor)
        {
            Context.Doctors.Add(doctor);
            Context.SaveChanges();
        }

        public void Update(Doctor doctor)
        {
            Context.Doctors.Update(doctor);
            Context.SaveChanges();
        }
    }
}
