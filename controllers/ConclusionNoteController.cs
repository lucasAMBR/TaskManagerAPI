using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/conclusion_note")]
    public class ConclusionNoteController : ControllerBase
    {
        private readonly IConclusionNoteService _conclusionNoteService;

        public ConclusionNoteController(IConclusionNoteService conclusionNoteService)
        {
            this._conclusionNoteService = conclusionNoteService;
        }

        [HttpGet("{equipId}")]
        [Authorize(Roles = "DEV,MNG")]
        public async Task<ActionResult<List<ConclusionNote>>> GetConclusionNotesByEquipId(string equipId)
        {
            return await _conclusionNoteService.ListAllConclusionNotesByEquipId(equipId);
        }
    }
}