using TalentBridge.Common.Entities;

namespace TalentBridge.Models;

public class QuestionOption : BaseEntity
{
    public int QuestionId { get; set; }
    public Question Question { get; set; }
    
    public string OptionText { get; set; }
    public bool IsCorrect { get; set; }
    public int OrderNumber { get; set; }
}