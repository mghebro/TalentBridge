﻿using Microsoft.EntityFrameworkCore;
using TalentBridge.Enums.OrganizationTypes;
using TalentBridge.Models;
using TalentBridge.Models.Auth;
using TalentBridge.Models.Organizations;
using TalentBridge.Models.Roles;
using TalentBridge.Models.UserRelated;

namespace TalentBridge.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    // Auth
    public DbSet<User> Users { get; set; }
    public DbSet<EmailVerification> EmailVerifications { get; set; }
    public DbSet<PasswordVerification> PasswordVerifications { get; set; }
    
    // Roles
    public DbSet<HRManager> HrManagers { get; set; }
    
    //Communication
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    // Recruitment
    public DbSet<Application> Applications { get; set; }
    public DbSet<ApplicationTimeline> ApplicationTimelines { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<SavedVacancy> SavedVacancies { get; set; }
    
    //testing
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestAssignment> TestAssignments { get; set; }
    public DbSet<SubmissionAnswer> SubmissionAnswers { get; set; }
    public DbSet<TestSubmission> TestSubmissions { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    
    //UserRelated
    public DbSet<Education> Educations { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<UserDetails> UserDetails { get; set; }
    
    //Analytics
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<VacancyView> VacancyViews { get; set; }
    
    //Organizations
    public DbSet<Organization> Organizations { get; set; } 
    public DbSet<BusinessOrganization> BusinessOrganizations { get; set; }
    public DbSet<EducationOrganization> EducationOrganizations { get; set; }
    public DbSet<HealthcareOrganization> HealthcareOrganizations { get; set; }
    public DbSet<NGOOrganization> NonGovOrganizations { get; set; }
    public DbSet<GOVOrganization> GovOrganizations { get; set; }
    public DbSet<OtherOrganization> OtherOrganizations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Organization>()
            .HasDiscriminator<TYPES>(o => o.Type)
            .HasValue<BusinessOrganization>(TYPES.BUSINESS_COMPANY)
            .HasValue<EducationOrganization>(TYPES.EDUCATION)
            .HasValue<HealthcareOrganization>(TYPES.HEALTHCARE)
            .HasValue<NGOOrganization>(TYPES.NON_GOV)
            .HasValue<GOVOrganization>(TYPES.GOV)
            .HasValue<OtherOrganization>(TYPES.OTHERS_ASSOCIATIONS);
        
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}