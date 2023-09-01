using BLL.Mappers;
using BLL.Models.Requests;
using BLL.Models.Responses;
using BLL.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using TelegramBot;

namespace SanyaProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        public IAppointmentService _appointmentService { get; set; }
      
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> CreateAppointment(AppointmentRequest request)
        {
            try
            {
                int newAppointmentId = await _appointmentService.CreateAsync(request);

                await BotRequestHandler.ConfirmRequest(request, newAppointmentId);

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create appointment.");
            }
        }
      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                bool isDeleted = await _appointmentService.TryToDeleteAsync(id);

                if (isDeleted)
                {
                    return NoContent(); // 204 No Content
                }
                else
                {
                    return NotFound(); // 404 Not Found
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentRequest request)
        {
            try
            {
                await _appointmentService.UpdateAsync(id, request);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }


        [HttpPut("ForDay")]
        public async Task<IActionResult> GetGetAppointmentsForDay(DateTime day)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsForDayAsync(day);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }


        [HttpGet("TimeCheck")]
        public async Task<ActionResult<bool>> CheckExceedingLimit(DateTime time)
        {
            try
            {              
                bool hasConfirmedAppointments = await _appointmentService.HasConfirmedAppointmentsOnTime(time);
                bool isLimitExceeded = await _appointmentService.isExceedingLimit(time);
                
                bool isTimeAvailable = !(hasConfirmedAppointments || isLimitExceeded);


                return Ok(isTimeAvailable);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the process
                return BadRequest("An error occurred while checking the limit.");
            }
        }

    }
}
