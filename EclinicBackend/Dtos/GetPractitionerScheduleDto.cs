using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EclinicBackend.Dtos
{
    public class GetPractitionerScheduleDto
    {

        public int ScheduleId { get; set; }
        public int PractitionerId { get; set; }
        public string PractitionerName { get; set; } = "";
        public int? PatientId { get; set; }
        public string PatientName { get; set; } = "";
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string ReasonForVisit { get; set; } = "";

    }
}