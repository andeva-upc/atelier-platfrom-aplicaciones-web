using System;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class Obd2Device : IUserAuditableEntity
{
    public Obd2Device()
    {
        Id = null!;
        BranchId = null!;
        MacAddress = string.Empty;
        Status = Obd2DeviceStatus.Available;
    }

    public Obd2Device(BranchId branchId, string macAddress) : this()
    {
        if (string.IsNullOrWhiteSpace(macAddress))
        {
            throw new ArgumentException("iot.error.macAddress.required");
        }

        Id = new Obd2DeviceId(Guid.NewGuid());
        BranchId = branchId;
        MacAddress = macAddress;
        Status = Obd2DeviceStatus.Available;
    }

    public Obd2DeviceId Id { get; private set; }
    public BranchId BranchId { get; private set; }
    public string MacAddress { get; private set; }
    public DateTimeOffset? LastPing { get; private set; }
    public Obd2DeviceStatus Status { get; private set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public long Version { get; set; }

    public void Ping()
    {
        LastPing = DateTimeOffset.UtcNow;
    }

    public void UpdateMacAddress(string macAddress)
    {
        if (string.IsNullOrWhiteSpace(macAddress))
        {
            throw new ArgumentException("iot.error.macAddress.required");
        }
        MacAddress = macAddress;
    }

    public void Link()
    {
        Status = Obd2DeviceStatus.Linked;
    }

    public void Unlink()
    {
        Status = Obd2DeviceStatus.Available;
    }
}
