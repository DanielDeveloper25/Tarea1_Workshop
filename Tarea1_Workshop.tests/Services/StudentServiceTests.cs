using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Tarea1_Workshop.Models.Dtos;
using Tarea1_Workshop.Models;
using Tarea1_Workshop.Repoditory.Interface;
using Tarea1_Workshop.Services;

namespace Tarea1_Workshop.tests.Services
{
    public class StudentServiceTests
    {
        [Fact]
        public async Task GetStudentById_ValidId_ReturnsStudentDto()
        {
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var mapperMock = new Mock<IMapper>();

            var studentService = new StudentService(studentRepositoryMock.Object, mapperMock.Object);

            var studentId = 1;
            var student = new Student { Id = studentId, Name = "Fulanito", Tution = "20222130", Career = "Software", Period = 7, Credits = 99 };
            var studentDto = new StudentDto { Id = studentId, Name = "Perensejo", Tution = "20222130", Career = "Software", Period = 7, Credits = 99 };

            studentRepositoryMock.Setup(repo => repo.Get(It.IsAny<System.Linq.Expressions.Expression<System.Func<Student, bool>>>(), false))
                .ReturnsAsync(student);
            mapperMock.Setup(mapper => mapper.Map<StudentDto>(It.IsAny<Student>())).Returns(studentDto);

            var result = await studentService.GetStudentById(studentId);

            Assert.NotNull(result);
            Assert.Equal(studentDto, result);
        }

        [Fact]
        public async Task CreateStudent_ValidDto_ReturnsStudentDto()
        {
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var mapperMock = new Mock<IMapper>();

            var studentService = new StudentService(studentRepositoryMock.Object, mapperMock.Object);

            var createStudentDto = new CreateStudentDto { Name = "Fulanito", Tution = "20222130", Career = "Software", Period = 7, Credits = 99 };
            var student = new Student { Id = 1, Name = "Fulanito", Tution = "20222130", Career = "Software", Period = 7, Credits = 99 };
            var studentDto = new StudentDto { Id = 1, Name = "Fulanito", Tution = "20222130", Career = "Software", Period = 7, Credits = 99 };

            studentRepositoryMock.Setup(repo => repo.Get(It.IsAny<System.Linq.Expressions.Expression<System.Func<Student, bool>>>(), false))
                .ReturnsAsync((Student)null);
            mapperMock.Setup(mapper => mapper.Map<Student>(It.IsAny<CreateStudentDto>())).Returns(student);
            studentRepositoryMock.Setup(repo => repo.add(It.IsAny<Student>())).Returns(Task.CompletedTask);
            mapperMock.Setup(mapper => mapper.Map<StudentDto>(It.IsAny<Student>())).Returns(studentDto);

            var result = await studentService.CreateStudent(createStudentDto);

            Assert.NotNull(result);
            Assert.Equal(studentDto, result);
        }
    }
}
