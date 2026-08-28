# Exercice 8 — Générer la documentation d'une classe non documentée

**Durée** : 30 minutes
**Module** : Module 8 — L'IA pour la « Developer Experience » (DX)

## Objectif pédagogique

Produire une documentation de qualité professionnelle (pas un simple reformulateur de noms de
méthodes) sur du code existant, en appliquant la vigilance sur le *pourquoi*.

## Consigne

Classe non documentée fournie pour l'exercice :

```csharp
public class RetryPolicy
{
    private readonly int _maxAttempts;
    private readonly TimeSpan _baseDelay;

    public RetryPolicy(int maxAttempts, TimeSpan baseDelay)
    {
        _maxAttempts = maxAttempts;
        _baseDelay = baseDelay;
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        for (int attempt = 1; attempt <= _maxAttempts; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception) when (attempt < _maxAttempts)
            {
                var delay = TimeSpan.FromMilliseconds(_baseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                await Task.Delay(delay);
            }
        }
        throw new InvalidOperationException("Unreachable");
    }
}
```

Utilisez le prompt fourni ci-dessous (adapté à votre langage cible : C#, Java ou Python) pour
générer la documentation de la classe `RetryPolicy` fournie dans `starter/` (à compléter dans
`DOC_TEMPLATE.md`, ou directement en commentaires de documentation dans le code selon votre
langage). Ne corrigez pas le code : l'exercice porte sur la documentation d'un code existant,
pas sur sa correction.

## Étapes

1. Ouvrez la classe `RetryPolicy` non documentée de votre langage cible (`csharp/starter`,
   `java/starter` ou `python/starter`).
2. Soumettez à l'IA le prompt fourni dans le support (voir ci-dessous), adapté si besoin au
   format de commentaires de documentation de votre langage.
3. Vérifiez que la documentation produite explique le *pourquoi* du backoff exponentiel, décrit
   précisément le comportement en cas d'échec de toutes les tentatives, et signale explicitement
   toute incohérence détectée dans le code — en particulier celle concernant l'exception levée
   à l'épuisement des tentatives.
4. Comparez votre résultat à `solution/` (documentation complète) et à `INCOHERENCES.md`
   (l'incohérence attendue).

## Prompts fournis dans le support

Prompt de documentation exigeante (pourquoi, pas seulement quoi) :
```text
Tu es un développeur senior.

Contexte : cette classe RetryPolicy est utilisée pour fiabiliser des appels réseau vers des services externes instables.

Tâche : documente entièrement cette classe et sa méthode ExecuteAsync au format commentaires XML C# (<summary>, <param>, <typeparam>, <returns>, <exception>).

Contraintes :
- explique le pourquoi du backoff exponentiel, pas seulement le quoi
- mentionne explicitement le comportement en cas d'échec de toutes les tentatives (quelle exception est propagée)
- signale, dans un commentaire dédié, toute ambiguïté ou incohérence que tu détectes dans le code (par exemple : l'exception d'origine est-elle bien préservée après le dernier échec ?)
```

Pour Java, adaptez la contrainte de format à Javadoc (`@param`, `@param <T>`, `@return`,
`@throws`) ; pour Python, adaptez-la à des docstrings au format PEP 257.

## Critères d'évaluation / débriefing

- La documentation produite ne se contente pas de reformuler les noms de méthodes et de
  paramètres : elle explique le *pourquoi* du backoff exponentiel (protéger un service externe
  déjà en difficulté en espaçant les tentatives plutôt que de le marteler à intervalle fixe).
- Elle décrit précisément quelle exception est levée en cas d'échec de toutes les tentatives.
- Elle signale explicitement, dans un commentaire ou une section dédiée, l'incohérence détectée
  dans le code.

**Point de vigilance** : La classe fournie a un défaut réel : après épuisement des tentatives,
elle lève une `InvalidOperationException("Unreachable")` au lieu de propager la dernière
exception rencontrée, ce qui masque la cause originale de l'échec. Un bon prompt de
documentation, poussé à signaler les incohérences, doit faire remonter ce défaut plutôt que de
le documenter comme un comportement voulu.

## Utilisation de ce dossier

- `csharp/starter`, `java/starter`, `python/starter` : la classe `RetryPolicy` non documentée
  (avec son défaut intact), accompagnée d'un `DOC_TEMPLATE.md` à compléter.
- `csharp/solution`, `java/solution`, `python/solution` : correction de référence — la même
  classe (logique et défaut inchangés), entièrement documentée dans le format propre au
  langage, accompagnée d'un `INCOHERENCES.md` (l'analyse attendue de l'incohérence) et, à titre
  de bonus, d'une variante `RetryPolicyFixed` montrant le correctif du défaut. À ne consulter
  qu'après avoir tenté l'exercice.
