using Moq;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Application.Doctors.Queries.GetDoctors;
using SweetMedical.Domain.Doctor;

namespace SweetMedical.Tests.SweetMedical.Application.Doctors.Queries.GetDoctors;

public class GetDoctorsQueryHandlerTests
{
    private readonly Mock<IDoctorRepository> _repositoryMock = new();
    private readonly GetDoctorsQueryHandler _handler;

    public GetDoctorsQueryHandlerTests()
    {
        _handler = new GetDoctorsQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyList_WhenNoDoctorsExist()
    {
        _repositoryMock
            .Setup(r => r.GetDoctors())
            .ReturnsAsync([]);

        var result = await _handler.Handle(new GetDoctorsQuery(), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Empty(result.Value.Doctors);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllDoctors_WhenDoctorsExist()
    {
        _repositoryMock
            .Setup(r => r.GetDoctors())
            .ReturnsAsync([
                new Doctor(Guid.NewGuid(), "Dr. House", "Diagnostics"),
                new Doctor(Guid.NewGuid(), "Dr. Strange", "Neurosurgery")
            ]);

        var result = await _handler.Handle(new GetDoctorsQuery(), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(2, result.Value.Doctors.Count);
    }

    [Fact]
    public async Task Handle_Should_ReturnDoctorsWithCorrectData()
    {
        var doctorId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetDoctors())
            .ReturnsAsync([new Doctor(doctorId, "Dr. House", "Diagnostics")]);

        var result = await _handler.Handle(new GetDoctorsQuery(), CancellationToken.None);

        var doctor = result.Value.Doctors.Single();
        Assert.Equal(doctorId, doctor.Id);
        Assert.Equal("Dr. House", doctor.Name);
        Assert.Equal("Diagnostics", doctor.Specialty);
    }
}
