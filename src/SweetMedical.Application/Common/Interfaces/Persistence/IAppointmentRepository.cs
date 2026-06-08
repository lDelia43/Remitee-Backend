using SweetMedical.Domain.Appointment;

namespace SweetMedical.Application.Common.Interfaces.Persistence;

public interface IAppointmentRepository
{
    Task<(List<Appointment> Items, int TotalCount)> GetAppointmentsByDoctor(Guid doctorId, int page, int pageSize);
    Task<List<Appointment>> GetActiveAppointmentsByDoctor(Guid doctorId);
    Task<Appointment?> GetById(Guid id);
    Task Add(Appointment appointment);
    Task Update(Appointment appointment);
}
