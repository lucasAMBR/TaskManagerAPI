using DTOs;
using Interfaces;
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
        public async Task<ActionResult<DevResponseDTO>> Update(string id, Dev dev){
            if(id != dev.Id){
                return BadRequest("The URL ID doesn't match with the ID in request body!!");
            }

            var update = await _devService.UpdateDevAsync(dev);

            if(update == null){
                return NotFound("User not found!!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string id){
            var deleted = await _devService.DeleteDevAsync(id);

            if(!deleted){
                return NotFound("User not found!!");
            }

            return NoContent();
        }
    }
}