using ErrorOr;
using MediatR;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Application.Doctors.Common.GetDoctors;

namespace SweetMedical.Application.Doctors.Queries.GetDoctors;

public class GetDoctorsQueryHandler : IRequestHandler<GetDoctorsQuery, ErrorOr<GetDoctorResult>>
{
    private readonly IDoctorQueries _doctorQueries;

    public GetDoctorsQueryHandler(IDoctorQueries doctorQueries)
    {
        _doctorQueries = doctorQueries;
    }

    public async Task<ErrorOr<GetDoctorResult>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctors = await _doctorQueries.GetAll();
        return new GetDoctorResult(doctors);
    }
}
