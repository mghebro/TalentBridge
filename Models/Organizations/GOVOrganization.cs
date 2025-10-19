using TalentBridge.Enums.OrganizationTypes;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models.Organizations;

public class GOVOrganization : Organization
{
    public GOV ExactType { get; set; }
}