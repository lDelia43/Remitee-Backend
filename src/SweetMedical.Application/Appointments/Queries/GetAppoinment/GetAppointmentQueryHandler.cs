using ErrorOr;
using MediatR;
using SweetMedical.Application.Appointments.Common.GetAppointments;
using SweetMedical.Application.Common.Interfaces.Persistence;

namespace SweetMedical.Application.Appointments.Queries;

public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, ErrorOr<GetAppointmentResult>>
{
    private readonly IAppointmentQueries _appointmentQueries;

    public GetAppointmentQueryHandler(IAppointmentQueries appointmentQueries)
    {
        _appointmentQueries = appointmentQueries;
    }

    public async Task<ErrorOr<GetAppointmentResult>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
    {
        var (appointments, totalCount) = await _appointmentQueries.GetByDoctor(
            request.DoctorId, request.Page, request.PageSize);

        return new GetAppointmentResult(appointments, totalCount, request.Page, request.PageSize);
    }
}
