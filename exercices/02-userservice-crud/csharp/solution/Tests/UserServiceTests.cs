using System.Net;
using System.Text;
using Moq;
using Moq.Protected;
using Xunit;

namespace UserServiceCrud.Tests;

/// <summary>
/// Tests unitaires de l'implémentation de référence de UserService, avec un
/// HttpMessageHandler mocké (Moq) injecté dans un HttpClient fourni par un
/// IHttpClientFactory mocké. Couvre les 4 verbes CRUD, la levée de
/// UserServiceException sur non-2xx, et le scénario de timeout.
/// </summary>
public class UserServiceTests
{
    private static (Mock<IHttpClientFactory> Factory, Mock<HttpMessageHandler> Handler) CreateFactory(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> respond)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns(respond);

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.test.local/")
        };

        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
        return (factoryMock, handlerMock);
    }

    private static Mock<IHttpClientFactory> CreateFactoryReturning(HttpResponseMessage response)
        => CreateFactory((_, _) => Task.FromResult(response)).Factory;

    private static HttpResponseMessage JsonResponse(HttpStatusCode statusCode, string json)
        => new(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

    [Fact]
    public async Task GetByIdAsync_Devrait_Retourner_Utilisateur_Quand_Reponse_200()
    {
        var factory = CreateFactoryReturning(JsonResponse(HttpStatusCode.OK, "{\"id\":1,\"name\":\"Alice\",\"email\":\"alice@example.com\"}"));
        var sut = new UserService(factory.Object);

        var result = await sut.GetByIdAsync(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("Alice", result.Name);
        Assert.Equal("alice@example.com", result.Email);
    }

    [Fact]
    public async Task GetAllAsync_Devrait_Retourner_Liste_Quand_Reponse_200()
    {
        var json = "[{\"id\":1,\"name\":\"Alice\",\"email\":\"alice@example.com\"},{\"id\":2,\"name\":\"Bob\",\"email\":\"bob@example.com\"}]";
        var factory = CreateFactoryReturning(JsonResponse(HttpStatusCode.OK, json));
        var sut = new UserService(factory.Object);

        var result = await sut.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("Bob", result[1].Name);
    }

    [Fact]
    public async Task CreateAsync_Devrait_Retourner_Utilisateur_Cree_Quand_Reponse_201()
    {
        var factory = CreateFactoryReturning(JsonResponse(HttpStatusCode.Created, "{\"id\":3,\"name\":\"Carol\",\"email\":\"carol@example.com\"}"));
        var sut = new UserService(factory.Object);

        var result = await sut.CreateAsync(new User { Name = "Carol", Email = "carol@example.com" });

        Assert.Equal(3, result.Id);
        Assert.Equal("Carol", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_Devrait_Retourner_Utilisateur_Mis_A_Jour_Quand_Reponse_200()
    {
        var factory = CreateFactoryReturning(JsonResponse(HttpStatusCode.OK, "{\"id\":1,\"name\":\"Alice Updated\",\"email\":\"alice@example.com\"}"));
        var sut = new UserService(factory.Object);

        var result = await sut.UpdateAsync(1, new User { Id = 1, Name = "Alice Updated", Email = "alice@example.com" });

        Assert.Equal("Alice Updated", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_Ne_Devrait_Pas_Lever_Quand_Reponse_204()
    {
        var factory = CreateFactoryReturning(new HttpResponseMessage(HttpStatusCode.NoContent));
        var sut = new UserService(factory.Object);

        await sut.DeleteAsync(1);
    }

    [Fact]
    public async Task GetByIdAsync_Devrait_Lever_UserServiceException_Quand_Reponse_404()
    {
        var factory = CreateFactoryReturning(JsonResponse(HttpStatusCode.NotFound, "{\"error\":\"user not found\"}"));
        var sut = new UserService(factory.Object);

        var exception = await Assert.ThrowsAsync<UserServiceException>(() => sut.GetByIdAsync(999));

        Assert.Equal(404, exception.StatusCode);
        Assert.Contains("user not found", exception.ResponseBody);
    }

    [Fact]
    public async Task GetByIdAsync_Devrait_Lever_TimeoutException_Quand_La_Reponse_Est_Trop_Lente()
    {
        // Le handler simule une réponse qui met 5s à arriver ; on injecte un timeout de 50ms
        // pour ne pas ralentir la suite de tests tout en exerçant réellement le chemin de timeout.
        var (factory, _) = CreateFactory(async (_, ct) =>
        {
            await Task.Delay(TimeSpan.FromSeconds(5), ct);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var sut = new UserService(factory.Object, requestTimeout: TimeSpan.FromMilliseconds(50));

        await Assert.ThrowsAsync<TimeoutException>(() => sut.GetByIdAsync(1));
    }
}
