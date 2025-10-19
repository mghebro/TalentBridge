using TalentBridge.Enums.OrganizationTypes;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models.Organizations;

public class BusinessOrganization : Organization
{
    public BUSINESS_COMPANY ExactType { get; set; }
}