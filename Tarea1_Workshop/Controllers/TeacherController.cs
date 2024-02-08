using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Tarea1_Workshop.Repoditory.Interface;
using Tarea1_Workshop.Models;
using Tarea1_Workshop.Models.Dtos;
using Tarea1_Workshop.Services;

namespace Tarea1_Workshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        public readonly ILogger<TeacherController> _logger;
        public readonly TeacherService _teacherService;
        public readonly IMapper _mapper;
        protected APIResponse _response;
        public TeacherController(ILogger<TeacherController> logger, TeacherService teacherService, IMapper mapper)
        {
            _logger = logger;
            _teacherService = teacherService;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet("GetTeachers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetTeachers()
        {
            try
            {
                _logger.LogInformation("Get all teachers");

                IEnumerable<TeacherDto> teachers = await _teacherService.GetTeachers();

                _response.Result = teachers;
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

                var teacher = await _teacherService.GetTeacherById(id);

                if (teacher == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Result = teacher;
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

                var createdTeacher = await _teacherService.CreateTeacher(createDto);

                if (createdTeacher == null)
                {
                    ModelState.AddModelError("TeacherExist", "This teacher already exists in the database");
                    return BadRequest(ModelState);
                }

                _response.Result = createdTeacher;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetTeacher", new { id = createdTeacher.Id }, _response);
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

                var isDeleted = await _teacherService.DeleteTeacher(id);

                if (!isDeleted)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

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
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var isUpdated = await _teacherService.UpdateTeacher(id, updateDto);

                if (!isUpdated)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

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

    }
}
