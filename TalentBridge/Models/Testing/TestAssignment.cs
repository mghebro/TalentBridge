using TalentBridge.Common.Entities;
using TalentBridge.Enums;

namespace TalentBridge.Models;

public class TestAssignment : BaseEntity
{
    public int ApplicationId { get; set; }
    public Application Application { get; set; }
    
    public int TestId { get; set; }
    public Test Test { get; set; }
    
    public int AssignedBy { get; set; }
    public HRManager AssignedByHRManager { get; set; }
    
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public TEST_ASSIGNMENT_STATUS Status { get; set; } = TEST_ASSIGNMENT_STATUS.Assigned;
    public string AccessToken { get; set; } 
    
    
    public TestSubmission? Submission { get; set; }
}