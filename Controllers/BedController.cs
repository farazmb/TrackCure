using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackCure.Dtos;
using TrackCure.Interfaces;
using TrackCure.Models;
using TrackCure.Services;

namespace TrackCure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedController : ControllerBase
    {
        private readonly IBedRepository _bedRepository;
        private readonly JwtService _jwtService;
        private readonly string _connectionString;
        public BedController(IBedRepository bedRepository, JwtService jwtService, IConfiguration configuration)
        {
            _bedRepository = bedRepository;
            _jwtService = jwtService;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        [HttpGet]
        [Authorize(Roles = "Receptionist")]
        public async Task <ActionResult<IEnumerable<Bed>>> GetAllAvailableBeds()
        {
            var bed = await _bedRepository.GetAllAvailableBeds();
            return Ok(bed);
        }

        [HttpPost("add-bed")]
        [Authorize(Roles = "Receptionist")]
        public async Task <ActionResult> AddBed(BedDto request)
        {
            var bed = new Bed
            {
                BedNumber = request.BedNumber,
                RoomNumber = request.RoomNumber,
                Status = request.Status
            };
            var success = await _bedRepository.AddBed(bed); 
            if (!success)
            {
                return BadRequest("UnSuccessful");
            }
            return Ok("Added Successfully");
        }



        [HttpGet("getbyid")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult<IEnumerable<Bed>>> GetBedById(int id)
        {
            var bed = await _bedRepository.GetBedById(id);
            if(bed == null)
            {
                return BadRequest("Bed Not Found");
            }
            return Ok(bed);
        }



        [HttpPut("update")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult<Bed>> UpdateBed([FromBody] BedDto bedDto, int Id)
        {
            var bed = new Bed
            {
                BedNumber = bedDto.BedNumber,
                RoomNumber = bedDto.RoomNumber,
                Status = bedDto.Status
            };

            var success = await _bedRepository.UpdateBed(Id, bed);
            if (!success)
            {
                return BadRequest("Couldn't Update");
            }
            return Ok("Updated Successfully");
        }





        [HttpDelete("delete")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult> DeleteBed(int Id)
        {
            var success = await _bedRepository.DeleteBed(Id);
            if (!success)
            {
                return BadRequest("Failed to Delete");
            }
            return Ok("Bed Deleted Successfully");
        }





    }
}
