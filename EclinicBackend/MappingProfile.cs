using AutoMapper;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
namespace EclinicBackend;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Patient, GetPatientDto>().ReverseMap();
        CreateMap<Patient, AddPatientDto>().ReverseMap();

        CreateMap<Practitioner, AddPractitionerDto>().ReverseMap();
        CreateMap<Practitioner, GetPractitionerDto>().ReverseMap();
        CreateMap<PractitionerAvailability, GetPractitionerAvailabilityDto>().ReverseMap(); // deprecated
        CreateMap<PractitionerAvailability, AddPractitionerAvailabilityDto>().ReverseMap(); // depreacted

        CreateMap<Appointment, GetAppointmentDto>().ReverseMap();  // deprecated
        CreateMap<Appointment, AddAppointmentDto>().ReverseMap();  // deprecated
        CreateMap<Medication, GetMedicationDto>().ReverseMap();
        CreateMap<Inpatient, AddInpatientDto>().ReverseMap();
        CreateMap<Inpatient, GetInpatientDto>().ReverseMap();
        CreateMap<PractitionerSchedule, GetPractitionerScheduleDto>().ReverseMap();
        CreateMap<PractitionerSchedule, AddPractitionerScheduleDto>().ReverseMap();

        CreateMap<PrescriptionDto, GetPrescriptionDto>().ReverseMap();
        CreateMap<PrescriptionDto, AddPrescriptionDto>().ReverseMap();

        CreateMap<VisitRecord, GetVisitRecordDto>().ReverseMap();
        CreateMap<VisitRecord, AddVisitRecordDto>().ReverseMap();
        CreateMap<Bed, UpdateBedDto>().ReverseMap();
        CreateMap<Bed, GetBedDto>().ReverseMap();

    }
}