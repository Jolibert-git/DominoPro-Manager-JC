using Domino.Application.Contract;
using Domino.Application.DTOs.PlayerDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Domino.API.Controllers
{
    [ApiController]
    [Route("Api/Players")]
    public class PlayerController: ControllerBase
    {
        private readonly IPlayerService _service;

        public PlayerController(IPlayerService service) => _service = service;

       
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();

            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("Active")]
        public async Task<IActionResult> GetActive()
        {
            var response = await _service.GetActiveAsync();
            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("Elo")]
        public async Task<IActionResult> GetByEloRange([FromQuery] short min,[FromQuery] short max)
        {
            var response = await _service.GetByEloRangeAsync(min, max);
            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id )
        {
            var response = await _service.GetByIdAsync(id );
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpGet("Email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var response = await _service.GetByEmailAsync(email );
            return StatusCode(response.StatusCode, response);
        }

       
        [HttpGet("{id:int}/Stats")]
        public async Task<IActionResult> GetWithStats(int id )
        {
            var response = await _service.GetWithStatsAsync(id );
            return StatusCode(response.StatusCode, response);
        }


        [HttpPost]
        public async Task<IActionResult> Create( CreatePlayerDTO request )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.CreateAsync(request );
            return StatusCode(response.StatusCode, response);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdatePlayerDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _service.UpdateAsync(id, request );

            return StatusCode(response.StatusCode, response);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id );
            return StatusCode(response.StatusCode, response);
        }
    }
}
