using System.Data;
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
    public class EquipmentController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly string _connectionString;

        public EquipmentController(IConfiguration configuration, JwtService jwtService, IEquipmentRepository equipmentRepository)
        {
            _jwtService = jwtService;
            _equipmentRepository = equipmentRepository;
            _connectionString = configuration.GetConnectionString("DefualtConnection")!;
        }

        [HttpPost("add-equipment")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult> AddEquipment(EquipmentDto request)
        {
            var equipment = new Equipment
            {
                Name = request.Name,
                Type = request.Type,
                Status = request.Status
            };

            var success = await _equipmentRepository.AddEquipment(equipment);
            if (!success)
            {
                return BadRequest("Failed to Add");
            }
            return Ok("Addded Succesfully");
        }


        [HttpGet]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult<Equipment>> GetAllEquipment()
        {
            var equipment = await _equipmentRepository.GetAllAvailableEquipments();
            return Ok(equipment);
        }

        [HttpGet("getbyid")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipmentById(int id)
        {
            var quipment = await _equipmentRepository.GetEquipmentById(id);
            if (quipment == null)
            {
                return BadRequest("Equipment not found");
            }
            return Ok(quipment);
        }

        [HttpPut("update-equipment")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult<Equipment>> UpdateEquipment( EquipmentDto equipmentDto, int id)
        {
            var equipment = new Equipment
            {
                Name = equipmentDto.Name,
                Type = equipmentDto.Type,
                Status = equipmentDto.Status
            };

            var success = await _equipmentRepository.UpdateEquipment(id, equipment);
            if (!success)
            {
                return BadRequest("Couldn't Update");
            }
            return Ok("Updated Successfully!");
        }


        [HttpDelete("delete-equipment")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult> DeleteEquipment(int id)
        {
            var success = await _equipmentRepository.DeleteEquipment(id);
            if (!success)
            {
                return BadRequest("Eqiupment not found");
            }
            return Ok("Deleted Successfully!");
        }

    }
}
