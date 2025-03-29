using System.Reflection;

namespace Frenet.Logistic.Domain.Shared;

/*
 * implementação do padrão "Smart Enum" (ou Enumeração Inteligente) 
 * e serve como uma alternativa mais robusta às enumerações tradicionais do C# (enum). 
 * Ela é usada para criar tipos enumerados que possuem mais funcionalidades e flexibilidade,
 * permitindo que você associe propriedades e métodos a cada valor enumerado.
 */
/// <summary>
/// Representa uma enumeração de objetos com um identificador numérico único e um nome.
/// </summary>
/// <typeparam name="TEnum">O tipo da enumeração.</typeparam>
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Lazy<Dictionary<int, TEnum>> EnumerationsDictionary =
        new(() => CreateEnumerationDictionary(typeof(TEnum)));

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Enumeration{TEnum}"/>.
    /// </summary>
    /// <param name="id">O identificador da enumeração.</param>
    /// <param name="name">O nome da enumeração.</param>
    protected Enumeration(int id, string name)
        : this()
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Enumeration{TEnum}"/>.
    /// </summary>
    /// <remarks>
    /// Necessário para desserialização.
    /// </remarks>
    protected Enumeration() => Name = string.Empty;

    /// <summary>
    /// Obtém o identificador.
    /// </summary>
    public int Id { get; protected init; }

    /// <summary>
    /// Obtém o nome.
    /// </summary>
    public string Name { get; protected init; }

    public static bool operator ==(Enumeration<TEnum>? a, Enumeration<TEnum>? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Enumeration<TEnum> a, Enumeration<TEnum> b) => !(a == b);

    /// <summary>
    /// Obtém os valores da enumeração.
    /// </summary>
    /// <returns>A coleção somente leitura de valores da enumeração.</returns>
    public static IReadOnlyCollection<TEnum> GetValues() => EnumerationsDictionary.Value.Values.ToList();

    /// <summary>
    /// Cria uma enumeração do tipo especificado com base no identificador fornecido.
    /// </summary>
    /// <param name="id">O identificador da enumeração.</param>
    /// <returns>A instância da enumeração que corresponde ao identificador especificado, se existir.</returns>
    public static TEnum? FromId(int id) => EnumerationsDictionary.Value.TryGetValue(id, out TEnum? enumeration) ? enumeration : null;

    /// <summary>
    /// Cria uma enumeração do tipo especificado com base no nome fornecido.
    /// </summary>
    /// <param name="name">O nome da enumeração.</param>
    /// <returns>A instância da enumeração que corresponde ao nome especificado, se existir.</returns>
    public static TEnum? FromName(string name) => EnumerationsDictionary.Value.Values.SingleOrDefault(x => x.Name == name);

    /// <summary>
    /// Verifica se a enumeração com o identificador especificado existe.
    /// </summary>
    /// <param name="id">O identificador da enumeração.</param>
    /// <returns>True se uma enumeração com o identificador especificado existir, caso contrário, false.</returns>
    public static bool Contains(int id) => EnumerationsDictionary.Value.ContainsKey(id);

    /// <inheritdoc />
    public virtual bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() && other.Id.Equals(Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        return obj is Enumeration<TEnum> otherValue && otherValue.Id.Equals(Id);
    }

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode() * 37;

    private static Dictionary<int, TEnum> CreateEnumerationDictionary(Type enumType) => GetFieldsForType(enumType).ToDictionary(t => t.Id);

    private static IEnumerable<TEnum> GetFieldsForType(Type enumType) =>
        enumType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);
}
