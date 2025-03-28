namespace Frenet.Logistic.Domain.Abstractions;

public record Error(string Code, string Name)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "Valor nulo foi fornecido");


    public static class Email
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email está vazio");

        public static readonly Error TooLong = new(
            "Email.TooLong",
            "Email é muito longo");

        public static readonly Error InvalidFormat = new(
            "Email.InvalidFormat",
            "Email formato inválido");

        public static readonly Error InvalidCredentials = new(
            "Member.InvalidCredentials",
            "Credencial invalida");
    }
}
