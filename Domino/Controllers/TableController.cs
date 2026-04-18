using Domino.Application.Contract;
using Domino.Application.DTOs.TableDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Domino.API.Controllers
{
    [ApiController]
    [Route("Api/Tables")]
    public class TableController: ControllerBase
    {
        private readonly ITableService _service;

        public TableController(ITableService service) => _service = service;



        [HttpGet("Round/{roundId:int}")]
        public async Task<IActionResult> GetByRound(int roundId)
        {
            var response = await _service.GetByRoundAsync(roundId);

            return StatusCode(response.StatusCode, response);
        }



        [HttpGet("Round/{roundId:int}/Pending")]
        public async Task<IActionResult> GetPendingByRound(int roundId)
        {
            var response = await _service.GetPendingByRoundAsync(roundId);
            return StatusCode(response.StatusCode, response);
        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);

            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("{id:int}/Results")]
        public async Task<IActionResult> GetWithResults(int id)
        {
            var response = await _service.GetWithResultsAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateTableDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.CreateAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("{id:int}/Status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateTableStatusDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.UpdateStatusAsync(id, request);

            return StatusCode(response.StatusCode, response);
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id )
        {
            var response = await _service.DeleteAsync(id );
            return StatusCode(response.StatusCode, response);
        }
    }
}
