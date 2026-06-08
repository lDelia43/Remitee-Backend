namespace SweetMedical.Contracts.Appointments.CreateAppointment;

public record CreateAppointmentRequest(
    Guid DoctorId,
    string PatientName,
    DateTime ScheduledAt);
