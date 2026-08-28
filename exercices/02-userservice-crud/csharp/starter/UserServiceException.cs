namespace UserServiceCrud;

/// <summary>
/// Exception typée levée par <see cref="UserService"/> lorsque l'API /users répond
/// avec un code de statut HTTP non-2xx. Porte le code de statut et le corps de la
/// réponse pour permettre à l'appelant de diagnostiquer l'erreur sans avoir à
/// intercepter une exception HTTP générique.
/// </summary>
public class UserServiceException : Exception
{
    /// <summary>Code de statut HTTP renvoyé par l'API (ex: 404, 500).</summary>
    public int StatusCode { get; }

    /// <summary>Corps brut de la réponse HTTP, tel que renvoyé par l'API.</summary>
    public string ResponseBody { get; }

    public UserServiceException(int statusCode, string responseBody)
        : base($"L'API /users a répondu avec le code HTTP {statusCode}.")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
