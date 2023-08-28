using BLL.Models.Requests;
using BLL.Models.Responses;
using DAL.Entities;

namespace BLL.Services.Abstract
{
    public interface IAppointmentService
    {
        public Task<AppointmentResponse> GetByIdAsync(int id);
        public Task<List<AppointmentResponse>> GetAllAsync();
        public Task<List<AppointmentResponse>> GetAppointmentsForDayAsync(DateTime day);
        public Task<int> CreateAsync(AppointmentRequest appointment);
        public Task<bool> TryToConfirmAppointment(int id);
        public Task UpdateAsync(int id, AppointmentRequest appointment);
        public Task<bool> TryToDeleteAsync(int id);
    }
}
