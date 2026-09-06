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

/**
 * TODO (exercice 2) : implémenter les cinq méthodes CRUD en respectant les
 * contraintes de la consigne :
 *  - client HTTP injecté (fetchFn), jamais instancié ici ;
 *  - UserServiceError sur tout code HTTP non-2xx, avec le code et le corps ;
 *  - délai d'expiration de 10 secondes -> UserServiceTimeoutError explicite ;
 *  - toutes les méthodes acceptent un AbortSignal optionnel ;
 *  - aucune logique métier : uniquement l'accès aux données.
 *
 * Le squelette compile et les tests s'exécutent : ils échouent tant que
 * l'implémentation n'est pas faite, c'est attendu.
 */
export class UserService implements IUserService {
  private readonly baseUrl: string;

  constructor(
    private readonly fetchFn: FetchLike,
    baseUrl: string,
    private readonly timeoutMs: number = DEFAULT_TIMEOUT_MS,
  ) {
    this.baseUrl = baseUrl.replace(/\/+$/, "");
  }

  async getById(_userId: number, _signal?: AbortSignal): Promise<User> {
    throw new Error("TODO: non implémenté");
  }

  async getAll(_signal?: AbortSignal): Promise<User[]> {
    throw new Error("TODO: non implémenté");
  }

  async create(_user: User, _signal?: AbortSignal): Promise<User> {
    throw new Error("TODO: non implémenté");
  }

  async update(_userId: number, _user: User, _signal?: AbortSignal): Promise<User> {
    throw new Error("TODO: non implémenté");
  }

  async delete(_userId: number, _signal?: AbortSignal): Promise<void> {
    throw new Error("TODO: non implémenté");
  }
}
