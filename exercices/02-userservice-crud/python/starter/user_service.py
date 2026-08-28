"""
UserService — client pour l'API REST externe /users.

EXERCICE : complétez la classe UserService ci-dessous. Consultez le README.md
de l'exercice pour le prompt fourni et la liste des contraintes à respecter :
client HTTP injecté (httpx.AsyncClient), exception typée sur les codes non-2xx,
sérialisation JSON (camelCase côté API, ce module utilise déjà des noms de
champs Python idiomatiques via des alias), et timeout explicite de 10 secondes.
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
    Accès aux données de la ressource /users. Aucune logique métier ici :
    uniquement l'accès HTTP (GET/POST/PUT/DELETE).
    """

    def __init__(self, client: httpx.AsyncClient, base_url: str, timeout_seconds: float = DEFAULT_TIMEOUT_SECONDS):
        self._client = client
        self._base_url = base_url.rstrip("/")
        self._timeout_seconds = timeout_seconds

    async def get_by_id(self, user_id: int) -> User:
        # TODO: GET {base_url}/users/{user_id}
        # - lever UserServiceError si le code HTTP n'est pas 2xx (statut + corps)
        # - lever UserServiceTimeoutError en cas de dépassement du délai (httpx.TimeoutException)
        raise NotImplementedError("TODO: get_by_id n'est pas encore implémenté")

    async def get_all(self) -> list[User]:
        # TODO: GET {base_url}/users, mêmes contraintes que get_by_id.
        raise NotImplementedError("TODO: get_all n'est pas encore implémenté")

    async def create(self, user: User) -> User:
        # TODO: POST {base_url}/users avec le corps JSON, mêmes contraintes.
        raise NotImplementedError("TODO: create n'est pas encore implémenté")

    async def update(self, user_id: int, user: User) -> User:
        # TODO: PUT {base_url}/users/{user_id} avec le corps JSON, mêmes contraintes.
        raise NotImplementedError("TODO: update n'est pas encore implémenté")

    async def delete(self, user_id: int) -> None:
        # TODO: DELETE {base_url}/users/{user_id}, pas de corps de réponse attendu.
        raise NotImplementedError("TODO: delete n'est pas encore implémenté")
