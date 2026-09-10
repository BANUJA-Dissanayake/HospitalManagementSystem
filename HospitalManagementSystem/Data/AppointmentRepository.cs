using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();
        Appointment GetById(int id);
        Patient FindPatient(int id);
        Doctor FindDoctor(int id);
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(Appointment appointment);
    }

    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HospitalContext context) : base(context) { }

        public override List<Appointment> GetAll() =>
            Context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderBy(a => a.ScheduledAt)
                .ToList();

        public Appointment GetById(int id) =>
            Context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefault(a => a.Id == id);

        public Patient FindPatient(int id) => Context.Patients.Find(id);

        public Doctor FindDoctor(int id) => Context.Doctors.Find(id);

        // Real availability check: does this doctor already have a live
        // appointment at the exact same time? Replaces the old, broken
        // "pre-registered available slot" model.
        private bool HasConflict(Appointment appointment)
        {
            return Context.Appointments.Any(a =>
                a.Id != appointment.Id &&
                a.DoctorId == appointment.DoctorId &&
                a.ScheduledAt == appointment.ScheduledAt &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public void Add(Appointment appointment)
        {
            if (HasConflict(appointment))
                throw new DoctorUnavailableException(
                    $"{appointment.Doctor?.Name ?? "This doctor"} already has an appointment at {appointment.ScheduledAt}.");

            Context.Appointments.Add(appointment);
            Context.SaveChanges();
        }

        public void Update(Appointment appointment)
        {
            if (HasConflict(appointment))
                throw new DoctorUnavailableException(
                    $"{appointment.Doctor?.Name ?? "This doctor"} already has an appointment at {appointment.ScheduledAt}.");

            Context.Appointments.Update(appointment);
            Context.SaveChanges();
        }
    }
}
