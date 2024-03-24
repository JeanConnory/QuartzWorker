using Microsoft.EntityFrameworkCore;
using WorkerDemo.Contracts.Entities;

namespace WorkerDemo.Persistence
{
	public class WorkerDbContext : DbContext
	{
        public WorkerDbContext(DbContextOptions<WorkerDbContext> dbContextOptions) : base(dbContextOptions)
        {            
        }

        public DbSet<Log> Logs { get; set; }
    }
}
