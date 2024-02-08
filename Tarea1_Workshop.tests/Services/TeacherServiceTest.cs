using AutoMapper;
using Moq;
using Tarea1_Workshop.Models.Dtos;
using Tarea1_Workshop.Models;
using Tarea1_Workshop.Repoditory.Interface;
using Tarea1_Workshop.Services;

namespace Tarea1_Workshop.tests.Controllers
{
    public class TeacherServiceTest
    {
        [Fact]
        public async Task GetTeacherById_ValidId_ReturnsTeacherDto()
        {
            // Arrange
            var teacherRepositoryMock = new Mock<ITeacherRepository>();
            var mapperMock = new Mock<IMapper>();

            var teacherService = new TeacherService(teacherRepositoryMock.Object, mapperMock.Object);

            var teacherId = 1;
            var teacher = new Teacher { Id = teacherId, Name = "Fulanito", Subject = "Math" };
            var teacherDto = new TeacherDto { Id = teacherId, Name = "Perensejo", Subject = "Math" };

            teacherRepositoryMock.Setup(repo => repo.Get(It.IsAny<System.Linq.Expressions.Expression<System.Func<Teacher, bool>>>(), false))
                .ReturnsAsync(teacher);
            mapperMock.Setup(mapper => mapper.Map<TeacherDto>(It.IsAny<Teacher>())).Returns(teacherDto);

            // Act
            var result = await teacherService.GetTeacherById(teacherId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(teacherDto, result);
        }

        [Fact]
        public async Task CreateTeacher_ValidDto_ReturnsTeacherDto()
        {
            // Arrange
            var teacherRepositoryMock = new Mock<ITeacherRepository>();
            var mapperMock = new Mock<IMapper>();

            var teacherService = new TeacherService(teacherRepositoryMock.Object, mapperMock.Object);

            var createTeacherDto = new CreateTeacherDto { Name = "John Doe", Subject = "Math" };
            var teacher = new Teacher { Id = 1, Name = "John Doe", Subject = "Math" };
            var teacherDto = new TeacherDto { Id = 1, Name = "John Doe", Subject = "Math" };

            teacherRepositoryMock.Setup(repo => repo.Get(It.IsAny<System.Linq.Expressions.Expression<System.Func<Teacher, bool>>>(), false))
                .ReturnsAsync((Teacher)null);
            mapperMock.Setup(mapper => mapper.Map<Teacher>(It.IsAny<CreateTeacherDto>())).Returns(teacher);
            teacherRepositoryMock.Setup(repo => repo.add(It.IsAny<Teacher>())).Returns(Task.CompletedTask);
            mapperMock.Setup(mapper => mapper.Map<TeacherDto>(It.IsAny<Teacher>())).Returns(teacherDto);

            // Act
            var result = await teacherService.CreateTeacher(createTeacherDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(teacherDto, result);
        }
    }
}
