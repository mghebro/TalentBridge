using TalentBridge.Common.Entities;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models;

public class SavedVacancy : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }
    
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}