# Exercice 4 — Générer une suite de tests unitaires complète

**Module** : Module 5 — Accélérer la Qualité : Tests et QA

## Objectif pédagogique

Mettre en pratique la précision du prompt sur la couverture de cas limites, et faire l'expérience concrète de l'itération : un premier jet incomplet, suivi d'un prompt de relance ciblé.

## Consigne

Fournissez à l'IA une fonction métier tirée de votre projet réel (ou la fonction de calcul de remise fournie ci-dessous si vous n'avez pas d'exemple sous la main), puis demandez la génération de la suite de tests couvrant cas nominaux, erreurs et cas limites.

```csharp
public static decimal CalculerMontantRemise(decimal montantCommande, int anneesFidelite, bool estClientPremium)
{
    if (montantCommande < 0)
        throw new ArgumentOutOfRangeException(nameof(montantCommande));

    decimal tauxBase = anneesFidelite switch
    {
        >= 10 => 0.15m,
        >= 5  => 0.10m,
        >= 1  => 0.05m,
        _     => 0m
    };

    decimal taux = estClientPremium ? tauxBase + 0.05m : tauxBase;
    taux = Math.Min(taux, 0.25m); // plafond contractuel à 25%

    return Math.Round(montantCommande * taux, 2, MidpointRounding.AwayFromZero);
}
```
*(Fonction métier fournie pour l'exercice, si pas d'exemple personnel)*

## Étapes

1. Premier prompt volontairement générique : « Génère les tests unitaires pour cette fonction ». Notez le nombre de cas couverts.
2. Analysez ce qui manque : le plafond à 25% est-il testé à sa frontière exacte ? Le cas `anneesFidelite = 0` ? Le cas `montantCommande = 0` ?
3. Renvoyez un prompt de relance ciblé listant explicitement les cas manquants identifiés, sans tout redemander depuis le début.

## Cas limites à couvrir

- Plafond de 25% à sa frontière exacte (et constat : avec les paliers actuels, le taux réel maximal atteignable est 20%, le plafond ne peut jamais s'activer — un bon débriefing fait ressortir ce constat plutôt que d'inventer un cas à 25%).
- `anneesFidelite = 0`.
- `montantCommande = 0`.
- `montantCommande` négatif (doit lever une exception/erreur typée).
- Comportement de l'arrondi (« loin de zéro ») sur un montant exactement à mi-chemin entre deux centimes.

## Utilisation de ce dossier

- `csharp/starter`, `java/starter`, `python/starter`, `typescript/starter` : la fonction métier déjà implémentée et correcte, avec un test minimal seulement (le projet compile et s'exécute en vert) — à vous de construire la suite complète en suivant la consigne.
- `csharp/solution`, `java/solution`, `python/solution`, `typescript/solution` : suite de tests de référence complète.
- `<langage>/solution-style-copilot`, `<langage>/solution-style-claude` : deux variantes de la solution, illustrant des différences de style typiquement observées selon l'outil IA utilisé (voir le `NOTE.md` de chacune). Elles sont fournies **dans les quatre langages**. Le comportement observable y est strictement identique à celui de `solution/` : seuls changent la granularité du découpage, le niveau de documentation et la richesse du typage.
