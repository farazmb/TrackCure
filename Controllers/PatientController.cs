            using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackCure.Interfaces;
using TrackCure.Models;
using TrackCure.Services;

namespace TrackCure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
       
        private readonly JwtService _jwtService;
        private readonly IPatientRepository _patientRepository;


        public PatientController( JwtService jwtService, IPatientRepository patientRepository)
        {            
            _jwtService = jwtService;
            _patientRepository = patientRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetAllPatients()
        {
            var patient = await _patientRepository.GetAllPatients();
            return Ok(patient);
        }


        [HttpPost("add-patient")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult> RegisterPatient(Patient request)
        {
            var patient = new Patient {
                Gender = request.Gender,
                DOB = request.DOB,
                Address = request.Address,
                Phone = request.Phone,
                AddedBy = request.AddedBy,
                AdmissionDate = request.AdmissionDate,
                DischargeDate = request.DischargeDate,
                BillingStatus = request.BillingStatus,
                Symptom = request.Symptom,
                Note = request.Note,
                FullName = request.FullName
            };
            var success = await _patientRepository.RegisterPatient(patient);
            if (!success)
            {
                return BadRequest("Registeration Failed");
            }
            return Ok("Patient registered successfully");
        }


        [HttpGet("getbyid")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientById(int id)
        {
            var patient = await _patientRepository.GetPatientById(id);
            if (patient == null)
            {
                return BadRequest("Patient Not Found");
            }
            return Ok(patient);

        }


        [HttpPut("update-patient")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<Patient>> UpdatePatient(int id, Patient patient)
        {
            var success = await _patientRepository.UpdatePatient(id, patient);
            if (!success)
            {
                return BadRequest("Couldn't Update");
            }

            return Ok("Patient Updated Successfully");
        }

        [HttpDelete("delete-patient")]
        [Authorize(Roles = "Admin Receptionist")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            var success = await _patientRepository.DeletePatient(id);
            if (!success)
            {
                return BadRequest("Coulnd't Delete");
            }
            return Ok("Patient Deleted Successfully");
        }

    }

}
