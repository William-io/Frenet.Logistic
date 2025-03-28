using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Shared;

namespace Frenet.Logistic.Domain.Customers;

public sealed class Email
{
    public const int MaxLength = 255;

    private Email(string value) => Value = value;

    private Email()
    {
    }

    public string Value { get; private set; }

    public static Result<Email> Create(string email) =>
        Result.Create(email)
            .Ensure(
                e => !string.IsNullOrWhiteSpace(e),
                Error.Email.Empty)
            .Ensure(
                e => e.Length <= MaxLength,
                Error.Email.TooLong)
            .Ensure(
                e => e.Split('@').Length == 2,
                Error.Email.InvalidFormat)
            .Map(e => new Email(e));
}