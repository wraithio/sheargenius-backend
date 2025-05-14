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
        [HttpPut("EditSchedule")]
        public async Task<IActionResult> EditSchedule([FromBody] ScheduleModel schedule)
        {
            var result = await _scheduleServices.EditSchedule(schedule);
            return result ? Ok("Schedule edited successfully.") : BadRequest("Failed to save schedule.");
        }

        [HttpPost("SendRequest")]
        public async Task<IActionResult> SendRequest([FromBody] RequestModel request)
        {
            var result = await _scheduleServices.SendRequest(request);
            return result ? Ok("Request saved successfully.") : BadRequest("Failed to save Request.");
        }

        [HttpGet("GetScheduleById/{id}")]
        public async Task<ActionResult<ScheduleModel>> GetScheduleByUserId(int id)
        {
            var schedule = await _scheduleServices.GetScheduleByUserId(id);
            if (schedule == null) return NotFound("Schedule not found.");
            return Ok(schedule);
        }


        [HttpGet("GetSheduleByUsername/{username}")]
        public async Task<ActionResult<ScheduleModel>> GetScheduleByUsername(string username)
        {
            var schedule = await _scheduleServices.GetScheduleByUsername(username);
            if (schedule == null) return NotFound("No schedules found for that username.");
            return Ok(schedule);
        }

        [HttpGet("SeeAllSchedules")]
        public async Task<ActionResult<List<ScheduleModel>>> GetAllSchedules()
        {
            var schedules = await _scheduleServices.SeeAllSchedules();
            return Ok(schedules);
        }
        [HttpGet("SeeAllRequests")]
        public async Task<ActionResult<List<RequestModel>>> GetAllRequests()
        {
            var schedules = await _scheduleServices.SeeAllRequests();
            return Ok(schedules);
        }

        [HttpDelete("DeleteSchedule/{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var success = await _scheduleServices.DeleteSchedule(id);
            return success ? Ok("Schedule deleted successfully.") : NotFound("Schedule not found.");
        }

        [HttpDelete("DeleteRequest/{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var success = await _scheduleServices.DeleteRequest(id);
            return success ? Ok("Request deleted successfully.") : NotFound("Request not found.");
        }

        [HttpPut("UpdateSchedule")]
        public async Task<IActionResult> UpdateSchedule([FromBody] ScheduleModel updatedSchedule)
        {


            var result = await _scheduleServices.UpdateSchedule(updatedSchedule);

            if (result)
                return Ok("Schedule updated successfully.");
            else
                return NotFound("Schedule not found or update failed.");
        }

        // [HttpGet("CheckAvailability")]
        // public async Task<IActionResult> CheckAvailability(string day, string time, string BarberName)
        // {
        //     // if (string.IsNullOrEmpty(day) || string.IsNullOrEmpty(time))
        //     //     return BadRequest("Both day and time must be provided.");

        //     var isAvailable = await _scheduleServices.CheckAvailability(day, time, BarberName);

        //     if (isAvailable)
        //         return Ok(new { available = true, message = "The requested time slot is available." });
        //     else
        //         return Ok(new { available = false, message = "The requested time slot is not available." });
        // }

        [HttpGet("FilterScheduleByRequest/{barberName}")]
        public async Task<IActionResult> FilterScheduleByRequest(string barberName)
        {
            var result = await _scheduleServices.FilterScheduleByRequest(barberName);

            return Ok(result);
        }

        [HttpPut("AcceptRequest/{id}")]
        public async Task<IActionResult>AcceptRequest (int id)
        {
            var result = await _scheduleServices.AcceptRequest(id);
            return Ok(result);
        }

        [HttpPut("DeclineRequest/{id}")]
        public async Task<IActionResult>DeclineRequest (int id)
        {
            var result = await _scheduleServices.DeclineRequest(id);
            return Ok(result);
        }

        [HttpGet("FindRequestsByBarberName/{barberName}")]
        public async Task<IActionResult>FindRequestsByBarberName (string barberName)
        {
            var result = await _scheduleServices.FindRequestsByBarberName(barberName);
            return Ok(result);
        }

        [HttpGet("FindRequestsByUsername/{username}")]
        public async Task<IActionResult>FindRequestsByUsername (string username)
        {
            var result = await _scheduleServices.FindRequestsByUsername(username);
            return Ok(result);
        }
    }
}
