/**
 * Calcule le montant de la remise applicable à une commande, en fonction de
 * l'ancienneté du client et de son statut premium.
 *
 * Fonction métier fournie pour l'exercice 4 : elle est déjà complète et
 * correcte. L'objectif de l'exercice est d'écrire, avec l'aide de l'IA, la
 * suite de tests unitaires qui la couvre.
 *
 * Note d'implémentation : les calculs se font en centimes et en points de
 * base (entiers) pour éviter les erreurs de représentation des flottants —
 * l'arrondi « loin de zéro » au centime est alors exact et testable.
 */

const PLAFOND_POINTS_DE_BASE = 2500; // plafond contractuel à 25 %

/** Erreur levée pour un montant de commande négatif. */
export class MontantCommandeInvalideError extends Error {
  constructor() {
    super("montantCommande doit être positif ou nul");
    this.name = "MontantCommandeInvalideError";
  }
}

function tauxDeBaseEnPointsDeBase(anneesFidelite: number): number {
  if (anneesFidelite >= 10) return 1500;
  if (anneesFidelite >= 5) return 1000;
  if (anneesFidelite >= 1) return 500;
  return 0;
}

/**
 * @param montantCommande Montant de la commande en euros. Doit être positif ou nul.
 * @param anneesFidelite Ancienneté du client en années révolues.
 * @param estClientPremium true si le client bénéficie du statut premium.
 * @returns Le montant de la remise en euros, arrondi au centime « loin de zéro ».
 * @throws {MontantCommandeInvalideError} si montantCommande est négatif.
 */
export function calculerMontantRemise(
  montantCommande: number,
  anneesFidelite: number,
  estClientPremium: boolean,
): number {
  if (montantCommande < 0) {
    throw new MontantCommandeInvalideError();
  }

  const base = tauxDeBaseEnPointsDeBase(anneesFidelite);
  const taux = Math.min(estClientPremium ? base + 500 : base, PLAFOND_POINTS_DE_BASE);

  const montantCentimes = Math.round(montantCommande * 100);
  const produit = montantCentimes * taux; // entier exact
  const quotient = Math.trunc(produit / 10000);
  const reste = produit % 10000;
  const remiseCentimes = reste * 2 >= 10000 ? quotient + 1 : quotient;

  return remiseCentimes / 100;
}
