using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackCure.Dtos;
using TrackCure.Implementations;
using TrackCure.Interfaces;
using TrackCure.Models;
using TrackCure.Services;

namespace TrackCure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAvailabilityController : ControllerBase
    {
        public readonly JwtService _jwtService;
        public readonly IDoctorAvailability _doctorAvailability;

        public DoctorAvailabilityController(JwtService jwtService,IDoctorAvailability doctorAvailability)
        {
            _jwtService = jwtService;
            _doctorAvailability = doctorAvailability;
        }


        [HttpPost("add-availability")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<string>> AddAvailability(DoctorAvailabilityDto Availabilitydto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var availability = new DoctorAvailability
            {
                UserId = userId,
                AvailableDate = Availabilitydto.AvailableDate,
                StartTime = Availabilitydto.StartTime,
                EndTime = Availabilitydto.EndTime
            };

            var result = await _doctorAvailability.AddAvailabilityAsync(availability);
            if (result)
                return Ok("Availability added successfully");

            return BadRequest("Failed to add availability");
        }




        [HttpGet("getbyid")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<DoctorAvailability>> GetAvailabilityByDoctor(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var availability = await _doctorAvailability.GetAvailabilityByDoctor(id);

            if (availability == null)
                return NotFound("No availability found for this doctor");

            return Ok(availability);
        }




        [HttpPut("update")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<DoctorAvailability>> UpdateAvailability([FromBody] DoctorAvailabilityDto availabilityDto, int Id)
        {
            var availability = new DoctorAvailability
            {
                AvailableDate = availabilityDto.AvailableDate,
                StartTime = availabilityDto.StartTime,
                EndTime = availabilityDto.EndTime
            };

            var success = await _doctorAvailability.UpdateAvailability(Id, availability);
            if (!success)
            {
                return BadRequest("Couldn't Update");
            }
            return Ok("Updated Successfully");
        }





        [HttpDelete("delete")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> DeleteAvailability (int Id)
        {
            var success = await _doctorAvailability.DeleteAvailability(Id);
            if (!success)
            {
                return BadRequest("Failed to Delete");
            }
            return Ok("Deleted Successfully");
        }
    }


}

