using TalentBridge.Enums.OrganizationTypes;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models.Organizations;

public class HealthcareOrganization : Organization
{
    public HEALTHCARE ExactType { get; set; }
}