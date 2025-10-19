using TalentBridge.Enums.OrganizationTypes;
using TalentBridge.Models.Roles;

namespace TalentBridge.Models.Organizations;

public class OtherOrganization : Organization
{
    public OTHERS_ASSOCIATIONS ExactType { get; set; }
}