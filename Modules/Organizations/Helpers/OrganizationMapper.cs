using AutoMapper;
using TalentBridge.Models.Organizations;
using TalentBridge.Models.Roles;
using TalentBridge.Modules.Organizations.DTOs.Requests;
using TalentBridge.Modules.Organizations.DTOs.Responses;

namespace TalentBridge.Modules.Organizations;

public class OrganizationMapper : Profile
{
    public OrganizationMapper()
    {
        CreateMap<Organization, OrganizationList>();
        CreateMap<Organization, OrganizationDetails>();
        CreateMap<Organization, OrganizationStatistics>();
        CreateMap<CreateOrganizationRequest, Organization>();
        CreateMap<UpdateOrganizationRequest, Organization>();
        
        CreateMap<CreateOrganizationRequest, BusinessOrganization>();
        CreateMap<CreateOrganizationRequest, EducationOrganization>();
        CreateMap<CreateOrganizationRequest, HealthcareOrganization>();
        CreateMap<CreateOrganizationRequest, NGOOrganization>();
        CreateMap<CreateOrganizationRequest, GOVOrganization>();
        CreateMap<CreateOrganizationRequest, OtherOrganization>();
    }
}