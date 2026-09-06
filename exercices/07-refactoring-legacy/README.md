# Exercice 7 — Refactorer une fonction legacy (SOLID / DRY)

**Module** : Module 7 — Dompter le Code Legacy

## Objectif pédagogique

Appliquer la méthode « expliquer avant de modifier, puis refactorer par étapes nommées » sur un bloc de code monolithique représentatif d'un cas réel fréquent en entreprise.

## Le code legacy fourni pour l'exercice

```csharp
public class OrderProcessor
{
    public string ProcessOrder(string customerType, double amount, string country, bool isFirstOrder)
    {
        double discount = 0;
        if (customerType == "VIP")
        {
            discount = 0.20;
        }
        else if (customerType == "REGULAR")
        {
            discount = 0.05;
        }

        if (isFirstOrder)
        {
            discount += 0.10;
        }

        double tax = 0;
        if (country == "BE") tax = 0.21;
        else if (country == "FR") tax = 0.20;
        else if (country == "DE") tax = 0.19;
        else tax = 0.15;

        double total = amount * (1 - discount) * (1 + tax);

        string result = "";
        if (total > 1000)
        {
            result = "Commande de " + total + " EUR - necessite validation manager";
            // TODO: envoyer un email au manager, jamais implémenté
        }
        else
        {
            result = "Commande de " + total + " EUR - validee automatiquement";
        }

        Console.WriteLine("LOG: order processed for " + customerType + " total=" + total);

        return result;
    }
}
```

**Important** : ce code legacy a un défaut réel et volontaire : le commentaire `// TODO: envoyer un email au manager, jamais implémenté` décrit un comportement qui n'a jamais existé. Le but pédagogique de l'exercice est de voir si le refactoring assisté par IA fait ressortir cette incohérence plutôt que de l'inventer ou de la faire disparaître silencieusement.

## Étape 1 — Faire expliquer avant de modifier

```text
Explique, ligne par ligne si nécessaire, ce que fait cette méthode ProcessOrder,
y compris ses effets de bord (I/O, logs) et ses éventuels TODO ou incohérences
visibles. Ne propose pas encore de correction.
```

## Étape 2 — Générer un filet de sécurité (tests de caractérisation)

```text
Avant toute modification, génère des tests xUnit de caractérisation qui figent
le comportement actuel de ProcessOrder pour au moins : un client VIP première
commande en Belgique, un client REGULAR récurrent en France, un montant qui
dépasse le seuil de validation manager. Le but est de détecter toute
régression pendant le refactoring à venir.
```

## Étape 3 — Refactorer par principe nommé, étape par étape

1. « Applique le principe de responsabilité unique : sépare le calcul de la remise, le calcul de la taxe, et la décision de validation manager en méthodes ou classes distinctes. »
2. « Élimine la duplication (DRY) et les chaînes magiques (customerType, country) en utilisant des enums et un dictionnaire de configuration pour les taux de taxe par pays. »
3. « Retire l'effet de bord Console.WriteLine du cœur métier ; propose une interface ILogger injectée à la place. »
4. « Vérifie que les tests de caractérisation de l'étape 2 passent toujours sur le code refactoré, sans les modifier. »

## Débriefing

Faites remarquer au groupe la présence volontaire d'un TODO jamais implémenté (l'envoi d'email au manager) : un bon refactoring assisté par IA doit signaler ce type d'incohérence plutôt que la faire disparaître silencieusement ou l'inventer de toutes pièces. C'est un excellent cas pour rappeler l'anti-pattern du copier-coller aveugle.

## Utilisation de ce dossier

- `csharp/starter`, `java/starter`, `python/starter`, `typescript/starter` : le code legacy original, non refactoré, avec toutes ses odeurs de code intactes (chaînes if/else, chaînes magiques, effet de bord console, TODO mort). Pas de tests encore : les écrire fait partie de l'exercice (étape 2).
- `csharp/solution`, `java/solution`, `python/solution`, `typescript/solution` : le code refactoré selon les 4 étapes nommées, avec les tests de caractérisation qui passent toujours sans avoir été modifiés. Le point d'extension de notification manager reste explicitement non implémenté (voir le commentaire dédié dans le code).
- `<langage>/solution-style-copilot`, `<langage>/solution-style-claude` : deux variantes de la solution, illustrant des différences de style typiquement observées selon l'outil IA utilisé (voir le `NOTE.md` de chacune). Elles sont fournies **dans les quatre langages**. Le comportement observable y est strictement identique à celui de `solution/` : seuls changent la granularité du découpage, le niveau de documentation et la richesse du typage.

**Note inter-langages** : les valeurs numériques attendues dans les tests diffèrent légèrement dans leur représentation textuelle d'un langage à l'autre (par ex. `"847 EUR"` en C# contre `"847.0 EUR"` en Java et en Python) — c'est une différence idiomatique de conversion `double`/`float` vers chaîne de caractères entre ces langages, pas une différence de comportement métier. La version TypeScript utilise ses propres valeurs de caractérisation, dont un `"114.99999999999999 EUR"` sur le cas du pays inconnu : cet artefact d'arithmétique flottante est **volontairement figé tel quel**. Le « corriger » pendant le refactoring serait un changement de comportement, à décider explicitement et non en passant — c'est précisément ce qu'un test de caractérisation sert à détecter.
