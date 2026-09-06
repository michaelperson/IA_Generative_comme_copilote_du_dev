import { describe, expect, it, vi } from "vitest";
import {
  UserService,
  UserServiceError,
  UserServiceTimeoutError,
  type User,
} from "../src/userService.js";

const ALICE: User = { id: 1, name: "Alice", email: "alice@example.com" };

/**
 * Chaque réponse est fournie sous forme de fabrique : un corps de Response ne
 * peut être lu qu'une fois, il faut donc en créer une neuve à chaque appel.
 */
function fetchStub(reponses: Array<() => Response>) {
  const appels: Array<{ url: string; init?: RequestInit }> = [];
  let i = 0;
  const fn = vi.fn(async (url: string | URL | Request, init?: RequestInit) => {
    appels.push({ url: String(url), init });
    const fabrique = reponses[Math.min(i, reponses.length - 1)]!;
    i += 1;
    return fabrique();
  });
  return { fn: fn as unknown as typeof fetch, appels };
}

const json = (body: unknown, status = 200) => () =>
  new Response(JSON.stringify(body), { status, headers: { "content-type": "application/json" } });

describe("UserService", () => {
  it("récupère un utilisateur par identifiant", async () => {
    const { fn, appels } = fetchStub([json(ALICE)]);
    const service = new UserService(fn, "https://api.example.com/");
    await expect(service.getById(1)).resolves.toEqual(ALICE);
    expect(appels[0]?.url).toBe("https://api.example.com/users/1");
    expect(appels[0]?.init?.method).toBe("GET");
  });

  it("récupère la liste complète", async () => {
    const { fn } = fetchStub([json([ALICE])]);
    const service = new UserService(fn, "https://api.example.com");
    await expect(service.getAll()).resolves.toEqual([ALICE]);
  });

  it("crée un utilisateur en envoyant le corps en JSON", async () => {
    const { fn, appels } = fetchStub([json(ALICE, 201)]);
    const service = new UserService(fn, "https://api.example.com");
    await service.create({ id: null, name: "Alice", email: "alice@example.com" });
    expect(appels[0]?.init?.method).toBe("POST");
    expect(JSON.parse(String(appels[0]?.init?.body))).toMatchObject({ name: "Alice" });
  });

  it("met à jour un utilisateur", async () => {
    const { fn, appels } = fetchStub([json(ALICE)]);
    const service = new UserService(fn, "https://api.example.com");
    await service.update(1, ALICE);
    expect(appels[0]?.init?.method).toBe("PUT");
    expect(appels[0]?.url).toBe("https://api.example.com/users/1");
  });

  it("supprime un utilisateur et tolère une réponse 204 sans corps", async () => {
    const { fn } = fetchStub([() => new Response(null, { status: 204 })]);
    const service = new UserService(fn, "https://api.example.com");
    await expect(service.delete(1)).resolves.toBeUndefined();
  });

  it("lève une exception typée sur code non-2xx, avec le code et le corps", async () => {
    const { fn } = fetchStub([() => new Response("utilisateur inconnu", { status: 404 })]);
    const service = new UserService(fn, "https://api.example.com");
    await expect(service.getById(42)).rejects.toBeInstanceOf(UserServiceError);
    await service.getById(42).catch((e: UserServiceError) => {
      expect(e.statusCode).toBe(404);
      expect(e.responseBody).toBe("utilisateur inconnu");
    });
  });

  it("traduit un dépassement de délai en UserServiceTimeoutError", async () => {
    const fn = (async () => {
      throw new DOMException("The operation was aborted due to timeout", "TimeoutError");
    }) as unknown as typeof fetch;
    const service = new UserService(fn, "https://api.example.com", 5);
    await expect(service.getAll()).rejects.toBeInstanceOf(UserServiceTimeoutError);
  });

  it("laisse remonter une annulation explicite de l'appelant", async () => {
    const fn = (async () => {
      throw new DOMException("The operation was aborted", "AbortError");
    }) as unknown as typeof fetch;
    const service = new UserService(fn, "https://api.example.com");
    await expect(service.getAll()).rejects.toBeInstanceOf(DOMException);
  });
});
