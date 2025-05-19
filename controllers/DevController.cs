using System.Security.Claims;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers{
    [ApiController]
    [Route("api/dev")]
    public class DevController : ControllerBase{

        private readonly IDevService _devService;

        public DevController(IDevService devService){
            _devService = devService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevResponseDTO>>> GetAll(){
            var devs = await _devService.GetAllDevsAsync();

            var devsListDTO = devs.Select(dev => new DevResponseDTO {
                Id = dev.Id,
                Name = dev.Name,
                Email = dev.Email
            }).ToList();

            return Ok(devsListDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DevResponseDTO>> GetById(string id){
            var dev = await _devService.GetDevByIdAsync(id);

            return Ok(new DevResponseDTO {
                Id = dev.Id,
                Name = dev.Name,
                Email = dev.Email
            }); 
        }

        [HttpPost]
        public async Task<ActionResult<DevResponseDTO>> Create(Dev dev){
            var created = await _devService.CreateDevAsync(dev);

            var responseDTO = new DevResponseDTO {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email
            };

            return CreatedAtAction(nameof(GetById), new {id = created.Id}, responseDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "DEV")]
        public async Task<ActionResult<DevResponseDTO>> Update(string id, UpdateDevDTO dev)
        {
            var devIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (devIdFromToken == null)
            {
                return Unauthorized("You must be logged in to update your account information");
            }

            if (devIdFromToken != id)
            {
                return Forbid("You only can update YOUR account information");
            }

            var update = await _devService.UpdateDevAsync(devIdFromToken, dev);

            if (update == null)
            {
                return NotFound("User not found!!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DEV")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var devIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (devIdFromToken != id)
            {
                return Unauthorized("You can delete only YOUR account");
            }

            var deleted = await _devService.DeleteDevAsync(devIdFromToken);

            if (!deleted)
            {
                return NotFound("User not found!!");
            }

            return NoContent();
        }
    }
}