using TalentBridge.Enums.OrganizationTypes;

namespace TalentBridge.Models.Roles;

public class Organization<TExactType>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
    public TYPES Type { get; set; }
    public TExactType ExactType { get; set; }
    public string? Logo { get; set; }
    public string? Website { get; set; }
    public string Description { get; set; }
}
