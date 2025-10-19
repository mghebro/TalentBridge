using TalentBridge.Enums.OrganizationTypes;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models.Organizations;

public class NGOOrganization : Organization
{
    public NON_GOV ExactType { get; set; }
}