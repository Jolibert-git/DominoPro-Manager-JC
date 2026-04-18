using Domino.Application.DTOs.TableDTOs;

namespace Domino.Application.DTOs.RoundDTOs
{
    public class RoundDetailDTO: RoundDTO
    {
        public List<TableDTO> Tables { get; set; } = [];
    }
}
