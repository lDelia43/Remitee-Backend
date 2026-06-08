using ErrorOr;
using Moq;
using SweetMedical.Application.Appointments.Commands.CreateAppointment;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.Appointment;

namespace SweetMedical.Tests.SweetMedical.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentRepository> _repositoryMock = new();
    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTests()
    {
        _handler = new CreateAppointmentCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_WhenScheduledInThePast()
    {
        var command = new CreateAppointmentCommand(Guid.NewGuid(), "John Doe", DateTime.UtcNow.AddDays(-1));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        _repositoryMock.Verify(r => r.Add(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnConflict_WhenDoctorHasActiveAppointmentAtSameTime()
    {
        var doctorId = Guid.NewGuid();
        var scheduledAt = DateTime.UtcNow.AddDays(1);
        var existing = new Appointment(Guid.NewGuid(), doctorId, "Existing Patient", scheduledAt);

        _repositoryMock
            .Setup(r => r.GetActiveAppointmentsByDoctor(doctorId))
            .ReturnsAsync([existing]);

        var command = new CreateAppointmentCommand(doctorId, "John Doe", scheduledAt);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Conflict, result.FirstError.Type);
        _repositoryMock.Verify(r => r.Add(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_CreateAndPersistAppointment_WhenRequestIsValid()
    {
        var doctorId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetActiveAppointmentsByDoctor(doctorId))
            .ReturnsAsync([]);

        var command = new CreateAppointmentCommand(doctorId, "John Doe", DateTime.UtcNow.AddDays(1));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal("John Doe", result.Value.Appointment.PatientName);
        _repositoryMock.Verify(r => r.Add(It.Is<Appointment>(a => a.DoctorId == doctorId)), Times.Once);
    }
}
