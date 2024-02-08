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
    public class TeacherController : ControllerBase
    {
        public readonly ILogger<TeacherController> _logger;
        public readonly ITeacherRepository _teacherRepo;
        public readonly IMapper _mapper;
        protected APIResponse _response;
        public TeacherController(ILogger<TeacherController> logger, ITeacherRepository proyectoRepositorio, IMapper mapper)
        {
            _logger = logger;
            _teacherRepo = proyectoRepositorio;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet("GetTeachers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetTeachers()
        {
            try
            {
                _logger.LogInformation("Get all techers");

                IEnumerable<Teacher> teachers = await _teacherRepo.GetAll();

                _response.Resultado = _mapper.Map<IEnumerable<TeacherDto>>(teachers);
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

        [HttpGet(Name = "GetTeacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetTeacher(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error to get the teacher with id:" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var teacher = await _teacherRepo.Get(t => t.Id == id);

                if (teacher == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<TeacherDto>(teacher);
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

        [HttpPost("CreateTeacher")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateTeacher([FromBody] CreateTeacherDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _teacherRepo.Get(t => t.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("TeacherExist", "This teacher alredy exist in the database");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Teacher teacher = _mapper.Map<Teacher>(createDto);

                await _teacherRepo.add(teacher);

                _response.Resultado = teacher;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetTeacher", new { id = teacher.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("DeleteTeacher")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var teacher = await _teacherRepo.Get(t => t.Id == id);

                if (teacher == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _teacherRepo.Remove(teacher);
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

        [HttpPost("UpdateTeacher")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherUpdateDto UpdateDto)
        {
            if (UpdateDto == null || id != UpdateDto.Id)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Teacher teacher = _mapper.Map<Teacher>(UpdateDto);

            await _teacherRepo.Update(teacher);

            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}
