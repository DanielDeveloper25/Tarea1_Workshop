using AutoMapper;
using Tarea1_Workshop.Models;
using Tarea1_Workshop.Models.Dtos;

namespace Tarea1_Workshop
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Teacher, TeacherDto>().ReverseMap();
            CreateMap<Teacher, CreateTeacherDto>().ReverseMap();
            CreateMap<Teacher, TeacherUpdateDto>().ReverseMap();

            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, CreateStudentDto>().ReverseMap();
            CreateMap<Student, StudentUpdateDto>().ReverseMap();
        }
    }
}
