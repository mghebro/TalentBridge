using TalentBridge.Common.Entities;
using TalentBridge.Enums;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models;

public class Vacancy : BaseEntity
{
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
    
    public int CreatedBy { get; set; }
    public HRManager CreatedByHRManager { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
    public string Responsibilities { get; set; }
    public string Profession { get; set; }
    public string Industry { get; set; }
    
    public EMPLOYMENT_TYPE EmploymentType { get; set; }
    public EXPERIENCE_LEVEL ExperienceLevel { get; set; }
    
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string SalaryCurrency { get; set; } = "GEL";
    
    public string Location { get; set; }
    public bool IsRemote { get; set; } = false;
    
    public VACANCY_STATUS Status { get; set; } = VACANCY_STATUS.Draft;
    public DateTime ApplicationDeadline { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; } = 0;
    
    public List<Application>? Applications { get; set; }
    public List<SavedVacancy>? SavedByUsers { get; set; }
    public List<VacancyView>? Views { get; set; }
}