using ErrorOr;
using MediatR;
using SweetMedical.Application.Appointments.Common.GetActiveAppointmentsByDoctor;
using SweetMedical.Application.Common.Interfaces.Persistence;

namespace SweetMedical.Application.Appointments.Queries.GetActiveAppointmentsByDoctor;

public class GetActiveAppointmentsByDoctorQueryHandler
    : IRequestHandler<GetActiveAppointmentsByDoctorQuery, ErrorOr<GetActiveAppointmentsByDoctorResult>>
{
    private readonly IAppointmentQueries _appointmentQueries;

    public GetActiveAppointmentsByDoctorQueryHandler(IAppointmentQueries appointmentQueries)
    {
        _appointmentQueries = appointmentQueries;
    }

    public async Task<ErrorOr<GetActiveAppointmentsByDoctorResult>> Handle(
        GetActiveAppointmentsByDoctorQuery request,
        CancellationToken cancellationToken)
    {
        var appointments = await _appointmentQueries.GetActiveByDoctor(request.DoctorId);
        return new GetActiveAppointmentsByDoctorResult(appointments);
    }
}
