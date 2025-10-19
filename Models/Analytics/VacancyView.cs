using TalentBridge.Common.Entities;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models;

public class VacancyView : BaseEntity
{
    public int VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }
    
    public int? UserId { get; set; }
    public User? User { get; set; }
    
    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}