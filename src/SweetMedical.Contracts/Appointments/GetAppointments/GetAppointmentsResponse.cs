namespace SweetMedical.Contracts.Appointments;

public record GetAppointmentsResponse(
    List<AppointmentResponse> Appointments,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

public record AppointmentResponse(
    Guid Id,
    Guid DoctorId,
    string PatientName,
    DateTime ScheduledAt,
    string Status
);
