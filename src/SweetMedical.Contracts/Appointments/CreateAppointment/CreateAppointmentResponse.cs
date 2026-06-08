namespace SweetMedical.Contracts.Appointments.CreateAppointment;

public record CreateAppointmentResponse(
    Guid Id,
    Guid DoctorId,
    string PatientName,
    DateTime ScheduledAt,
    string Status);
