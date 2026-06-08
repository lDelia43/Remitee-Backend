using SweetMedical.Domain.AggregateModels.AppointmentAggregate.Enums;

namespace SweetMedical.Tests.SweetMedical.Domain.Appointment;

public class AppointmentTests
{
    private static global::SweetMedical.Domain.AggregateModels.AppointmentAggregate.Appointment CreateAppointment(DateTime scheduledAt) =>
        new(Guid.NewGuid(), Guid.NewGuid(), "John Doe", scheduledAt);

    [Fact]
    public void Constructor_Should_SetStatusToActive()
    {
        var appointment = CreateAppointment(DateTime.UtcNow.AddDays(1));

        Assert.Equal(AppointmentStatus.Active, appointment.Status);
    }

    [Fact]
    public void Cancel_Should_SetStatusToCancelled_WhenAppointmentIsActiveAndFuture()
    {
        var appointment = CreateAppointment(DateTime.UtcNow.AddDays(1));

        appointment.Cancel();

        Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
    }

    [Fact]
    public void Cancel_Should_Throw_WhenAppointmentAlreadyCancelled()
    {
        var appointment = CreateAppointment(DateTime.UtcNow.AddDays(1));
        appointment.Cancel();

        var exception = Assert.Throws<InvalidOperationException>(() => appointment.Cancel());
        Assert.Contains("already cancelled", exception.Message);
    }

    [Fact]
    public void Cancel_Should_Throw_WhenAppointmentIsInThePast()
    {
        var appointment = CreateAppointment(DateTime.UtcNow.AddDays(-1));

        var exception = Assert.Throws<InvalidOperationException>(() => appointment.Cancel());
        Assert.Contains("already passed", exception.Message);
    }
}
