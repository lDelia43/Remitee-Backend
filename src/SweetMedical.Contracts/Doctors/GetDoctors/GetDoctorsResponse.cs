namespace SweetMedical.Contracts.Doctors;

public record GetDoctorsResponse(List<DoctorResponse> Doctors);

public record DoctorResponse(
    Guid Id,
    string Name,
    string Specialty
);
