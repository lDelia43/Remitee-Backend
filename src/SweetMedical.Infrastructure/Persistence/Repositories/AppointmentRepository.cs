using Microsoft.EntityFrameworkCore;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.Appointment;
using SweetMedical.Domain.Appointment.Enums;

namespace SweetMedical.Infrastructure.Persistence.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;

    public AppointmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Appointment> Items, int TotalCount)> GetAppointmentsByDoctor(Guid doctorId, int page, int pageSize)
    {
        var query = _context.Appointments.Where(a => a.DoctorId == doctorId);
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<Appointment>> GetActiveAppointmentsByDoctor(Guid doctorId)
    {
        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Active)
            .ToListAsync();
    }

    public async Task<Appointment?> GetById(Guid id)
    {
        return await _context.Appointments.FindAsync(id);
    }

    public async Task Add(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }
}
