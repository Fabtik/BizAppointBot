using BLL.Models.Requests;
using BLL.Models.Responses;
using DAL.Entities;

namespace BLL.Services.Abstract
{
    public interface IAppointmentService
    {
        public Task<AppointmentResponse> GetByIdAsync(int id);
        public Task<List<AppointmentResponse>> GetAllAsync();
        public Task<int> CreateAsync(AppointmentRequest appointment);
        public Task UpdateAsync(int id, AppointmentRequest appointment);
        public Task DeleteAsync(int id);
    }
}
