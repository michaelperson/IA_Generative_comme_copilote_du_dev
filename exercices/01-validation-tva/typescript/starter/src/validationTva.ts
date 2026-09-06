/**
 * Validation de numéros de TVA intracommunautaires belges.
 *
 * TODO (exercice 1) : implémenter validerNumeroTvaBelge en suivant la consigne
 * du README. Le squelette compile et les tests s'exécutent — ils échouent tant
 * que l'implémentation n'est pas faite, c'est attendu.
 *
 * Rappel de la règle métier :
 *  1. Nettoyer l'entrée (espaces, points, tirets, majuscules).
 *  2. Vérifier le format "BE" + 10 chiffres.
 *  3. Les 8 premiers chiffres = numéro d'entreprise, les 2 derniers = clé.
 *  4. Clé attendue = 97 - (numéro d'entreprise modulo 97).
 *  5. Si (numéro d'entreprise modulo 97) vaut 0 : numéro INVALIDE.
 *  6. Retourner un résultat structuré, pas un booléen nu.
 *  7. Gérer explicitement l'entrée vide, null ou undefined.
 */

export interface ResultatValidationTva {
  readonly estValide: boolean;
  readonly messageErreur: string | null;
}

export function validerNumeroTvaBelge(
  entree: string | null | undefined,
): ResultatValidationTva {
  // TODO: remplacer ce stub par l'implémentation complète.
  void entree;
  return { estValide: false, messageErreur: "Non implémenté." };
}
