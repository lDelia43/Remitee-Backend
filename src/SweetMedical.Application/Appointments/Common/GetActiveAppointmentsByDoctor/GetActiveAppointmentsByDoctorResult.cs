using SweetMedical.Domain.AggregateModels.AppointmentAggregate;

namespace SweetMedical.Application.Appointments.Common.GetActiveAppointmentsByDoctor;

public record GetActiveAppointmentsByDoctorResult(List<Appointment> Appointments);
