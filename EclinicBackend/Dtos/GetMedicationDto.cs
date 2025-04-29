using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EclinicBackend.Dtos
{
    public class GetMedicationDto
    {

        public int MedicationId { get; set; }


        public string Name { get; set; } = "";

        public string? Dosage { get; set; }


        public string? Route { get; set; } // Oral, IV, etc.


        public string? Frequency { get; set; }

        public string? SideEffects { get; set; }


    }

}