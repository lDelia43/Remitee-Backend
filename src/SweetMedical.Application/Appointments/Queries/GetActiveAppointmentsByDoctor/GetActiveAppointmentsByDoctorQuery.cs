using ErrorOr;
using MediatR;
using SweetMedical.Application.Appointments.Common.GetActiveAppointmentsByDoctor;

namespace SweetMedical.Application.Appointments.Queries.GetActiveAppointmentsByDoctor;

public record GetActiveAppointmentsByDoctorQuery(Guid DoctorId) : IRequest<ErrorOr<GetActiveAppointmentsByDoctorResult>>;
