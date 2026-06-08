using ErrorOr;
using MediatR;
using SweetMedical.Application.Appointments.Common.GetAppointments;

namespace SweetMedical.Application.Appointments.Queries;

public record GetAppointmentQuery(Guid DoctorId, int Page, int PageSize) : IRequest<ErrorOr<GetAppointmentResult>>;