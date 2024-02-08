using AutoMapper;
using Tarea1_Workshop.Models;
using Tarea1_Workshop.Models.Dtos;
using Tarea1_Workshop.Repoditory.Interface;

namespace Tarea1_Workshop.Services
{
    public class TeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeacherDto>> GetTeachers()
        {
            var teachers = await _teacherRepository.GetAll();
            return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
        }

        public async Task<TeacherDto> GetTeacherById(int id)
        {
            var teacher = await _teacherRepository.Get(t => t.Id == id);
            return _mapper.Map<TeacherDto>(teacher);
        }

        public async Task<TeacherDto> CreateTeacher(CreateTeacherDto createDto)
        {
            var existingTeacher = await _teacherRepository.Get(t => t.Name.ToLower() == createDto.Name.ToLower());
            if (existingTeacher != null)
            {
                return null;
            }

            var teacher = _mapper.Map<Teacher>(createDto);
            await _teacherRepository.add(teacher);

            return _mapper.Map<TeacherDto>(teacher);
        }

        public async Task<bool> UpdateTeacher(int id, TeacherUpdateDto updateDto)
        {
            var teacher = await _teacherRepository.Get(t => t.Id == id);
            if (teacher == null)
            {
                return false;
            }

            _mapper.Map(updateDto, teacher);
            await _teacherRepository.Update(teacher);

            return true;
        }

        public async Task<bool> DeleteTeacher(int id)
        {
            var teacher = await _teacherRepository.Get(t => t.Id == id);
            if (teacher == null)
            {
                return false;
            }

            await _teacherRepository.Remove(teacher);

            return true;
        }
    }
}
