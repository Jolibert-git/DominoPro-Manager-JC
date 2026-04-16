using Domino.Application.DTOs.ResultDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.TableDTOs
{
    public class TableDetailDTO: TableDTO
    {
        public IReadOnlyList<ResultDTO> Results { get; set; } = [];
        public ResultDTO? Winner { get; set; }
    }
}
