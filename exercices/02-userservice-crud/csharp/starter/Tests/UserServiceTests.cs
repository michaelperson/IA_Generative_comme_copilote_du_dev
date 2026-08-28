using System.Net;
using System.Text;
using Moq;
using Moq.Protected;
using Xunit;

namespace UserServiceCrud.Tests;

/// <summary>
/// Tests unitaires de UserService, avec un HttpMessageHandler mocké (Moq) injecté
/// dans un HttpClient fourni par un IHttpClientFactory mocké.
/// Tant que UserService n'est pas implémenté (voir les TODO), ces tests échouent.
/// </summary>
public class UserServiceTests
{
    private static Mock<IHttpClientFactory> CreateFactory(HttpResponseMessage response)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.test.local/")
        };

        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
        return factoryMock;
    }

    private static HttpResponseMessage JsonResponse(HttpStatusCode statusCode, string json)
        => new(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

    [Fact]
    public async Task GetByIdAsync_Devrait_Retourner_Utilisateur_Quand_Reponse_200()
    {
        var factory = CreateFactory(JsonResponse(HttpStatusCode.OK, "{\"id\":1,\"name\":\"Alice\",\"email\":\"alice@example.com\"}"));
        var sut = new UserService(factory.Object);

        var result = await sut.GetByIdAsync(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("Alice", result.Name);
        Assert.Equal("alice@example.com", result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_Devrait_Lever_UserServiceException_Quand_Reponse_404()
    {
        var factory = CreateFactory(JsonResponse(HttpStatusCode.NotFound, "{\"error\":\"user not found\"}"));
        var sut = new UserService(factory.Object);

        var exception = await Assert.ThrowsAsync<UserServiceException>(() => sut.GetByIdAsync(999));

        Assert.Equal(404, exception.StatusCode);
        Assert.Contains("user not found", exception.ResponseBody);
    }
}
