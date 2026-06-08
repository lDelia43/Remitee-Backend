using SweetMedical.Domain.Doctor;

namespace SweetMedical.Application.Common.Interfaces.Persistence;

public interface IDoctorRepository
{
    Task<List<Doctor>> GetDoctors();
}