using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SweetMedical.Domain.AggregateModels.AggregateDoctor;
using SweetMedical.Domain.AggregateModels.AppointmentAggregate;
using SweetMedical.Domain.AggregateModels.AppointmentAggregate.Enums;

namespace SweetMedical.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.PatientName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ScheduledAt)
            .IsRequired();

        builder.Property(a => a.DoctorId)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                s => s.ToString(),
                s => Enum.Parse<AppointmentStatus>(s));

        builder.HasOne<Doctor>()
            .WithMany()
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
