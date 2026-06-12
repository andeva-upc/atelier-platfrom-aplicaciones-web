using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Operations.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyOperationsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Service>().HasQueryFilter(s => s.DeletedAt == null);
        builder.Entity<WorkOrder>().HasQueryFilter(w => w.DeletedAt == null);
        builder.Entity<WorkOrderTask>().HasQueryFilter(t => t.DeletedAt == null);
        builder.Entity<WorkOrderTaskProduct>().HasQueryFilter(p => p.DeletedAt == null);

        builder.Entity<Service>(entity =>
        {
            entity.ToTable("services");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new ServiceId(v))
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);

            entity.Property(e => e.Price)
                .HasConversion(v => v.Amount, v => new Money(v))
                .IsRequired();

            entity.Property(e => e.Version).IsConcurrencyToken();
        });

        builder.Entity<WorkOrderTaskProduct>(entity =>
        {
            entity.ToTable("work_order_task_products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id)
                .HasConversion(vo => vo.Value, value => new WorkOrderTaskProductId(value))
                .IsRequired()
                .ValueGeneratedNever();
            entity.Property(p => p.ProductId)
                .HasConversion(vo => vo.Value, value => new ProductId(value))
                .IsRequired();
            entity.Property(p => p.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
            entity.Property(p => p.Quantity)
                .HasConversion(vo => vo.Value, value => new Quantity(value))
                .IsRequired();
            entity.Property(p => p.UnitPrice)
                .HasConversion(vo => vo.Amount, value => new Money(value))
                .IsRequired();
            entity.Property(p => p.TotalAmount)
                .HasConversion(vo => vo.Amount, value => new Money(value))
                .IsRequired();
            entity.Property(p => p.Version).IsConcurrencyToken();
        });
        
        builder.Entity<WorkOrderTask>(entity =>
        {
            entity.ToTable("work_order_tasks");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id)
                .HasConversion(vo => vo.Value, value => new WorkOrderTaskId(value))
                .IsRequired()
                .ValueGeneratedNever();
            entity.Property(t => t.ServiceId)
                .HasConversion(vo => vo.Value, value => new ServiceId(value))
                .IsRequired();
            entity.Property(t => t.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
            entity.Property(t => t.AssignedMechanicId)
                .HasConversion(vo => vo.Value, value => new MechanicId(value))
                .IsRequired();
            entity.Property(t => t.Description)
                .HasConversion(vo => vo.Value, value => new TaskDescription(value))
                .IsRequired()
                .HasColumnType("TEXT");
            entity.Property(t => t.Price)
                .HasConversion(vo => vo.Amount, value => new Money(value))
                .IsRequired();
            entity.Property(t => t.Status)
                .HasConversion(
                    status => status == WorkOrderTaskStatus.Pending ? "PENDING" :
                        status == WorkOrderTaskStatus.Doing ? "DOING" :
                        status == WorkOrderTaskStatus.Completed ? "COMPLETED" : "PENDING",
                    value => value.ToUpper() == "PENDING" ? WorkOrderTaskStatus.Pending :
                        value.ToUpper() == "DOING" ? WorkOrderTaskStatus.Doing :
                        value.ToUpper() == "COMPLETED" ? WorkOrderTaskStatus.Completed : WorkOrderTaskStatus.Pending)
                .IsRequired();
            entity.HasMany(t => t.Products)
                .WithOne()
                .HasForeignKey("WorkOrderTaskId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(p => p.Version).IsConcurrencyToken();
        });
        
        builder.Entity<WorkOrder>(entity =>
        {
            entity.ToTable("work_orders");
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Id)
                .HasConversion(vo => vo.Value, value => new WorkOrderId(value))
                .IsRequired()
                .ValueGeneratedNever();
            entity.Property(w => w.AppointmentId)
                .HasConversion(vo => vo.Value, value => new AppointmentId(value))
                .IsRequired();
            entity.Property(w => w.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
            entity.Property(w => w.VehicleId)
                .HasConversion(vo => vo.Value, value => new VehicleId(value))
                .IsRequired();
            entity.Property(w => w.CustomerId)
                .HasConversion(vo => vo.Value, value => new CustomerId(value))
                .IsRequired();
            entity.Property(w => w.InternalNumber)
                .IsRequired();
            entity.Property(w => w.DiagnosticSummary)
                .HasConversion(vo => vo.Value, value => new DiagnosticSummary(value))
                .IsRequired()
                .HasColumnType("TEXT");
            entity.Property(w => w.MileageIn)
                .HasConversion(vo => vo.Value, value => new Mileage(value))
                .IsRequired();
            entity.Property(w => w.TotalAmount)
                .HasConversion(vo => vo.Amount, value => new Money(value))
                .IsRequired();
            entity.Property(w => w.Status)
                .HasConversion(
                    status => status == WorkOrderStatus.Pending ? "PENDING" :
                        status == WorkOrderStatus.InProgress ? "IN_PROGRESS" :
                        status == WorkOrderStatus.Completed ? "COMPLETED" :
                        status == WorkOrderStatus.Paid ? "PAID" : "PENDING",
                    value => value.ToUpper() == "PENDING" ? WorkOrderStatus.Pending :
                        value.ToUpper() == "IN_PROGRESS" ? WorkOrderStatus.InProgress :
                        value.ToUpper() == "COMPLETED" ? WorkOrderStatus.Completed :
                        value.ToUpper() == "PAID" ? WorkOrderStatus.Paid : WorkOrderStatus.Pending)
                .IsRequired();
            entity.HasMany(w => w.Tasks)
                .WithOne()
                .HasForeignKey("WorkOrderId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(p => p.Version).IsConcurrencyToken();
        });
    }
}
