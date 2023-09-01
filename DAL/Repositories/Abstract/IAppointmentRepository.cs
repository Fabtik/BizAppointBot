using DAL.Entities;

namespace DAL.Repositories.Abstract
{
    public interface IAppointmentRepository
    {
        public Task<AppointmentEntity> GetByIdAsync(int id);
        public Task<AppointmentEntity> GetByAppointedTimeAsync(DateTime time);
        public Task<List<AppointmentEntity>> GetAllAppointmentByTimeAsync(DateTime time);

        public Task<List<AppointmentEntity>> GetAppointmentsForDayAsync(DateTime day);
        public Task<bool> TryToConfirmAppointment(int id);
        public Task<bool> isExceedingLimit(DateTime time);
        public Task<List<AppointmentEntity>> GetAllAsync();
        public Task<int> CreateAsync(AppointmentEntity appointment);
        public Task UpdateAsync(int id, AppointmentEntity appointment);
        public Task<bool> TryToDeleteAsync(int id);
    }
}
