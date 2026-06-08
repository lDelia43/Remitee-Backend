using Microsoft.EntityFrameworkCore;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.AggregateModels.AggregateDoctor;

namespace SweetMedical.Infrastructure.Persistence.Queries;

public class DoctorQueries : IDoctorQueries
{
    private readonly ApplicationDbContext _context;

    public DoctorQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Doctor>> GetAll()
    {
        return await _context.Doctors.AsNoTracking().ToListAsync();
    }
}
