using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TalentBridge.Common.DTOs.Responses;
using TalentBridge.Common.Services;
using TalentBridge.Data;
using TalentBridge.Enums;
using TalentBridge.Enums.Auth;
using TalentBridge.Enums.OrganizationTypes;
using TalentBridge.Models.Organizations;
using TalentBridge.Models.Roles;
using TalentBridge.Modules.Organizations.DTOs.Requests;
using TalentBridge.Modules.Organizations.DTOs.Responses;

namespace TalentBridge.Modules.Organizations.Services;

public class OrganizationServices : IOrganizationServices
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IFileService _fileService; 
    private readonly ILogger<OrganizationServices> _logger;

    public OrganizationServices(
        IMapper mapper, 
        DataContext context,
        IFileService fileService,
        ILogger<OrganizationServices> logger)
    {
        _mapper = mapper;
        _context = context;
        _fileService = fileService;
        _logger = logger;
    }

    
   public async Task<ServiceResult<OrganizationDetails>> CreateOrganizationAsync(
    CreateOrganizationRequest dto, 
    IFormFile logoFile, 
    int createdByUserId)
{
    var user = await _context.Users.FindAsync(createdByUserId);
    if (user == null)
        return ServiceResult<OrganizationDetails>.FailureResult("User not found");

    var existingOrg = await _context.Organizations
        .FirstOrDefaultAsync(o => o.Name.ToLower() == dto.Name.ToLower());

    if (existingOrg != null)
        return ServiceResult<OrganizationDetails>.FailureResult("Organization with this name already exists");

    Organization organization = dto.Type switch
    {
        TYPES.BUSINESS_COMPANY => _mapper.Map<BusinessOrganization>(dto),
        TYPES.EDUCATION => _mapper.Map<EducationOrganization>(dto),
        TYPES.HEALTHCARE => _mapper.Map<HealthcareOrganization>(dto),
        TYPES.NON_GOV => _mapper.Map<NGOOrganization>(dto),
        TYPES.GOV => _mapper.Map<GOVOrganization>(dto),
        TYPES.OTHERS_ASSOCIATIONS => _mapper.Map<OtherOrganization>(dto),
        _ => throw new InvalidOperationException("Unknown organization type")
    };

    if (logoFile != null)
    {
        if (logoFile.Length > 2_000_000)
            return ServiceResult<OrganizationDetails>.FailureResult("Logo file is too large");

        var ext = Path.GetExtension(logoFile.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".svg" };
        if (!allowedExtensions.Contains(ext))
            return ServiceResult<OrganizationDetails>.FailureResult("Invalid logo file type");

        using var memoryStream = new MemoryStream();
        await logoFile.CopyToAsync(memoryStream);
        organization.Logo = memoryStream.ToArray(); 
    }

    organization.UserId = createdByUserId;
    organization.IsActive = true;

    _context.Organizations.Add(organization);
    await _context.SaveChangesAsync();

    var organizationDetails = await GetOrganizationDetailsWithStats(organization.Id);

    _logger.LogInformation($"Organization created: {organization.Name} (ID: {organization.Id})");

    return ServiceResult<OrganizationDetails>.SuccessResult(
        organizationDetails,
        "Organization created successfully"
    );
}

    

    
    public async Task<ServiceResult<PaginatedResult<OrganizationList>>> GetOrganizationsAsync(
        OrganizationFilterRequest request)
    {
        
            var query = _context.Organizations.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(o => 
                    o.Name.Contains(request.Search) || 
                    o.Description.Contains(request.Search) ||
                    o.Address.Contains(request.Search)
                );
            }

            if (request.Type.HasValue)
            {
                query = query.Where(o => o.Type == request.Type.Value);
            }

            if (request.ExactTypeValue.HasValue)
            {
                query = query.Where(o => o.ExactTypeValue == request.ExactTypeValue.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Location))
            {
                query = query.Where(o => o.Address.Contains(request.Location));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(o => o.IsActive == request.IsActive.Value);
            }

            var totalItems = await query.CountAsync();

            query = request.SortBy?.ToLower() switch
            {
                "name" => request.SortOrder?.ToLower() == "asc" 
                    ? query.OrderBy(o => o.Name) 
                    : query.OrderByDescending(o => o.Name),
                "createddate" => request.SortOrder?.ToLower() == "asc" 
                    ? query.OrderBy(o => o.CreatedAt) 
                    : query.OrderByDescending(o => o.CreatedAt),
                _ => query.OrderByDescending(o => o.CreatedAt)
            };

            var organizations = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var organizationDtos = new List<OrganizationList>();
            foreach (var org in organizations)
            {
                var dto = _mapper.Map<OrganizationList>(org);
                
                dto.ActiveVacancies = await _context.Vacancies
                    .CountAsync(v => v.OrganizationId == org.Id && v.Status == VACANCY_STATUS.Active);
                
                dto.TotalApplications = await _context.Applications
                    .CountAsync(a => a.Vacancy.OrganizationId == org.Id);
                
                organizationDtos.Add(dto);
            }

            var paginatedResult = new PaginatedResult<OrganizationList>
            {
                Items = organizationDtos,
                TotalItems = totalItems,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return ServiceResult<PaginatedResult<OrganizationList>>.SuccessResult(paginatedResult);
        }
       


    
    public async Task<ServiceResult<OrganizationDetails>> GetOrganizationByIdAsync(int id)
    {
        
            var organization = await _context.Organizations
                .FirstOrDefaultAsync(o => o.Id == id);

            if (organization == null)
            {
                return ServiceResult<OrganizationDetails>.FailureResult("Organization not found");
            }

            var organizationDetails = await GetOrganizationDetailsWithStats(id);

            return ServiceResult<OrganizationDetails>.SuccessResult(organizationDetails);
       
    }
    

    
   public async Task<ServiceResult<OrganizationDetails>> UpdateOrganizationAsync(
    int id, 
    UpdateOrganizationRequest dto, 
    IFormFile? logoFile, 
    int userId)
{
    var organization = await _context.Organizations
        .FirstOrDefaultAsync(o => o.Id == id);

    if (organization == null)
        return ServiceResult<OrganizationDetails>.FailureResult("Organization not found");

    if (!await UserCanManageOrganizationAsync(userId, id))
        return ServiceResult<OrganizationDetails>.FailureResult("You don't have permission to update this organization");

    if (!string.Equals(dto.Name, organization.Name, StringComparison.OrdinalIgnoreCase))
    {
        var existingOrg = await _context.Organizations
            .FirstOrDefaultAsync(o => o.Name.ToLower() == dto.Name.ToLower() && o.Id != id);

        if (existingOrg != null)
            return ServiceResult<OrganizationDetails>.FailureResult("Organization with this name already exists");
    }

    if (logoFile != null)
    {
        if (logoFile.Length > 2_000_000)
            return ServiceResult<OrganizationDetails>.FailureResult("Logo file is too large");

        var ext = Path.GetExtension(logoFile.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".svg" };
        if (!allowedExtensions.Contains(ext))
            return ServiceResult<OrganizationDetails>.FailureResult("Invalid logo file type");

        using var memoryStream = new MemoryStream();
        await logoFile.CopyToAsync(memoryStream);

        organization.Logo = memoryStream.ToArray(); 
    }
    else if (dto.DeleteLogo && organization.Logo != null)
    {
        organization.Logo = null;
    }

    _mapper.Map(dto, organization);
    organization.UpdatedAt = DateTime.UtcNow;

    _context.Organizations.Update(organization);
    await _context.SaveChangesAsync();

    var organizationDetails = await GetOrganizationDetailsWithStats(id);
    _logger.LogInformation($"Organization updated: {organization.Name} (ID: {id})");

    return ServiceResult<OrganizationDetails>.SuccessResult(
        organizationDetails,
        "Organization updated successfully"
    );
}

    

    
    public async Task<ServiceResult<string>> DeleteOrganizationAsync(int id, int userId)
    {
      
            var organization = await _context.Organizations
                .FirstOrDefaultAsync(o => o.Id == id);

            if (organization == null)
            {
                return ServiceResult<string>.FailureResult("Organization not found");
            }

            if (!await UserCanManageOrganizationAsync(userId, id))
            {
                return ServiceResult<string>.FailureResult(
                    "You don't have permission to delete this organization"
                );
            }

            var hasActiveVacancies = await _context.Vacancies
                .AnyAsync(v => v.OrganizationId == id && v.Status == VACANCY_STATUS.Active);

            if (hasActiveVacancies)
            {
                return ServiceResult<string>.FailureResult(
                    "Cannot delete organization with active vacancies"
                );
            }

            organization.IsActive = false;
            organization.IsDeleted = true;
            organization.DeletedAt = DateTime.UtcNow;

            _context.Organizations.Update(organization);
            await _context.SaveChangesAsync();


            return ServiceResult<string>.SuccessResult(
                "Organization deleted successfully",
                "Organization has been successfully deleted"
            );
       
    }
    
    
    
    private async Task<OrganizationDetails> GetOrganizationDetailsWithStats(int organizationId)
    {
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.Id == organizationId);

        var details = _mapper.Map<OrganizationDetails>(organization);

       // Step 1: Fetch completed test scores
