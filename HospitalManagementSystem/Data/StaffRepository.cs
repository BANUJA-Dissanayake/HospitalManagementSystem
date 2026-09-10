using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data
{
    public interface IStaffRepository
    {
        List<Staff> GetAll();
        Staff GetById(int id);
        List<Staff> Search(string keyword);
        void Add(Staff staff);
        void Update(Staff staff);
        void Delete(Staff staff);
    }

    public class StaffRepository : RepositoryBase<Staff>, IStaffRepository
    {
        public StaffRepository(HospitalContext context) : base(context) { }

        public Staff GetById(int id) => Context.StaffMembers.Find(id);

        public List<Staff> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
            keyword = keyword.Trim().ToLower();
            return Context.StaffMembers
                .Where(s => s.Name.ToLower().Contains(keyword) || s.Role.ToLower().Contains(keyword))
                .ToList();
        }

        public void Add(Staff staff)
        {
            Context.StaffMembers.Add(staff);
            Context.SaveChanges();
        }

        public void Update(Staff staff)
        {
            Context.StaffMembers.Update(staff);
            Context.SaveChanges();
        }
    }
}
