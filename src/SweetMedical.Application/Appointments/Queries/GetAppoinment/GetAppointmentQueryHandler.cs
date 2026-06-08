using ErrorOr;
using MediatR;
using SweetMedical.Application.Appointments.Common.GetAppointments;
using SweetMedical.Application.Common.Interfaces.Persistence;

namespace SweetMedical.Application.Appointments.Queries;

public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, ErrorOr<GetAppointmentResult>>
{
    
    private readonly IAppointmentRepository _appointmentRepository;

    public GetAppointmentQueryHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<ErrorOr<GetAppointmentResult>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
    {
        var (appointments, totalCount) = await _appointmentRepository.GetAppointmentsByDoctor(
            request.DoctorId, request.Page, request.PageSize);

        return new GetAppointmentResult(appointments, totalCount, request.Page, request.PageSize);
    }
}