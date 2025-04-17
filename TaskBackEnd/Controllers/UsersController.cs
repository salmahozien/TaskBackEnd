using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskBackEnd.Dtos;
using TaskBackEnd.Interfaces;
using TaskBackEnd.Services;

namespace TaskBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("AddUsers")]
        public async Task<IActionResult> AddUser([FromForm]AddUserDto model)
        {
            var result = await _unitOfWork.Users.AddUser(model);
            if (result.Fail != string.Empty)
            {
                return BadRequest(result.Fail);
            }
            return Ok(result.Success);

        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result= await _unitOfWork.Users.GetAllUsers();
            return Ok(result);
        }
        [HttpGet("GenerateUserPdf")]
        public async Task<IActionResult> GenerateUserPdf([FromQuery]int userId)
        {
            var pdfBytes = await _unitOfWork.Users.GeneratePdf(userId);

            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                return NotFound("Failed to generate PDF.");
            }

            return File(pdfBytes, "application/pdf", $"User_{userId}_Details.pdf");
        }
    }
    }

