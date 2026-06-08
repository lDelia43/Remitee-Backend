using SweetMedical.Domain.AggregateModels.AggregateDoctor;

namespace SweetMedical.Application.Doctors.Common.GetDoctors;

public record GetDoctorResult(List<Doctor> Doctors);