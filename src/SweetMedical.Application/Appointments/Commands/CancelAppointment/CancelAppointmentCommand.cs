using ErrorOr;
using MediatR;

namespace SweetMedical.Application.Appointments.Commands.CancelAppointment;

public record CancelAppointmentCommand(Guid AppointmentId) : IRequest<ErrorOr<Deleted>>;
