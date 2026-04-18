using Domino.Application.DTOs.ResultDTOs;

namespace Domino.Application.DTOs.TableDTOs
{
    public class TableDetailDTO: TableDTO
    {
        public IReadOnlyList<ResultDTO> Results { get; set; } = [];
        public ResultDTO? Winner { get; set; }
    }
}
