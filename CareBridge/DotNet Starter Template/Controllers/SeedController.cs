using Microsoft.AspNetCore.Mvc;
using CareBridge.Data;
using CareBridge.Models.Entities;

namespace CareBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            if (_context.Dentists.Any() || _context.Patients.Any())
            {
                return BadRequest("Database already contains data. Clear it first if you want to re-seed.");
            }

            var dentists = new List<Dentist>
            {
                new Dentist { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Specialization = "Orthodontist", Phone = "555-0101", LicenseNumber = "L-1001", CreatedAt = DateTime.UtcNow },
                new Dentist { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Specialization = "Pediatric Dentist", Phone = "555-0102", LicenseNumber = "L-1002", CreatedAt = DateTime.UtcNow },
                new Dentist { FirstName = "Emily", LastName = "Davis", Email = "emily.davis@example.com", Specialization = "Oral Surgeon", Phone = "555-0103", LicenseNumber = "L-1003", CreatedAt = DateTime.UtcNow },
                new Dentist { FirstName = "Michael", LastName = "Wilson", Email = "michael.wilson@example.com", Specialization = "General Dentist", Phone = "555-0104", LicenseNumber = "L-1004", CreatedAt = DateTime.UtcNow },
                new Dentist { FirstName = "Sarah", LastName = "Brown", Email = "sarah.brown@example.com", Specialization = "Periodontist", Phone = "555-0105", LicenseNumber = "L-1005", CreatedAt = DateTime.UtcNow }
            };

            await _context.Dentists.AddRangeAsync(dentists);
            await _context.SaveChangesAsync();

            var patients = new List<Patient>
            {
                new Patient { FirstName = "Alice", LastName = "Johnson", Email = "alice.j@example.com", Phone = "555-1001", DateOfBirth = new DateTime(1985, 5, 20), Address = "123 Maple St", MedicalHistory = "None", CreatedAt = DateTime.UtcNow },
                new Patient { FirstName = "Bob", LastName = "Williams", Email = "bob.w@example.com", Phone = "555-1002", DateOfBirth = new DateTime(1990, 8, 15), Address = "456 Oak Ave", MedicalHistory = "Allergic to penicillin", CreatedAt = DateTime.UtcNow },
                new Patient { FirstName = "Charlie", LastName = "Jones", Email = "charlie.j@example.com", Phone = "555-1003", DateOfBirth = new DateTime(1978, 12, 1), Address = "789 Pine Rd", MedicalHistory = "High blood pressure", CreatedAt = DateTime.UtcNow },
                new Patient { FirstName = "Diana", LastName = "Garcia", Email = "diana.g@example.com", Phone = "555-1004", DateOfBirth = new DateTime(2000, 3, 10), Address = "321 Elm St", MedicalHistory = "None", CreatedAt = DateTime.UtcNow },
                new Patient { FirstName = "Evan", LastName = "Martinez", Email = "evan.m@example.com", Phone = "555-1005", DateOfBirth = new DateTime(1995, 7, 22), Address = "654 Cedar Ln", MedicalHistory = "Asthma", CreatedAt = DateTime.UtcNow }
            };

            await _context.Patients.AddRangeAsync(patients);
            await _context.SaveChangesAsync();

            var appointments = new List<Appointment>();
            var rnd = new Random();
            var procedureTypes = new[] { "Checkup", "Cleaning", "Filling", "Root Canal", "Extraction" };

            foreach (var patient in patients)
            {
                var dentist = dentists[rnd.Next(dentists.Count)];
                var estimatedCost = (decimal)rnd.Next(50, 500);
                appointments.Add(new Appointment
                {
                    PatientId = patient.Id,
                    DentistId = dentist.Id,
                    AppointmentDate = DateTime.UtcNow.AddDays(rnd.Next(1, 30)),
                    ProcedureType = procedureTypes[rnd.Next(procedureTypes.Length)],
                    Status = "Scheduled",
                    TotalAmount = estimatedCost,
                    EstimatedCost = estimatedCost,
                    Notes = "Routine visit",
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.Appointments.AddRangeAsync(appointments);
            await _context.SaveChangesAsync();

            return Ok("Database seeded successfully with Dentists, Patients, and Appointments.");
        }
    }
}
