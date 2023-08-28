﻿using AutoMapper;
using BLL.Models.Requests;
using BLL.Models.Responses;
using BLL.Services.Abstract;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.Extensions.Configuration;

namespace BLL.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IMapper _mapper;     

        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IMapper mapper, IAppointmentRepository appointmentRepository)
        {
            _mapper = mapper;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<bool> TryToConfirmAppointment(int id)
        {
            return await _appointmentRepository.TryToConfirmAppointment(id);
        }

        public async Task<int> CreateAsync(AppointmentRequest appointment)
        {
            var newAppointment = _mapper.Map<AppointmentEntity>(appointment);

            var newAppointmentTime = newAppointment.AppointedTime;

            var existingAppointment = await _appointmentRepository.GetByAppointedTimeAsync(newAppointmentTime);

            if (existingAppointment != null && existingAppointment.Confirmed == true) 
            {
                throw new Exception($"Appintment on time {newAppointmentTime} already exist");
            }

            return await _appointmentRepository.CreateAsync(newAppointment);           
        }


        public async Task<bool> TryToDeleteAsync(int id)
        {
            return await _appointmentRepository.TryToDeleteAsync(id);
        }

        public async Task<List<AppointmentResponse>> GetAllAsync()
        {
            return _mapper.Map<List<AppointmentResponse>>
                (await _appointmentRepository.GetAllAsync());
        }

        public async Task<AppointmentResponse> GetByIdAsync(int id)
        {
            return _mapper.Map<AppointmentResponse>
                (await _appointmentRepository.GetByIdAsync(id));
        }

        public async Task<List<AppointmentResponse>> GetAppointmentsForDayAsync(DateTime day)
        {
            return _mapper.Map<List<AppointmentResponse>>
                (await _appointmentRepository.GetAppointmentsForDayAsync(day));
        }


        public async Task UpdateAsync(int id, AppointmentRequest appointment)
        {
            await _appointmentRepository.UpdateAsync
                (id, _mapper.Map<AppointmentEntity>(appointment));
        }
    }
}
