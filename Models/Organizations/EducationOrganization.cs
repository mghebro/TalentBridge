using TalentBridge.Enums.OrganizationTypes;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models.Organizations;

public class EducationOrganization : Organization
{
    public EDUCATION ExactType { get; set; }
}