using System.Text.Json.Serialization;

namespace UserServiceCrud;

/// <summary>
/// Représente un utilisateur tel qu'exposé par l'API REST externe /users.
/// Le modèle utilise le PascalCase idiomatique C#, la sérialisation JSON
/// (camelCase côté API) est gérée par les options de <see cref="System.Text.Json.JsonSerializer"/>
/// configurées dans <see cref="UserService"/>.
/// </summary>
public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
