using TalentBridge.Common.Entities;
using TalentBridge.Enums;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models;

public class Test : BaseEntity
{
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
    
    public int CreatedBy { get; set; }
    public HRManager CreatedByHRManager { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string Profession { get; set; }
    public string Industry { get; set; }
    
    public int DurationMinutes { get; set; }
    public decimal PassingScore { get; set; } 
    public decimal TotalPoints { get; set; }
    
    public TEST_DIFFICULTY Difficulty { get; set; } = TEST_DIFFICULTY.Medium;
    public bool IsActive { get; set; } = true;
    public string? Instructions { get; set; }
    
    public List<Question>? Questions { get; set; }
    public List<TestAssignment>? Assignments { get; set; }
}