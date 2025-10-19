using TalentBridge.Common.Entities;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models;

public class TestSubmission : BaseEntity
{
    public int TestAssignmentId { get; set; }
    public TestAssignment TestAssignment { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int TestId { get; set; }
    public Test Test { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? SubmittedAt { get; set; }
    
    public decimal? TotalPointsEarned { get; set; }
    public decimal? PercentageScore { get; set; }
    public bool? IsPassed { get; set; }
    
    public bool IsAutoGraded { get; set; } = true;
    public bool RequiresManualReview { get; set; } = false;
    
    public int? ReviewedBy { get; set; }
    public HRManager? ReviewedByHRManager { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? Feedback { get; set; }
    
    public List<SubmissionAnswer>? Answers { get; set; }
}