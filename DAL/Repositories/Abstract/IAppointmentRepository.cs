using DAL.Entities;

namespace DAL.Repositories.Abstract
{
    public interface IAppointmentRepository
    {
        public Task<AppointmentEntity> GetByIdAsync(int id);
        public Task<List<AppointmentEntity>> GetAllAsync();
        public Task<int> CreateAsync(AppointmentEntity appointment);
        public Task UpdateAsync(int id, AppointmentEntity appointment);
        public Task DeleteAsync(int id);
    }
}
