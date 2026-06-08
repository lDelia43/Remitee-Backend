using Microsoft.EntityFrameworkCore;
using SweetMedical.Application.Common.Interfaces.Persistence;
using SweetMedical.Domain.Doctor;

namespace SweetMedical.Infrastructure.Persistence.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ApplicationDbContext _context;

    public DoctorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Doctor>> GetDoctors()
    {
        return await _context.Doctors.ToListAsync();
    }
}
