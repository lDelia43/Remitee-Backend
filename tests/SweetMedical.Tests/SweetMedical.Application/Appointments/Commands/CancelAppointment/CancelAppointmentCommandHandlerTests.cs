using ErrorOr;
using Moq;
using SweetMedical.Application.Appointments.Commands.CancelAppointment;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.AggregateModels.AppointmentAggregate;
using SweetMedical.Domain.AggregateModels.AppointmentAggregate.Enums;

namespace SweetMedical.Tests.SweetMedical.Application.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentRepository> _repositoryMock = new();
    private readonly CancelAppointmentCommandHandler _handler;

    public CancelAppointmentCommandHandlerTests()
    {
        _handler = new CancelAppointmentCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenAppointmentDoesNotExist()
    {
        _repositoryMock
            .Setup(r => r.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Appointment?)null);

        var result = await _handler.Handle(new CancelAppointmentCommand(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.FirstError.Type);
    }

    [Fact]
    public async Task Handle_Should_CancelAppointment_WhenItExistsAndIsActive()
    {
        var appointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), "John Doe", DateTime.UtcNow.AddDays(1));

        _repositoryMock
            .Setup(r => r.GetById(appointment.Id))
            .ReturnsAsync(appointment);

        var result = await _handler.Handle(new CancelAppointmentCommand(appointment.Id), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
        _repositoryMock.Verify(r => r.Update(appointment), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_WhenAppointmentAlreadyCancelled()
    {
        var appointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), "John Doe", DateTime.UtcNow.AddDays(1));
        appointment.Cancel();

        _repositoryMock
            .Setup(r => r.GetById(appointment.Id))
            .ReturnsAsync(appointment);

        var result = await _handler.Handle(new CancelAppointmentCommand(appointment.Id), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        _repositoryMock.Verify(r => r.Update(It.IsAny<Appointment>()), Times.Never);
    }
}
