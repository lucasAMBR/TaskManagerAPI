using System.Collections;
using System.Security.Claims;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers{

    [ApiController]
    [Route("api/equip")]
    public class EquipController : ControllerBase {

        private readonly IEquipService _equipService;

        private readonly IEquipAndDevService _equipAndDevService;

        public EquipController(IEquipService equipService, IEquipAndDevService equipAndDevService){
            _equipService = equipService;
            _equipAndDevService = equipAndDevService;
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
        [Authorize(Roles = "MNG")]
        public async Task<ActionResult<Equip>> Create(CreateEquipDTO equip)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken == null)
            {
                return Unauthorized("You must be logged in to create a equip");
            }

            var created = await _equipService.CreateEquipAsync(managerIdFromToken, equip);

            var addResult = await _equipAndDevService.AddDevToEquip(created.Id, equip.LeaderId);

            if (!addResult)
            {
                return BadRequest("Somthing in process to add member to equip go wrong");
            }

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "MNG")]
        public async Task<ActionResult<Equip>> Update(string id, UpdateEquipDTO equip)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken == null)
            {
                return Unauthorized("You must be logged to update a equip");
            }

            var updated = await _equipService.UpdateEquipAsync(managerIdFromToken, id, equip);

            if (updated == null)
            {
                return NotFound("Equip not Found!!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MNG")]
        public async Task<IActionResult> Delete(string id)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken == null)
            {
                return Unauthorized("You must be logged to delete a equip");
            }

            var deleted = await _equipService.DeleteEquipAsync(managerIdFromToken, id);

            if (!deleted)
            {
                return NotFound("Equip not found!!");
            }

            return NoContent();
        }

        [HttpPost("{equipId}/add/{devId}")]
        [Authorize(Roles = "MNG,DEV")]
        public async Task<IActionResult> AddDevToEquip(string equipId, string devId)
        {
            var equip = await _equipService.GetEquipByIdAsync(equipId);

            if (equip.Project == null)
            {
                return BadRequest("You cannot add a member in a equip that not exists");
            }

            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole("MNG"))
            {
                if (equip.Project.ManagerId != userIdFromToken)
                {
                    return Forbid("Only the manager of this project can add members");
                }
            }

            if (User.IsInRole("DEV"))
            {
                if (equip.LeaderId != userIdFromToken)
                {
                    return Forbid("Only the Leader of this equip can add members");
                }
            }
            
            var addResult = await _equipAndDevService.AddDevToEquip(equipId, devId);

            if (!addResult)
            {
                return BadRequest("Invalid Equip ID or Dev ID");
            }

            return NoContent();
        }
    }
}