using SweetMedical.Domain.AggregateModels.AppointmentAggregate;

namespace SweetMedical.Application.Common.Interfaces.Persistence;

public interface IAppointmentRepository
{
    Task<Appointment?> GetById(Guid id);
    Task Add(Appointment appointment);
    Task Update(Appointment appointment);
}
