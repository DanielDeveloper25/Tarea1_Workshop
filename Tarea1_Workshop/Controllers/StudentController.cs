using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Tarea1_Workshop.Repoditory.Interface;
using Tarea1_Workshop.Models;
using Tarea1_Workshop.Models.Dtos;

namespace Tarea1_Workshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        public readonly ILogger<StudentController> _logger;
        public readonly IStudentRepository _studentRepo;
        public readonly IMapper _mapper;
        protected APIResponse _response;
        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepo, IMapper mapper)
        {
            _logger = logger;
            _studentRepo = studentRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet("GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetStudent()
        {
            try
            {
                _logger.LogInformation("Get all student");

                IEnumerable<Student> students = await _studentRepo.GetAll();

                _response.Resultado = _mapper.Map<IEnumerable<StudentDto>>(students);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet(Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetStudent(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error to get the student with id:" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var student = await _studentRepo.Get(s => s.Id == id);

                if (student == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<StudentDto>(student);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("CreateStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateTeacher([FromBody] CreateStudentDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _studentRepo.Get(t => t.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("StudentExist", "This student alredy exist in the database");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Student student = _mapper.Map<Student>(createDto);

                await _studentRepo.add(student);

                _response.Resultado = student;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetStudent", new { id = student.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var student = await _studentRepo.Get(s => s.Id == id);

                if (student == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _studentRepo.Remove(student);
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPost("UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateDto UpdateDto)
        {
            if (UpdateDto == null || id != UpdateDto.Id)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Student student = _mapper.Map<Student>(UpdateDto);

            await _studentRepo.Update(student);

            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}
