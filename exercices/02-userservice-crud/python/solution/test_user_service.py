"""
Tests unitaires de UserService, avec un httpx.MockTransport (aucune
dépendance supplémentaire, aucun appel réseau réel) simulant l'API /users.
"""
import httpx
import pytest

from user_service import User, UserService, UserServiceError, UserServiceTimeoutError


def make_client(handler) -> httpx.AsyncClient:
    transport = httpx.MockTransport(handler)
    return httpx.AsyncClient(transport=transport, base_url="http://api.example.test")


@pytest.mark.asyncio
async def test_get_by_id_succes():
    def handler(request: httpx.Request) -> httpx.Response:
        assert request.url.path == "/users/1"
        return httpx.Response(200, json={"id": 1, "name": "Alice", "email": "alice@example.com"})

    async with make_client(handler) as client:
        service = UserService(client, "http://api.example.test")
        user = await service.get_by_id(1)
        assert user.id == 1
        assert user.name == "Alice"


@pytest.mark.asyncio
async def test_get_all_succes():
    def handler(request: httpx.Request) -> httpx.Response:
        return httpx.Response(200, json=[
            {"id": 1, "name": "Alice", "email": "alice@example.com"},
            {"id": 2, "name": "Bob", "email": "bob@example.com"},
        ])

    async with make_client(handler) as client:
        service = UserService(client, "http://api.example.test")
        users = await service.get_all()
        assert len(users) == 2


@pytest.mark.asyncio
async def test_create_succes():
    def handler(request: httpx.Request) -> httpx.Response:
        assert request.method == "POST"
        return httpx.Response(201, json={"id": 3, "name": "Carol", "email": "carol@example.com"})

    async with make_client(handler) as client:
        service = UserService(client, "http://api.example.test")
        created = await service.create(User(id=None, name="Carol", email="carol@example.com"))
        assert created.id == 3


@pytest.mark.asyncio
async def test_delete_succes_pas_de_corps():
    def handler(request: httpx.Request) -> httpx.Response:
        assert request.method == "DELETE"
        return httpx.Response(204)

    async with make_client(handler) as client:
        service = UserService(client, "http://api.example.test")
        await service.delete(1)  # ne doit pas lever d'exception


@pytest.mark.asyncio
async def test_code_non_2xx_leve_user_service_error_avec_statut_et_corps():
    def handler(request: httpx.Request) -> httpx.Response:
        return httpx.Response(404, text='{"error": "not found"}')

    async with make_client(handler) as client:
        service = UserService(client, "http://api.example.test")
        with pytest.raises(UserServiceError) as exc_info:
            await service.get_by_id(404)
        assert exc_info.value.status_code == 404
        assert "not found" in exc_info.value.response_body


@pytest.mark.asyncio
async def test_timeout_leve_user_service_timeout_error():
    def handler(request: httpx.Request) -> httpx.Response:
        raise httpx.TimeoutException("simulated timeout", request=request)

    async with make_client(handler) as client:
        service = UserService(client, "http://api.example.test", timeout_seconds=0.2)
        with pytest.raises(UserServiceTimeoutError):
            await service.get_by_id(1)
