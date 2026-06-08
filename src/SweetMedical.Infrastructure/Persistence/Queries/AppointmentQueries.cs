using Microsoft.EntityFrameworkCore;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.AggregateModels.AppointmentAggregate;
using SweetMedical.Domain.AggregateModels.AppointmentAggregate.Enums;

namespace SweetMedical.Infrastructure.Persistence.Queries;

public class AppointmentQueries : IAppointmentQueries
{
    private readonly ApplicationDbContext _context;

    public AppointmentQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Appointment> Items, int TotalCount)> GetByDoctor(Guid doctorId, int page, int pageSize)
    {
        var query = _context.Appointments.Where(a => a.DoctorId == doctorId);
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<Appointment>> GetActiveByDoctor(Guid doctorId)
    {
        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Active)
            .AsNoTracking()
            .ToListAsync();
    }
}
