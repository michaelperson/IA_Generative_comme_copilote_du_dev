# Diagnostic — incident OrderService (Java / Spring Data JPA / Hibernate)

Corrigé de référence pour les 3 étapes de l'exercice. À ne consulter qu'après avoir rédigé ses propres réponses dans `starter/DIAGNOSTIC.md`.

## Diagnostic à l'aveugle

En ne soumettant que la première ligne du message d'exception (« A different object with the same identifier value was already associated with the session ») sans code ni contexte, l'IA ne peut proposer qu'une explication générique du mécanisme de session Hibernate : elle rappelle qu'une session ne peut suivre qu'une seule instance par identifiant, et suggère des pistes génériques (`session.merge()`, détacher l'entité, vérifier qu'on ne charge pas deux fois la même entité). Ces pistes ne sont pas fausses en soi, mais elles ne permettent pas d'identifier *où* dans le code se produit le double chargement, ni pourquoi le problème n'apparaît que sous charge.

## Diagnostic contextualisé

En fournissant la stack trace complète, l'extrait de `OrderService.java` et l'information « intermittent, uniquement sous charge », le diagnostic devient précis : l'IA peut identifier que le cache Caffeine (singleton applicatif) renvoie une entité `Order` potentiellement chargée par un appel précédent, donc associée à une session Hibernate d'une requête antérieure — déjà fermée ou en tout cas distincte de la session/EntityManager courant. Le caractère « intermittent, sous charge » s'explique par une condition de course : le bug ne se manifeste que lorsque deux requêtes concurrentes accèdent à la même commande pendant la fenêtre de validité du cache (10 minutes).

## Correction proposée

**Cause racine** : l'entité mise en cache provient d'une session Hibernate liée à une requête précédente et n'est plus associée à la session courante : deux requêtes concurrentes récupérant la même entité depuis le cache tentent de la faire suivre par deux sessions différentes, d'où le conflit (`NonUniqueObjectException`).

**Piste de correction retenue** : ne pas mettre en cache des entités suivies par Hibernate, mais des DTO immuables (voir `OrderSnapshot`), et recharger l'entité depuis le repository (donc depuis l'EntityManager courant) avant toute modification. C'est exactement ce qu'implémente `OrderService.java` dans ce dossier `solution/` :
- `updateOrderStatus` ne lit plus jamais l'entité depuis le cache : elle la recharge systématiquement via `orderRepository.findById(orderId)`, qui appartient à l'EntityManager de la requête courante.
- Le cache ne conserve plus que des `OrderSnapshot` (record immuable, aucune référence à une session), utilisés uniquement en lecture seule dans `getOrderSnapshot`.

**Piste alternative** (mentionnée mais non retenue ici) : réattacher explicitement l'entité mise en cache via `session.update()`/`session.merge()` en interceptant le cas où une autre instance serait déjà associée — plus défensif mais plus complexe et plus fragile dans le temps que de simplement ne jamais mettre en cache d'entité suivie par Hibernate.
