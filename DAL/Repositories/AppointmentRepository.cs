using DAL.Context;
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
            var newUser = await _context.Appointments.AddAsync(appointment);

            await _context.SaveChangesAsync();

            return newUser.Entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {

                throw new Exception(message: $"Appointment with id = {id} doesn't exist");
                
            }

            _context.Appointments.Remove(appointment);

            await _context.SaveChangesAsync();

        }

        public async Task<List<AppointmentEntity>> GetAllAsync()
        {
            var appointments = await _context.Appointments.ToListAsync();

            return appointments;
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

            ReflectionHelper.CopyObjectPropertiesWithoutID(existingAppointment, appointment);

            await _context.SaveChangesAsync();
        }
    }
}
