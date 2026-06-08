using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Core.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyCoreConfiguration(this ModelBuilder builder)
    {
        // 1. Query Filters for Soft Delete
        builder.Entity<Branch>().HasQueryFilter(b => b.DeletedAt == null);
        builder.Entity<Customer>().HasQueryFilter(c => c.DeletedAt == null);
        builder.Entity<Employee>().HasQueryFilter(e => e.DeletedAt == null);
        builder.Entity<Owner>().HasQueryFilter(o => o.DeletedAt == null);
        builder.Entity<Workshop>().HasQueryFilter(w => w.DeletedAt == null);

        // 2. Branch Mapping
        builder.Entity<Branch>(entity =>
        {
            entity.ToTable("branches");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.WorkshopId)
                .HasConversion(v => v.Value, v => new WorkshopId(v))
                .IsRequired();

            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            entity.Property(e => e.Address)
                .HasConversion(v => v.Value, v => new Address(v))
                .IsRequired();

            entity.Property(e => e.Phone)
                .HasConversion(v => v.Value, v => new Phone(v))
                .IsRequired();
        });

        // 3. Customer Mapping
        builder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new CustomerId(v))
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.UserId)
                .HasConversion(v => v.Value, v => new UserId(v))
                .IsRequired();

            entity.Property(e => e.IsCorporate).IsRequired();
            entity.Property(e => e.BusinessName).HasMaxLength(150);

            entity.OwnsOne(e => e.Name, name =>
            {
                name.Property("CustomerId").HasColumnName("id");
                name.Property(n => n.FirstName).HasColumnName("first_name").HasMaxLength(100);
                name.Property(n => n.LastName).HasColumnName("last_name").HasMaxLength(100);
            });

            entity.OwnsOne(e => e.Document, doc =>
            {
                doc.Property("CustomerId").HasColumnName("id");
                doc.Property(d => d.DocumentType).HasColumnName("document_type").IsRequired()
                    .HasConversion(
                        v => v.ToString().ToUpper(),
                        v => (DocumentType)Enum.Parse(typeof(DocumentType), v, true)
                    );
                doc.Property(d => d.DocumentNumber).HasColumnName("document_number").IsRequired().HasMaxLength(50);
            });

            entity.Property(e => e.Phone)
                .HasConversion(v => v.Value, v => new Phone(v))
                .IsRequired();

            entity.HasIndex(e => e.UserId).IsUnique();
        });

        // 4. Employee Mapping
        builder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new EmployeeId(v))
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.UserId)
                .HasConversion(v => v.Value, v => new UserId(v))
                .IsRequired();

            entity.OwnsOne(e => e.Name, name =>
            {
                name.Property("EmployeeId").HasColumnName("id");
                name.Property(n => n.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
                name.Property(n => n.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
            });

            entity.OwnsOne(e => e.Document, doc =>
            {
                doc.Property("EmployeeId").HasColumnName("id");
                doc.Property(d => d.DocumentType).HasColumnName("document_type").IsRequired()
                    .HasConversion(
                        v => v.ToString().ToUpper(),
                        v => (DocumentType)Enum.Parse(typeof(DocumentType), v, true)
                    );
                doc.Property(d => d.DocumentNumber).HasColumnName("document_number").IsRequired().HasMaxLength(50);
            });

            entity.Property(e => e.Phone)
                .HasConversion(v => v.Value, v => new Phone(v))
                .IsRequired();

            entity.HasIndex(e => e.UserId).IsUnique();
        });

        // 5. Owner Mapping
        builder.Entity<Owner>(entity =>
        {
            entity.ToTable("owners");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new OwnerId(v))
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.UserId)
                .HasConversion(v => v.Value, v => new UserId(v))
                .IsRequired();

            entity.OwnsOne(e => e.Name, name =>
            {
                name.Property("OwnerId").HasColumnName("id");
                name.Property(n => n.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
                name.Property(n => n.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
            });

            entity.OwnsOne(e => e.Document, doc =>
            {
                doc.Property("OwnerId").HasColumnName("id");
                doc.Property(d => d.DocumentType).HasColumnName("document_type").IsRequired()
                    .HasConversion(
                        v => v.ToString().ToUpper(),
                        v => (DocumentType)Enum.Parse(typeof(DocumentType), v, true)
                    );
                doc.Property(d => d.DocumentNumber).HasColumnName("document_number").IsRequired().HasMaxLength(50);
            });

            entity.Property(e => e.Phone)
                .HasConversion(v => v.Value, v => new Phone(v))
                .IsRequired();

            entity.HasIndex(e => e.UserId).IsUnique();
        });

        // 6. Workshop Mapping
        builder.Entity<Workshop>(entity =>
        {
            entity.ToTable("workshops");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new WorkshopId(v))
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.OwnerId)
                .HasConversion(v => v.Value, v => new OwnerId(v))
                .IsRequired();

            entity.Property(e => e.BusinessName).IsRequired().HasMaxLength(150);
            entity.Property(e => e.BrandName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.TaxId).IsRequired().HasMaxLength(20);
            entity.Property(e => e.MileageIntervalConfig).IsRequired();
        });

        // 7. SubscriptionPlan Mapping
        builder.Entity<SubscriptionPlan>(entity =>
        {
            entity.ToTable("subscription_plans");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new SubscriptionPlanId(v))
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.MonthlyPrice).IsRequired();
            entity.Property(e => e.MaxObd2Devices).IsRequired();
            entity.Property(e => e.MaxMonthlySnapshotsPerVehicle).IsRequired();
            entity.Property(e => e.MaxCustomers).IsRequired();
            entity.Property(e => e.MaxStaffAccounts).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
        });

        // 8. BranchSubscription Mapping
        builder.Entity<BranchSubscription>(entity =>
        {
            entity.ToTable("branch_subscriptions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new BranchSubscriptionId(v))
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.PlanId)
                .HasConversion(v => v.Value, v => new SubscriptionPlanId(v))
                .IsRequired();

            entity.Property(e => e.Status).IsRequired()
                .HasConversion(
                    v => v.ToString().ToUpper(),
                    v => (SubscriptionStatus)Enum.Parse(typeof(SubscriptionStatus), v, true)
                );

            entity.Property(e => e.BillingCycle).IsRequired()
                .HasConversion(
                    v => v.ToString().ToUpper(),
                    v => (BillingCycle)Enum.Parse(typeof(BillingCycle), v, true)
                );

            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.EndDate).IsRequired();
            entity.Property(e => e.CanceledAt);
        });
    }
}
