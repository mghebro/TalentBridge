using System.ComponentModel.DataAnnotations.Schema;
using TalentBridge.Enums.OrganizationTypes;

namespace TalentBridge.Models.Roles;

public abstract class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
    public TYPES Type { get; set; }
    public int ExactTypeValue { get; set; }
    public string? Logo { get; set; }
    public string? Website { get; set; }
    public string Description { get; set; }
    [NotMapped]
    public object ExactType
    {
        get => Type switch
        {
            TYPES.BUSINESS_COMPANY => (BUSINESS_COMPANY)ExactTypeValue,
            TYPES.EDUCATION => (EDUCATION)ExactTypeValue,
            TYPES.HEALTHCARE => (HEALTHCARE)ExactTypeValue,
            TYPES.NON_GOV => (NON_GOV)ExactTypeValue,
            TYPES.GOV => (GOV)ExactTypeValue,
            TYPES.OTHERS_ASSOCIATIONS => (OTHERS_ASSOCIATIONS)ExactTypeValue,
            _ => ExactTypeValue
        };
        set => ExactTypeValue = Convert.ToInt32(value);
    }
}
