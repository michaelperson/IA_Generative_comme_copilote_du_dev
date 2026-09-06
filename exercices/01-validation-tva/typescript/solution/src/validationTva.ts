/**
 * Validation de numéros de TVA intracommunautaires belges.
 *
 * Règle métier :
 *  - Format attendu après nettoyage : "BE" suivi de 10 chiffres.
 *  - Les 8 premiers chiffres forment le numéro d'entreprise.
 *  - Les 2 derniers forment la clé de contrôle, égale à
 *    97 - (numéro d'entreprise modulo 97).
 *  - Cas limite : si (numéro d'entreprise modulo 97) vaut 0, la clé attendue
 *    vaudrait 97, ce qui n'existe pas sur deux chiffres. Le numéro est alors
 *    considéré comme invalide, quelle que soit la clé fournie.
 */

/** Résultat structuré de la validation : volontairement plus riche qu'un booléen. */
export interface ResultatValidationTva {
  /** true si le numéro de TVA est valide. */
  readonly estValide: boolean;
  /** Message d'erreur explicite si invalide, null si valide. */
  readonly messageErreur: string | null;
}

const FORMAT_BELGE = /^BE\d{10}$/;
const MODULO = 97;

/** Supprime espaces, points et tirets, puis met en majuscules. */
function nettoyer(entree: string): string {
  return entree.replace(/[\s.\-]/g, "").toUpperCase();
}

/**
 * Valide un numéro de TVA intracommunautaire belge.
 *
 * @param entree Numéro saisi par l'utilisateur. Peut contenir des espaces,
 *   points ou tirets (ex : "BE 0123.456.749"), être vide, null ou undefined.
 * @returns Un {@link ResultatValidationTva} exploitable directement par
 *   l'appelant (formulaire web, API...).
 */
export function validerNumeroTvaBelge(
  entree: string | null | undefined,
): ResultatValidationTva {
  if (entree === null || entree === undefined || entree.trim() === "") {
    return { estValide: false, messageErreur: "Le numéro de TVA ne peut pas être vide." };
  }

  const nettoye = nettoyer(entree);

  if (!FORMAT_BELGE.test(nettoye)) {
    return {
      estValide: false,
      messageErreur:
        'Format invalide : un numéro de TVA belge doit être "BE" suivi de 10 chiffres.',
    };
  }

  const chiffres = nettoye.slice(2);
  const numeroEntreprise = Number.parseInt(chiffres.slice(0, 8), 10);
  const cleControle = Number.parseInt(chiffres.slice(8, 10), 10);

  const reste = numeroEntreprise % MODULO;

  if (reste === 0) {
    // Cas limite du modulo 97 : la clé attendue vaudrait 97, ce qui ne
    // correspond à aucune clé valide sur deux chiffres. On traite le cas
    // explicitement plutôt que de laisser la clé attendue déborder.
    return {
      estValide: false,
      messageErreur:
        "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).",
    };
  }

  const cleAttendue = MODULO - reste;

  if (cleControle !== cleAttendue) {
    const attendu = String(cleAttendue).padStart(2, "0");
    const recu = String(cleControle).padStart(2, "0");
    return {
      estValide: false,
      messageErreur: `Clé de contrôle invalide : attendu ${attendu}, reçu ${recu}.`,
    };
  }

  return { estValide: true, messageErreur: null };
}
