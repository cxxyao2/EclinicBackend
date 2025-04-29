using System.Collections.Concurrent;
using EclinicBackend.Enums;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public virtual DbSet<Patient> Patients { get; set; }
    public virtual DbSet<Practitioner> Practitioners { get; set; }
    public virtual DbSet<Appointment> Appointments { get; set; }
    public virtual DbSet<Medication> Medications { get; set; }
    public virtual DbSet<Prescription> Prescriptions { get; set; }
    public virtual DbSet<VisitRecord> VisitRecords { get; set; }
    public virtual DbSet<LabTest> LabTests { get; set; }
    public virtual DbSet<Billing> Billings { get; set; }
    public virtual DbSet<ImageRecord> ImageRecords { get; set; }
    public virtual DbSet<Inpatient> Inpatients { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<PractitionerAvailability> PractitionerAvailabilities { get; set; }
    public virtual DbSet<PractitionerSchedule> PractitionerSchedules { get; set; }
    public virtual DbSet<Bed> Beds { get; set; }
    public virtual DbSet<UserLogHistory> UserLogHistories { get; set; }

    public virtual DbSet<ChatRoom> ChatRooms { get; set; }
    public virtual DbSet<ChatMessage> ChatMessages { get; set; }
    public virtual DbSet<ChatRoomParticipant> ChatRoomParticipants { get; set; }



}

