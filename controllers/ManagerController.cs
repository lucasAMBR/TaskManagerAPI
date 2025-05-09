using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Controllers{
    
    [ApiController]
    [Route("api/manager")]
    public class ManagerController : ControllerBase{

        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService){
            _managerService = managerService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManagerResponseDTO>>> GetAll(){
            var managers = await _managerService.GetAllManagersAsync();

            var managersListDTO = managers.Select(manager =>new ManagerResponseDTO{
                Id = manager.Id,
                Name = manager.Name,
                Email = manager.Email
            }).ToList();

            return Ok(managersListDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ManagerResponseDTO>> GetById(string id){
            var manager = await _managerService.GetManagerByIdAsync(id);

            return Ok(new ManagerResponseDTO {
                Id = manager.Id,
                Name = manager.Name,
                Email = manager.Email
            });
        }

        [HttpPost]
        public async Task<ActionResult<ManagerResponseDTO>> Create(Manager manager){
            
            var created = await _managerService.CreateManagerAsync(manager);

            var responseDTO = new ManagerResponseDTO {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email
            };

            return CreatedAtAction(nameof(GetById), new {id = created.Id}, responseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Manager manager){
            if(id != manager.Id){
                return BadRequest("the URL id doens't match with the id in the request body!!");
            }

            var updated = await _managerService.UpdateManagerAsync(manager);

            if(updated == null){
                return NotFound("User not Found!!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){
            var deleted = await _managerService.DeleteManagerAsync(id);

            if(!deleted){
                return NotFound("User not found!!");
            }

            return NoContent();
        }

    }
}