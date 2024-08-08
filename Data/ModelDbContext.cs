using CoreApiInNet.Model;
using Microsoft.EntityFrameworkCore;

namespace CoreApiInNet.Data
{
    public class ModelDbContext:DbContext
    {
        public ModelDbContext(DbContextOptions options):base(options) 
        { 
        
        }
        public DbSet<DbModelUser> UserModel { get; set; }
        public DbSet<DbModelData> DataModel { get; set; }
    }
}
