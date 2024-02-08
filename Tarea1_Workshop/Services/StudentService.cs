using AutoMapper;
using Tarea1_Workshop.Models;
using Tarea1_Workshop.Models.Dtos;
using Tarea1_Workshop.Repoditory.Interface;

namespace Tarea1_Workshop.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> GetStudents()
        {
            var students = await _studentRepository.GetAll();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto> GetStudentById(int id)
        {
            var student = await _studentRepository.Get(s => s.Id == id);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> CreateStudent(CreateStudentDto createDto)
        {
            var existingStudent = await _studentRepository.Get(s => s.Name.ToLower() == createDto.Name.ToLower());
            if (existingStudent != null)
            {
                return null;
            }

            var student = _mapper.Map<Student>(createDto);
            await _studentRepository.add(student);

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<bool> UpdateStudent(int id, StudentUpdateDto updateDto)
        {
            var student = await _studentRepository.Get(s => s.Id == id);
            if (student == null)
            {
                return false;
            }

            _mapper.Map(updateDto, student);
            await _studentRepository.Update(student);

            return true;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _studentRepository.Get(s => s.Id == id);
            if (student == null)
            {
                return false;
            }

            await _studentRepository.Remove(student);

            return true;
        }
    }
}
