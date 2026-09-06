/**
 * Types métier extraits des chaînes magiques du code d'origine.
 *
 * Le code legacy comparait des chaînes littérales ("VIP", "BE"...) disséminées
 * dans la méthode. Les remplacer par des énumérations ferme deux catégories de
 * défauts d'un coup : les fautes de frappe silencieuses, et l'ajout d'une
 * valeur sans mise à jour de toutes les branches qui la testent.
 *
 * Les fonctions de conversion acceptent volontairement n'importe quelle chaîne
 * et retombent sur une valeur « inconnue » plutôt que de lever une exception :
 * c'est le comportement du code d'origine, et le refactoring ne doit pas le
 * changer sans décision explicite.
 */

export enum CustomerType {
  Vip = "VIP",
  Regular = "REGULAR",
  Unknown = "UNKNOWN",
}

export enum Country {
  Belgium = "BE",
  France = "FR",
  Germany = "DE",
  Other = "OTHER",
}

export function parseCustomerType(valeur: string): CustomerType {
  return valeur === CustomerType.Vip || valeur === CustomerType.Regular
    ? (valeur as CustomerType)
    : CustomerType.Unknown;
}

export function parseCountry(valeur: string): Country {
  return valeur === Country.Belgium || valeur === Country.France || valeur === Country.Germany
    ? (valeur as Country)
    : Country.Other;
}
