/**
 * UserService — client de l'API REST externe /users.
 *
 * Équivalent TypeScript des versions C#, Java et Python de cet exercice :
 * client HTTP injecté (ici la fonction fetch, pour rester sans dépendance),
 * exception typée sur code non-2xx, délai d'expiration explicite de 10
 * secondes, annulation coopérative via AbortSignal (équivalent du
 * CancellationToken), et aucune logique métier dans le service.
 */

export interface User {
  id: number | null;
  name: string;
  email: string;
}

/** Levée pour tout code HTTP non-2xx renvoyé par l'API /users. */
export class UserServiceError extends Error {
  constructor(
    readonly statusCode: number,
    readonly responseBody: string,
  ) {
    super(`L'API /users a répondu avec le code HTTP ${statusCode}.`);
    this.name = "UserServiceError";
  }
}

/** Levée lorsque l'appel à l'API /users dépasse le délai imparti. */
export class UserServiceTimeoutError extends Error {
  constructor(message: string) {
    super(message);
    this.name = "UserServiceTimeoutError";
  }
}

export const DEFAULT_TIMEOUT_MS = 10_000;

/** Contrat du service : uniquement de l'accès aux données. */
export interface IUserService {
  getById(userId: number, signal?: AbortSignal): Promise<User>;
  getAll(signal?: AbortSignal): Promise<User[]>;
  create(user: User, signal?: AbortSignal): Promise<User>;
  update(userId: number, user: User, signal?: AbortSignal): Promise<User>;
  delete(userId: number, signal?: AbortSignal): Promise<void>;
}

export type FetchLike = typeof fetch;

export class UserService implements IUserService {
  private readonly baseUrl: string;

  constructor(
    private readonly fetchFn: FetchLike,
    baseUrl: string,
    private readonly timeoutMs: number = DEFAULT_TIMEOUT_MS,
  ) {
    this.baseUrl = baseUrl.replace(/\/+$/, "");
  }

  /**
   * Envoie une requête et centralise la gestion des erreurs.
   * Combine le délai d'expiration du service et l'annulation demandée par
   * l'appelant : le premier des deux qui se déclenche interrompt l'appel.
   */
  private async send<T>(
    method: string,
    path: string,
    body?: unknown,
    signal?: AbortSignal,
  ): Promise<T | null> {
    const url = `${this.baseUrl}${path}`;
    const timeout = AbortSignal.timeout(this.timeoutMs);
    const combined = signal ? AbortSignal.any([signal, timeout]) : timeout;

    let response: Response;
    try {
      response = await this.fetchFn(url, {
        method,
        signal: combined,
        headers: body === undefined ? undefined : { "content-type": "application/json" },
        body: body === undefined ? undefined : JSON.stringify(body),
      });
    } catch (error) {
      // TimeoutError provient du délai d'expiration ; AbortError d'une
      // annulation explicite de l'appelant, qui doit remonter telle quelle.
      if (error instanceof DOMException && error.name === "TimeoutError") {
        throw new UserServiceTimeoutError(
          `La requête ${method} ${url} a dépassé le délai de ${this.timeoutMs} ms.`,
        );
      }
      throw error;
    }

    if (response.status < 200 || response.status >= 300) {
      throw new UserServiceError(response.status, await response.text());
    }

    if (response.status === 204) return null;
    const text = await response.text();
    return text.length === 0 ? null : (JSON.parse(text) as T);
  }

  async getById(userId: number, signal?: AbortSignal): Promise<User> {
    return (await this.send<User>("GET", `/users/${userId}`, undefined, signal))!;
  }

  async getAll(signal?: AbortSignal): Promise<User[]> {
    return (await this.send<User[]>("GET", "/users", undefined, signal)) ?? [];
  }

  async create(user: User, signal?: AbortSignal): Promise<User> {
    return (await this.send<User>("POST", "/users", user, signal))!;
  }

  async update(userId: number, user: User, signal?: AbortSignal): Promise<User> {
    return (await this.send<User>("PUT", `/users/${userId}`, user, signal))!;
  }

  async delete(userId: number, signal?: AbortSignal): Promise<void> {
    await this.send<void>("DELETE", `/users/${userId}`, undefined, signal);
  }
}
