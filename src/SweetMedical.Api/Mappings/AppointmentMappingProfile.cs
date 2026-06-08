using AutoMapper;
using SweetMedical.Application.Appointments.Common.GetAppointments;
using SweetMedical.Contracts.Appointments;
using SweetMedical.Contracts.Appointments.CreateAppointment;
using SweetMedical.Domain.AggregateModels.AppointmentAggregate;

namespace SweetMedical.Api.Mappings;

public class AppointmentMappingProfile : Profile
{
    public AppointmentMappingProfile()
    {
        CreateMap<Appointment, AppointmentResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<GetAppointmentResult, GetAppointmentsResponse>()
            .ConstructUsing((src, ctx) => new GetAppointmentsResponse(
                ctx.Mapper.Map<List<AppointmentResponse>>(src.Appointments),
                src.TotalCount,
                src.Page,
                src.PageSize,
                (int)Math.Ceiling(src.TotalCount / (double)src.PageSize)));

        CreateMap<Appointment, CreateAppointmentResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
