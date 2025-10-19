using TalentBridge.Common.Entities;
using TalentBridge.Enums;

namespace TalentBridge.Models;

public class Question : BaseEntity
{
    public int TestId { get; set; }
    public Test Test { get; set; }
    
    public string QuestionText { get; set; }
    public QUESTION_TYPE QuestionType { get; set; }
    public decimal Points { get; set; }
    public int OrderNumber { get; set; }
    public int? TimeLimitSeconds { get; set; }
    public bool IsRequired { get; set; } = true;
    public string? Explanation { get; set; }
    
    public List<QuestionOption>? Options { get; set; }
    public List<SubmissionAnswer>? Answers { get; set; }
}