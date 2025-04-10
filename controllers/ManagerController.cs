using Interfaces;
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
        public async Task<ActionResult<IEnumerable<Manager>>> GetAll(){
            var managers = await _managerService.GetAllManagersAsync();

            return Ok(managers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Manager>> GetById(string id){
            var manager = await _managerService.GetManagerByIdAsync(id);

            return Ok(manager);
        }

        [HttpPost]
        public async Task<ActionResult<Manager>> Create(Manager manager){
            var created = await _managerService.CreateManagerAsync(manager);

            return CreatedAtAction(nameof(GetById), new {id = created.Id}, created);
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
                return NotFound("User nor Found!!");
            }

            return NoContent();
        }

    }
}