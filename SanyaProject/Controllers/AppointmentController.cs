using BLL.Mappers;
using BLL.Models.Requests;
using BLL.Models.Responses;
using BLL.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(typeof(List<AppointmentResponse>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAppointment(AppointmentRequest request)
        {
            try
            {
                await _appointmentService.CreateAsync(request);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create appointment.");
            }
        }

        [HttpPut("Confirm/{id}")]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            try
            {
                var isConfirmed = await _appointmentService.TryToConfirmAppointment(id);


                if(isConfirmed)
                {
                    return Ok(); // Return 200 OK on success
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    }
}
