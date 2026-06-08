using ErrorOr;
using MediatR;
using SweetMedical.Application.Appointments.Common.CreateAppointment;
using SweetMedical.Application.Appointments.Queries.GetActiveAppointmentsByDoctor;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.AggregateModels.AppointmentAggregate;

namespace SweetMedical.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, ErrorOr<CreateAppoinmentResult>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ISender _sender;

    public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, ISender sender)
    {
        _appointmentRepository = appointmentRepository;
        _sender = sender;
    }

    public async Task<ErrorOr<CreateAppoinmentResult>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (request.ScheduledAt < DateTime.UtcNow)
            return Error.Validation("Appointment.ScheduledAt", "Cannot create an appointment in the past.");

        var activeAppointments = await _sender.Send(
            new GetActiveAppointmentsByDoctorQuery(request.DoctorId), cancellationToken);

        if (activeAppointments.IsError)
            return activeAppointments.Errors;

        var hasConflict = activeAppointments.Value.Appointments.Any(a => a.ScheduledAt == request.ScheduledAt);

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
