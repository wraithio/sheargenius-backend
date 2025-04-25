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


        [HttpPost("SetSchedule")]
        public async Task<IActionResult> SetSchedule([FromBody] ScheduleModel schedule)
        {
            var result = await _scheduleServices.SetSchedule(schedule);
            return result ? Ok("Schedule saved successfully.") : BadRequest("Failed to save schedule.");
        }


        [HttpGet("GetScheduleById/{id}")]
        public async Task<ActionResult<ScheduleModel>> GetScheduleByUserId(int id)
        {
            var schedule = await _scheduleServices.GetScheduleByUserId(id);
            if (schedule == null) return NotFound("Schedule not found.");
            return Ok(schedule);
        }


        [HttpGet("GetSheduleByUsername/{username}")]
        public async Task<ActionResult<List<ScheduleModel>>> GetScheduleByUsername(string username)
        {
            var schedules = await _scheduleServices.GetScheduleByUsername(username);
            if (schedules == null || !schedules.Any()) return NotFound("No schedules found for that username.");
            return Ok(schedules);
        }

        [HttpGet("SeeAllSchedules")]
        public async Task<ActionResult<List<ScheduleModel>>> GetAllSchedules()
        {
            var schedules = await _scheduleServices.SeeAllSchedules();
            return Ok(schedules);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var success = await _scheduleServices.DeleteSchedule(id);
            return success ? Ok("Schedule deleted successfully.") : NotFound("Schedule not found.");
        }


       

        [HttpPut("UpdateSchedule")]
        public async Task<IActionResult> UpdateSchedule( [FromBody] ScheduleModel updatedSchedule)
        {
           

            var result = await _scheduleServices.UpdateSchedule(updatedSchedule);

            if (result)
                return Ok("Schedule updated successfully.");
            else
                return NotFound("Schedule not found or update failed.");
        }

        [HttpGet("CheckAvailability")]
        public async Task<IActionResult> CheckAvailability(string username, [FromQuery] string day, [FromQuery] string time)
        {
            if (string.IsNullOrEmpty(day) || string.IsNullOrEmpty(time))
                return BadRequest("Both day and time must be provided.");

            var isAvailable = await _scheduleServices.CheckAvailability(username, day, time);

            if (isAvailable)
                return Ok(new { available = true, message = "The requested time slot is available." });
            else
                return Ok(new { available = false, message = "The requested time slot is not available." });
        }
    }
}
