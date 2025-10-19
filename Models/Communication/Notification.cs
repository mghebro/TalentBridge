using TalentBridge.Common.Entities;
using TalentBridge.Enums;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models;

public class Notification : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    
    public NOTIFICATION_TYPE Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    
    public int? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public string? ActionUrl { get; set; }
}