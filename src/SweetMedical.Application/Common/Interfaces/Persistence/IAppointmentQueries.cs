using SweetMedical.Domain.AggregateModels.AppointmentAggregate;

namespace SweetMedical.Application.Common.Interfaces.Persistence;

public interface IAppointmentQueries
{
    Task<(List<Appointment> Items, int TotalCount)> GetByDoctor(Guid doctorId, int page, int pageSize);
    Task<List<Appointment>> GetActiveByDoctor(Guid doctorId);
}
