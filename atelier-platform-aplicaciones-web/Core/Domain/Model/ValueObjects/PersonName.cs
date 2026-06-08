using System;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record PersonName
{
    public PersonName()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public PersonName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("core.error.firstName.notBlank");
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("core.error.lastName.notBlank");
        }
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; init; }
    public string LastName { get; init; }

    public string FullName => $"{FirstName} {LastName}";
}
