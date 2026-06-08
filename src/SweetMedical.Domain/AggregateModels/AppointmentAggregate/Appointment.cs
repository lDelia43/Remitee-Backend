using SweetMedical.Domain.AggregateModels.AppointmentAggregate.Enums;
using SweetMedical.Domain.Models;

namespace SweetMedical.Domain.AggregateModels.AppointmentAggregate;

public sealed class Appointment : AggregateRoot<Guid>
{
    public Guid DoctorId { get; private set; }
    public string PatientName { get; private set; } = null!;
    public DateTime ScheduledAt { get; private set; }
    public AppointmentStatus Status { get; private set; }

    public Appointment(Guid id, Guid doctorId, string patientName, DateTime scheduledAt)
        : base(id)
    {
        DoctorId = doctorId;
        PatientName = patientName;
        ScheduledAt = scheduledAt;
        Status = AppointmentStatus.Active;
    }

    private Appointment() { }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Cancelled)
            throw new InvalidOperationException("The appointment is already cancelled.");

        if (ScheduledAt < DateTime.UtcNow)
            throw new InvalidOperationException("Cannot cancel an appointment that has already passed.");

        Status = AppointmentStatus.Cancelled;
    }
}
