using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.AggregateModels.AggregateDoctor;

namespace SweetMedical.Infrastructure.Persistence.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ApplicationDbContext _context;

    public DoctorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();
    }
}
