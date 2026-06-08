using SweetMedical.Domain.Entities;

namespace SweetMedical.Domain.Doctor;

public sealed class Doctor : Person<Guid>
{
    public string Specialty { get; private set; } = null!;

    public Doctor(Guid id, string name, string specialty)
        : base(id, name)
    {
        Specialty = specialty;
    }

    private Doctor() { }
}
