using ErrorOr;
using MediatR;
using SweetMedical.Application.Appointments.Common.CreateAppointment;

namespace SweetMedical.Application.Appointments.Commands.CreateAppointment;

public record CreateAppointmentCommand(
    Guid DoctorId,
    string PatientName,
    DateTime ScheduledAt
) : IRequest<ErrorOr<CreateAppoinmentResult>>;
