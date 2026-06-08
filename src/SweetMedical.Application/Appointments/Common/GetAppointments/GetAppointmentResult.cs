using SweetMedical.Domain.AggregateModels.AppointmentAggregate;

namespace SweetMedical.Application.Appointments.Common.GetAppointments;

public record GetAppointmentResult(
    List<Appointment> Appointments,
    int TotalCount,
    int Page,
    int PageSize
);