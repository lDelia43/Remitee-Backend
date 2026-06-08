using AutoMapper;
using SweetMedical.Application.Doctors.Common.GetDoctors;
using SweetMedical.Contracts.Doctors;
using SweetMedical.Domain.AggregateModels.AggregateDoctor;

namespace SweetMedical.Api.Mappings;

public class DoctorMappingProfile : Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<Doctor, DoctorResponse>();
        CreateMap<GetDoctorResult, GetDoctorsResponse>()
            .ForMember(dest => dest.Doctors, opt => opt.MapFrom(src => src.Doctors));
    }
}
