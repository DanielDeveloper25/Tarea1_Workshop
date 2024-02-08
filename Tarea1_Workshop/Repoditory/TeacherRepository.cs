using Tarea1_Workshop.Data;
using Tarea1_Workshop.Models;
using Tarea1_Workshop.Repoditory.Interface;

namespace Tarea1_Workshop.Repoditory
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext context) : base(context){ }
    }
}
