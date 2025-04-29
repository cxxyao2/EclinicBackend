using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EclinicBackend.Dtos
{
    public class GetBedDto
    {
        public int BedId { get; set; }
        public string RoomNumber { get; set; } = string.Empty; // Room number
        public string BedNumber { get; set; } = string.Empty; // Bed number

        public int? InpatientId { get; set; }

        public int PatientId { get; set; }
        public string? PatientName { get; set; }

        public int PractitionerId { get; set; }
        public string? PractitionerName { get; set; }

        public int? NurseId { get; set; }
        public string? NurseName { get; set; }

    }
}