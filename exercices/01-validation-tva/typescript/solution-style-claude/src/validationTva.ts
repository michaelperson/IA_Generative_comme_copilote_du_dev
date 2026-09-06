/**
 * Validation de numéros de TVA intracommunautaires belges.
 *
 * Choix de conception, explicités volontairement :
 *
 * 1. Le résultat est un objet structuré plutôt qu'un booléen. Un appelant qui
 *    valide une saisie de formulaire a besoin de savoir POURQUOI le numéro est
 *    refusé, pas seulement qu'il l'est.
 * 2. Les motifs de rejet sont typés (`MotifRejet`) en plus du message. Le
 *    message est destiné à l'humain, le motif au code appelant, qui peut ainsi
 *    réagir différemment selon la cause sans faire de correspondance sur du
 *    texte — fragile et non traduisible.
 * 3. Aucune exception n'est levée pour une entrée invalide : une saisie
 *    utilisateur incorrecte est un cas nominal, pas une situation
 *    exceptionnelle.
 */

/** Motif de rejet, exploitable par le code appelant. */
export enum MotifRejet {
  EntreeVide = "ENTREE_VIDE",
  FormatInvalide = "FORMAT_INVALIDE",
  CasLimiteModulo = "CAS_LIMITE_MODULO",
  CleControleInvalide = "CLE_CONTROLE_INVALIDE",
}

/** Résultat structuré de la validation. */
export interface ResultatValidationTva {
  readonly estValide: boolean;
  readonly motif: MotifRejet | null;
  readonly messageErreur: string | null;
}

const FORMAT_BELGE = /^BE\d{10}$/;
const MODULO = 97;
const LONGUEUR_NUMERO_ENTREPRISE = 8;

function valide(): ResultatValidationTva {
  return { estValide: true, motif: null, messageErreur: null };
}

function rejet(motif: MotifRejet, message: string): ResultatValidationTva {
  return { estValide: false, motif, messageErreur: message };
}

/**
 * Normalise la saisie : retire les séparateurs usuels et met en majuscules.
 *
 * Les utilisateurs saisissent indifféremment « BE 0123.456.749 »,
 * « be-0123456749 » ou « BE0123456749 ». Refuser ces variantes serait une
 * rigueur inutile qui se paierait en support.
 */
function normaliser(entree: string): string {
  return entree.replace(/[\s.\-]/g, "").toUpperCase();
}

/**
 * Valide un numéro de TVA intracommunautaire belge.
 *
 * @param entree Saisie utilisateur, éventuellement nulle, vide ou formatée
 *   avec des séparateurs.
 * @returns Un résultat structuré : validité, motif de rejet typé, et message
 *   destiné à l'affichage.
 */
export function validerNumeroTvaBelge(
  entree: string | null | undefined,
): ResultatValidationTva {
  if (entree === null || entree === undefined || entree.trim().length === 0) {
    return rejet(MotifRejet.EntreeVide, "Le numéro de TVA ne peut pas être vide.");
  }

  const nettoye = normaliser(entree);

  if (!FORMAT_BELGE.test(nettoye)) {
    return rejet(
      MotifRejet.FormatInvalide,
      'Format invalide : un numéro de TVA belge doit être "BE" suivi de 10 chiffres.',
    );
  }

  const chiffres = nettoye.slice(2);
  const numeroEntreprise = Number.parseInt(chiffres.slice(0, LONGUEUR_NUMERO_ENTREPRISE), 10);
  const cleControle = Number.parseInt(chiffres.slice(LONGUEUR_NUMERO_ENTREPRISE), 10);

  const reste = numeroEntreprise % MODULO;

  // Cas limite mathématique : si le reste est nul, la clé attendue vaudrait 97,
  // valeur impossible à représenter sur les deux chiffres prévus par le schéma.
  // Ce n'est pas une erreur de saisie mais une impossibilité structurelle : on
  // la distingue donc d'une clé simplement fausse.
  if (reste === 0) {
    return rejet(
      MotifRejet.CasLimiteModulo,
      "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).",
    );
  }

  const cleAttendue = MODULO - reste;

  if (cleControle !== cleAttendue) {
    const attendu = String(cleAttendue).padStart(2, "0");
    const recu = String(cleControle).padStart(2, "0");
    return rejet(
      MotifRejet.CleControleInvalide,
      `Clé de contrôle invalide : attendu ${attendu}, reçu ${recu}.`,
    );
  }

  return valide();
}
