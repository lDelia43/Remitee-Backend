using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SweetMedical.Domain.Appointment;
using SweetMedical.Domain.Doctor;

namespace SweetMedical.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        if (context.Database.IsRelational())
            await context.Database.MigrateAsync();
        else
            await context.Database.EnsureCreatedAsync();

        if (await context.Doctors.AnyAsync()) return;

        var doctors = new List<Doctor>
        {
            new(Guid.NewGuid(), "John Smith", "Cardiology"),
            new(Guid.NewGuid(), "Jane Doe", "Neurology"),
            new(Guid.NewGuid(), "Carlos Rivera", "Pediatrics"),
        };

        context.Doctors.AddRange(doctors);

        var now = DateTime.UtcNow;
        var appointments = new List<Appointment>
        {
            new(Guid.NewGuid(), doctors[0].Id, "Alice Johnson",   now.AddDays(1).Date.AddHours(8)),
            new(Guid.NewGuid(), doctors[0].Id, "Bob Williams",    now.AddDays(1).Date.AddHours(9)),
            new(Guid.NewGuid(), doctors[0].Id, "Carol Davis",     now.AddDays(1).Date.AddHours(10)),
            new(Guid.NewGuid(), doctors[0].Id, "David Martinez",  now.AddDays(2).Date.AddHours(8)),
            new(Guid.NewGuid(), doctors[0].Id, "Eva Thompson",    now.AddDays(2).Date.AddHours(9)),
            new(Guid.NewGuid(), doctors[0].Id, "Frank Wilson",    now.AddDays(3).Date.AddHours(8)),
            new(Guid.NewGuid(), doctors[0].Id, "Grace Lee",       now.AddDays(3).Date.AddHours(9)),
            new(Guid.NewGuid(), doctors[0].Id, "Henry Clark",     now.AddDays(4).Date.AddHours(8)),

            new(Guid.NewGuid(), doctors[1].Id, "Isla Walker",     now.AddDays(1).Date.AddHours(10)),
            new(Guid.NewGuid(), doctors[1].Id, "Jack Hall",       now.AddDays(1).Date.AddHours(11)),
            new(Guid.NewGuid(), doctors[1].Id, "Karen Young",     now.AddDays(2).Date.AddHours(10)),
            new(Guid.NewGuid(), doctors[1].Id, "Leo Allen",       now.AddDays(2).Date.AddHours(11)),
            new(Guid.NewGuid(), doctors[1].Id, "Mia Scott",       now.AddDays(3).Date.AddHours(10)),
            new(Guid.NewGuid(), doctors[1].Id, "Noah Adams",      now.AddDays(4).Date.AddHours(10)),

            new(Guid.NewGuid(), doctors[2].Id, "Olivia Baker",    now.AddDays(1).Date.AddHours(14)),
            new(Guid.NewGuid(), doctors[2].Id, "Pablo Gomez",     now.AddDays(1).Date.AddHours(15)),
            new(Guid.NewGuid(), doctors[2].Id, "Quinn Nelson",    now.AddDays(2).Date.AddHours(14)),
            new(Guid.NewGuid(), doctors[2].Id, "Rose Carter",     now.AddDays(3).Date.AddHours(14)),
            new(Guid.NewGuid(), doctors[2].Id, "Sam Mitchell",    now.AddDays(4).Date.AddHours(14)),
        };

        context.Appointments.AddRange(appointments);

        await context.SaveChangesAsync();
    }
}
