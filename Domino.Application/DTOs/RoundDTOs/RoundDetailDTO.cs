using Domino.Application.DTOs.TableDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.RoundDTOs
{
    public class RoundDetailDTO: RoundDTO
    {
        public List<TableDTO> Tables { get; set; } = [];
    }
}
