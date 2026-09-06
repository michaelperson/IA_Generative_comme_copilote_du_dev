/**
 * Modèle et « unité de travail » simplifiée, qui simule le suivi d'entités
 * d'un ORM (Entity Framework Core, Hibernate, SQLAlchemy...).
 *
 * Une entité ne peut être suivie que par une seule session à la fois : c'est
 * exactement la contrainte qui déclenche l'incident de cet exercice.
 */

export enum OrderStatus {
  Pending = "PENDING",
  Paid = "PAID",
  Shipped = "SHIPPED",
}

export class Order {
  /** Session qui suit actuellement cette entité (null si détachée). */
  attachedTo: Session | null = null;

  constructor(
    readonly id: string,
    public status: OrderStatus,
  ) {}
}

/** Équivalent de InvalidOperationException (EF Core) / NonUniqueObjectException (Hibernate). */
export class EntityAlreadyTrackedError extends Error {
  constructor(id: string) {
    super(
      `The instance of entity type 'Order' cannot be tracked because another ` +
        `instance with the same key value for {'Id'} is already being tracked. ` +
        `(id=${id})`,
    );
    this.name = "EntityAlreadyTrackedError";
  }
}

/** Base de données simulée, partagée par toutes les sessions. */
const TABLE_ORDERS = new Map<string, OrderStatus>([
  ["11111111-1111-1111-1111-111111111111", OrderStatus.Pending],
]);

/** Session de persistance, instanciée pour la durée d'une requête HTTP. */
export class Session {
  private readonly tracked = new Map<string, Order>();

  /** Charge une entité depuis la base et la rattache à CETTE session. */
  async find(id: string): Promise<Order> {
    const statut = TABLE_ORDERS.get(id);
    if (statut === undefined) throw new Error(`Order ${id} introuvable`);
    const order = new Order(id, statut);
    this.track(order);
    return order;
  }

  /** Rattache une entité à cette session. Échoue si elle en suit déjà une autre. */
  track(order: Order): void {
    if (order.attachedTo !== null && order.attachedTo !== this) {
      throw new EntityAlreadyTrackedError(order.id);
    }
    order.attachedTo = this;
    this.tracked.set(order.id, order);
  }

  async saveChanges(): Promise<void> {
    for (const [id, order] of this.tracked) {
      TABLE_ORDERS.set(id, order.status);
    }
  }
}

export function lireStatutEnBase(id: string): OrderStatus | undefined {
  return TABLE_ORDERS.get(id);
}
