using SweetMedical.Domain.AggregateModels.AggregateDoctor;

namespace SweetMedical.Application.Common.Interfaces.Persistence;

public interface IDoctorRepository
{
    Task Add(Doctor doctor);
}