var completedTests = await _context.TestAssignments
    .Where(ts => ts.Test.Vacancy.OrganizationId == organizationId &&
                 ts.Status == TEST_ASSIGNMENT_STATUS.Completed &&
                 ts.Submission != null &&
                 ts.Submission.PercentageScore.HasValue)
    .Select(ts => ts.Submission.PercentageScore.Value)
    .ToListAsync(); 

details.Statistics = new OrganizationStatistics
{
    TotalVacancies = await _context.Vacancies
        .CountAsync(v => v.OrganizationId == organizationId),
    
    ActiveVacancies = await _context.Vacancies
        .CountAsync(v => v.OrganizationId == organizationId && v.Status == VACANCY_STATUS.Active),
    
    ClosedVacancies = await _context.Vacancies
        .CountAsync(v => v.OrganizationId == organizationId && v.Status == VACANCY_STATUS.Closed),
    
    TotalApplications = await _context.Applications
        .CountAsync(a => a.Vacancy.OrganizationId == organizationId),
    
    PendingApplications = await _context.Applications
        .CountAsync(a => a.Vacancy.OrganizationId == organizationId && 
            a.Status == APPLICATION_STATUS.Pending),
    
    ReviewedApplications = await _context.Applications
        .CountAsync(a => a.Vacancy.OrganizationId == organizationId && 
            a.Status == APPLICATION_STATUS.UnderReview),
    
    TotalTests = await _context.Tests
        .CountAsync(t => t.Vacancy.OrganizationId == organizationId),
    
    AverageTestScore = completedTests.Any() 
        ? completedTests.Average() 
        : 0, 
    
    TotalHires = await _context.Applications
        .CountAsync(a => a.Vacancy.OrganizationId == organizationId && 
            a.Status == APPLICATION_STATUS.Accepted),
    
    LastVacancyPosted = await _context.Vacancies
        .Where(v => v.OrganizationId == organizationId)
        .OrderByDescending(v => v.CreatedAt)
        .Select(v => (DateTime?)v.CreatedAt)
        .FirstOrDefaultAsync(),
    
    LastApplicationReceived = await _context.Applications
        .Where(a => a.Vacancy.OrganizationId == organizationId)
        .OrderByDescending(a => a.CreatedAt)
        .Select(a => (DateTime?)a.CreatedAt)
        .FirstOrDefaultAsync()
};

        var hiredApplications = await _context.Applications
            .Where(a => a.Vacancy.OrganizationId == organizationId && 
                a.Status == APPLICATION_STATUS.Accepted && a.HiredAt.HasValue)
            .Select(a => new { a.AppliedAt, a.HiredAt })
            .ToListAsync();

        if (hiredApplications.Any())
        {
            details.Statistics.AverageTimeToHire = (decimal)hiredApplications.Average(a => (a.HiredAt.Value - a.AppliedAt).TotalDays);
        }

        return details;
    }

    private async Task<bool> UserCanManageOrganizationAsync(int userId, int organizationId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user?.Role == ROLES.ORGANIZATION_ADMIN)
        {
            return true;
        }

        var organization = await _context.Organizations.FindAsync(organizationId);
        if (organization?.UserId == userId)
        {
            return true;
        }

        var isOrgHR = await _context.HrManagers
            .AnyAsync(oh => oh.UserId == userId && oh.OrganizationId == organizationId && oh.IsActive);

        return isOrgHR;
    }
    
}