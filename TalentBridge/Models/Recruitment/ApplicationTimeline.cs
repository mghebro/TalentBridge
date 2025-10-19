using TalentBridge.Common.Entities;
using TalentBridge.Enums;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models;

public class ApplicationTimeline : BaseEntity
{
    public int ApplicationId { get; set; }
    public Application Application { get; set; }
    
    public APPLICATION_STATUS Status { get; set; }
    
    public int ChangedBy { get; set; }
    public User ChangedByUser { get; set; }
    
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}