using Tarea1_Workshop.Data;
using Tarea1_Workshop.Models;
using Tarea1_Workshop.Repoditory.Interface;

namespace Tarea1_Workshop.Repoditory
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }
    }
}
