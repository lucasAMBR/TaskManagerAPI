using System.Collections;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers{

    [ApiController]
    [Route("api/equip")]
    public class EquipController : ControllerBase {

        private readonly IEquipService _equipService;

        public EquipController(IEquipService equipService){
            _equipService = equipService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equip>>> GetAll(){
            return await _equipService.GetAllEquipsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Equip>> GetById(string id){
            return await _equipService.GetEquipByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Equip>> Create(Equip equip){
            var created = await _equipService.CreateEquipAsync(equip);

            return CreatedAtAction(nameof(GetById), new {id = created.Id}, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Equip>> Update(string id, Equip equip){
            if(id != equip.Id){
                return BadRequest("the URL id doens't match with the id in the request body!!");
            }

            var updated = await _equipService.UpdateEquipAsync(equip);

            if(updated == null){
                return NotFound("Equip not Found!!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){
            var deleted = await _equipService.DeleteEquipAsync(id);

            if(!deleted){
                return NotFound("Equip not found!!");
            }

            return NoContent();
        }

    }
}