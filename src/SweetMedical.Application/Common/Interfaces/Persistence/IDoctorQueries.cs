using SweetMedical.Domain.AggregateModels.AggregateDoctor;

namespace SweetMedical.Application.Common.Interfaces.Persistence;

public interface IDoctorQueries
{
    Task<List<Doctor>> GetAll();
}
