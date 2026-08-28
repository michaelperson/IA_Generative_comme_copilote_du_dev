"""
UserService — implémentation de référence du client pour l'API REST
externe /users, via httpx.AsyncClient injecté (équivalent Python de
IHttpClientFactory). Aucune logique métier : uniquement l'accès aux
données distantes.
"""
from __future__ import annotations

from dataclasses import dataclass
from typing import Optional

import httpx

DEFAULT_TIMEOUT_SECONDS = 10.0


@dataclass
class User:
    id: Optional[int]
    name: str
    email: str

    @staticmethod
    def from_json(data: dict) -> "User":
        return User(id=data.get("id"), name=data["name"], email=data["email"])

    def to_json(self) -> dict:
        return {"id": self.id, "name": self.name, "email": self.email}


class UserServiceError(Exception):
    """Exception typée levée pour tout code HTTP non-2xx renvoyé par l'API /users."""

    def __init__(self, status_code: int, response_body: str):
        super().__init__(f"L'API /users a répondu avec le code HTTP {status_code}.")
        self.status_code = status_code
        self.response_body = response_body


class UserServiceTimeoutError(Exception):
    """Exception typée levée lorsque l'appel à l'API /users dépasse le délai imparti."""


class UserService:
    """
    Accès aux données de la ressource /users : GET/POST/PUT/DELETE complets,
    avec gestion explicite des codes non-2xx et du timeout réseau.
    """

    def __init__(self, client: httpx.AsyncClient, base_url: str, timeout_seconds: float = DEFAULT_TIMEOUT_SECONDS):
        self._client = client
        self._base_url = base_url.rstrip("/")
        self._timeout_seconds = timeout_seconds

    async def _send(self, method: str, path: str, json_body: Optional[dict] = None) -> Optional[dict]:
        url = f"{self._base_url}{path}"
        try:
            response = await self._client.request(
                method, url, json=json_body, timeout=self._timeout_seconds
            )
        except httpx.TimeoutException as exc:
            raise UserServiceTimeoutError(
                f"La requête {method} {url} vers l'API /users a dépassé le délai "
                f"de {self._timeout_seconds} secondes."
            ) from exc

        if not (200 <= response.status_code < 300):
            raise UserServiceError(response.status_code, response.text)

        if not response.content:
            return None
        return response.json()

    async def get_by_id(self, user_id: int) -> User:
        data = await self._send("GET", f"/users/{user_id}")
        return User.from_json(data)

    async def get_all(self) -> list[User]:
        data = await self._send("GET", "/users")
        return [User.from_json(item) for item in data]

    async def create(self, user: User) -> User:
        data = await self._send("POST", "/users", json_body=user.to_json())
        return User.from_json(data)

    async def update(self, user_id: int, user: User) -> User:
        data = await self._send("PUT", f"/users/{user_id}", json_body=user.to_json())
        return User.from_json(data)

    async def delete(self, user_id: int) -> None:
        await self._send("DELETE", f"/users/{user_id}")
