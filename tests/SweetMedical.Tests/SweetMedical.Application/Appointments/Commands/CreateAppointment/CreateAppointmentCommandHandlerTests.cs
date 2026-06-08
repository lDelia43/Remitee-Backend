using ErrorOr;
using MediatR;
using Moq;
using SweetMedical.Application.Appointments.Commands.CreateAppointment;
using SweetMedical.Application.Appointments.Common.GetActiveAppointmentsByDoctor;
using SweetMedical.Application.Appointments.Queries.GetActiveAppointmentsByDoctor;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.AggregateModels.AppointmentAggregate;

namespace SweetMedical.Tests.SweetMedical.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentRepository> _repositoryMock = new();
    private readonly Mock<ISender> _senderMock = new();
    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTests()
    {
        _handler = new CreateAppointmentCommandHandler(_repositoryMock.Object, _senderMock.Object);
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

        _senderMock
            .Setup(s => s.Send(It.Is<GetActiveAppointmentsByDoctorQuery>(q => q.DoctorId == doctorId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetActiveAppointmentsByDoctorResult([existing]));

        var result = await _handler.Handle(new CreateAppointmentCommand(doctorId, "John Doe", scheduledAt), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Conflict, result.FirstError.Type);
        _repositoryMock.Verify(r => r.Add(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_CreateAndPersistAppointment_WhenRequestIsValid()
    {
        var doctorId = Guid.NewGuid();

        _senderMock
            .Setup(s => s.Send(It.Is<GetActiveAppointmentsByDoctorQuery>(q => q.DoctorId == doctorId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetActiveAppointmentsByDoctorResult([]));

        var result = await _handler.Handle(
            new CreateAppointmentCommand(doctorId, "John Doe", DateTime.UtcNow.AddDays(1)), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal("John Doe", result.Value.Appointment.PatientName);
        _repositoryMock.Verify(r => r.Add(It.Is<Appointment>(a => a.DoctorId == doctorId)), Times.Once);
    }
}
