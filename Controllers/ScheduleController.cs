using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sheargenius_backend.Models;
using sheargenius_backend.Services;

namespace sheargenius_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleServices _scheduleServices;

        public ScheduleController(ScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
        }

        // POST /Schedule
        [HttpPost]
        public async Task<IActionResult> SetSchedule([FromBody] ScheduleModel schedule)
        {
            var result = await _scheduleServices.SetSchedule(schedule);
            return result ? Ok("Schedule saved successfully.") : BadRequest("Failed to save schedule.");
        }

        // GET /Schedule/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleModel>> GetScheduleByUserId(int id)
        {
            var schedule = await _scheduleServices.GetScheduleByUserId(id);
            if (schedule == null) return NotFound("Schedule not found.");
            return Ok(schedule);
        }

        // GET /Schedule/username/{username}
[HttpGet("username/{username}")]
public async Task<ActionResult<List<ScheduleModel>>> GetScheduleByUsername(string username)
{
    var schedules = await _scheduleServices.GetScheduleByUsername(username);
    if (schedules == null || !schedules.Any()) return NotFound("No schedules found for that username.");
    return Ok(schedules);
}

    }
}
