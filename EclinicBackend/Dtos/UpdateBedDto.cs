using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EclinicBackend.Dtos
{
    public class UpdateBedDto
    {

        public int BedId { get; set; }
        public int? InpatientId { get; set; }

    }
}