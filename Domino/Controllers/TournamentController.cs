using Domino.Application.Contract;
using Domino.Application.DTOs.TournamentDTOs;
using Microsoft.AspNetCore.Mvc;
using static Domino.Domain.Entities.Enums;

namespace Domino.API.Controllers
{
    [ApiController]
    [Route("Api/Tournament")]
    public class TournamentController: ControllerBase
    {
        private readonly ITournamentService _service;

        public TournamentController(ITournamentService service) 
        { 
            _service = service; 
        }



        [HttpGet("Active")]
        public async Task<IActionResult> GetActive()
        {
            var response = await _service.GetActiveAsync();

            return StatusCode(response.StatusCode, response);
        }



        [HttpGet("Status/{status}")]
        public async Task<IActionResult> GetByStatus(TournamentStatus status)
        {
            var response = await _service.GetAllByStatusAsync(status);
            return StatusCode(response.StatusCode, response);
        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);

            return StatusCode(response.StatusCode, response);
        }



        [HttpGet("{id:int}/Full")]
        public async Task<IActionResult> GetFull(int id)
        {
            var response = await _service.GetFullAsync(id);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPost]
        public async Task<IActionResult> Create( CreateTournamentDTO createTournamentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.CreateAsync(createTournamentDTO);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateTournamentDTO updateTournamentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.UpdateAsync(id, updateTournamentDTO);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPatch("{id:int}/Status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateTournamentStatusDTO updateTournamentStatusDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.UpdateStatusAsync(id, updateTournamentStatusDTO);

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
