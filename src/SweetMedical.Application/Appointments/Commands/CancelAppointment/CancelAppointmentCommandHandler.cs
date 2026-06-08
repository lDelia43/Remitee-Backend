using ErrorOr;
using MediatR;
using SweetMedical.Application.Common.Interfaces.Persistence;

namespace SweetMedical.Application.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, ErrorOr<Deleted>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetById(request.AppointmentId);

        if (appointment is null)
            return Error.NotFound("Appointment.NotFound", "Appointment not found.");

        try
        {
            appointment.Cancel();
        }
        catch (InvalidOperationException ex)
        {
            return Error.Validation("Appointment.Cancel", ex.Message);
        }

        await _appointmentRepository.Update(appointment);

        return Result.Deleted;
    }
}
