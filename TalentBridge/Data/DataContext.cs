using Microsoft.EntityFrameworkCore;
using TalentBridge.Models.Auth;
using TalentBridge.Models.Roles;
namespace TalentBridge.Data;

public class DataContext : DbContext
{
    // Auth
    public DbSet<User> Users { get; set; }
    public DbSet<EmailVerification> EmailVerifications { get; set; }
    public DbSet<PasswordVerification> PasswordVerifications { get; set; }

    // 

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}