using TalentBridge.Common.Entities;
using TalentBridge.Enums;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models;

public class Application : BaseEntity
{
    public int VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public APPLICATION_STATUS Status { get; set; } = APPLICATION_STATUS.Pending;
    public string? CoverLetter { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    
    public int? ReviewedBy { get; set; }
    public HRManager? ReviewedByHRManager { get; set; }
    public string? ReviewNotes { get; set; }
    public string? RejectionReason { get; set; }
    
    public List<TestAssignment>? TestAssignments { get; set; }
    public List<ApplicationTimeline>? Timeline { get; set; }
    public List<Message>? Messages { get; set; }
}