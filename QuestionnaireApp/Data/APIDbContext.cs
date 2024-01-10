using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Model;

namespace QuestionnaireApp.Data
{
    public class APIDbContext : DbContext
    {

        public APIDbContext(DbContextOptions option) : base(option)
        {

        }

        public DbSet<Question> Questions { get; set; }

    }
}
