using ErrorOr;
using MediatR;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Application.Doctors.Common.GetDoctors;

namespace SweetMedical.Application.Doctors.Queries.GetDoctors;

public class GetDoctorsQueryHandler : IRequestHandler<GetDoctorsQuery, ErrorOr<GetDoctorResult>>
{
    
    private readonly IDoctorRepository _doctorRepository;

    public GetDoctorsQueryHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<ErrorOr<GetDoctorResult>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctors = await _doctorRepository.GetDoctors();
        return new GetDoctorResult(doctors);
    }
}