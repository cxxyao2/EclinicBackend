using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EclinicBackend.Dtos
{
    public class UpdatePractitionerScheduleDto
    {

        public int ScheduleId { get; set; }
        public int PractitionerId { get; set; }

        public int? PatientId { get; set; }
        public string ReasonForVisit { get; set; } = "";

    }
}