using AutoMapper;
using BLL.Models.Requests;
using BLL.Models.Responses;
using DAL.Entities;

namespace BLL.Mappers
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<AppointmentRequest, AppointmentEntity>();

            CreateMap<AppointmentEntity, AppointmentResponse>();

        }
    }
}
