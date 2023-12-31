﻿using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<int> CreateAsync(AppointmentEntity appointment)
        {
            if(await isExceedingLimit(appointment.AppointedTime))
            {
                appointment.Confirmed = false; // double check

                var newUser = await _context.Appointments.AddAsync(appointment);

                await _context.SaveChangesAsync();

                return newUser.Entity.Id;
            }


            throw new Exception($"Failed to create an appointment");
        }

        public async Task<bool> TryToDeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return false; // Appointment doesn't exist
            }

            _context.Appointments.Remove(appointment);

            await _context.SaveChangesAsync();

            return true; // Deletion was successful
        }

        public async Task<bool> TryToConfirmAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                throw new Exception($"Cannot find appointment with id = {id}");
            }

            var appointmentTime = appointment.AppointedTime;

            var existingAppointments = await GetAllAppointmentByTimeAsync(appointmentTime);

            int confirmedAppointmentId = -1;// .-.

            foreach (var existingAppointment in existingAppointments)
            {
                if (existingAppointment.Confirmed)
                {
                    confirmedAppointmentId = existingAppointment.Id;
                    break;
                }
            }

            if (confirmedAppointmentId == -1)
            {
                appointment.Confirmed = true;

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {

                await _context.SaveChangesAsync();
                return false;
            }
        }
        public async Task<List<AppointmentEntity>> GetAllAppointmentByTimeAsync(DateTime time)
        {
            var appointments = await _context.Appointments.
                Where(a => a.AppointedTime == time).ToListAsync();

            return appointments;
        }

        public async Task<List<AppointmentEntity>> GetAllAsync()
        {
            var appointments = await _context.Appointments.ToListAsync();

            return appointments;
        }

        public async Task<AppointmentEntity> GetByAppointedTimeAsync(DateTime time)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointedTime == time);

            return appointment;
        }

        public async Task<List<AppointmentEntity>> GetAppointmentsForDayAsync(DateTime day)
        {
            DateTime startOfDay = day.Date;
            DateTime endOfDay = day.Date.AddDays(1).AddTicks(-1);

            return await _context.Appointments
                .Where(a => a.AppointedTime >= startOfDay && a.AppointedTime <= endOfDay)
                .ToListAsync();
        }

        public async Task<AppointmentEntity> GetByIdAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {

                throw new Exception(message: $"Appointment with id = {id} doesn't exist");

            }

            return appointment;
        }      

        public async Task UpdateAsync(int id, AppointmentEntity appointment)
        {
            var existingAppointment = await _context.Appointments.FindAsync(id);

            if (existingAppointment == null)
            {
                throw new Exception(message: $"Appointment with id = {id} doesn't exist");
            }

            ReflectionHelper.CopyObjectPropertiesWithoutSpecified(existingAppointment, appointment, "ID", "CreatedDate");

            await _context.SaveChangesAsync();
        }

        public async Task<bool> isExceedingLimit(DateTime time)
        {           
            var existingAppointments = await GetAllAppointmentByTimeAsync(time);

            if (existingAppointments.Count > 10)//Magic numbers, take it out into configuration.
            {
                return false;
            }

            return true;
        }
    }
}
