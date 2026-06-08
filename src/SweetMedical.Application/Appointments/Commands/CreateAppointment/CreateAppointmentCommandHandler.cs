using ErrorOr;
using MediatR;
using SweetMedical.Application.Appointments.Common.CreateAppointment;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.Appointment;
using SweetMedical.Domain.Appointment.Enums;

namespace SweetMedical.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, ErrorOr<CreateAppoinmentResult>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<ErrorOr<CreateAppoinmentResult>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (request.ScheduledAt < DateTime.UtcNow)
            return Error.Validation("Appointment.ScheduledAt", "Cannot create an appointment in the past.");

        var existingAppointments = await _appointmentRepository.GetActiveAppointmentsByDoctor(request.DoctorId);

        var hasConflict = existingAppointments.Any(a =>
            a.Status == AppointmentStatus.Active &&
            a.ScheduledAt == request.ScheduledAt);

        if (hasConflict)
            return Error.Conflict("Appointment.Conflict", "The doctor already has an active appointment at that time.");

        var appointment = new Appointment(
            Guid.NewGuid(),
            request.DoctorId,
            request.PatientName,
            request.ScheduledAt);

        await _appointmentRepository.Add(appointment);

        return new CreateAppoinmentResult(appointment);
    }
}
