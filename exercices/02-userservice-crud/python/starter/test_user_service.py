"""
Ces tests échouent tant que UserService n'est pas complété (EXERCICE).
Complétez-les et ajoutez les cas décrits dans le README (succès CRUD,
code non-2xx -> UserServiceError, timeout -> UserServiceTimeoutError)
au fur et à mesure de votre implémentation.
"""
import httpx
import pytest

from user_service import User, UserService


@pytest.mark.asyncio
async def test_get_by_id_not_implemented_yet():
    async with httpx.AsyncClient(base_url="http://localhost:0") as client:
        service = UserService(client, "http://localhost:0")
        with pytest.raises(NotImplementedError):
            await service.get_by_id(1)
