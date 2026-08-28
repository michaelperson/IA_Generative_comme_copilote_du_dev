# Diagnostic — incident order_service (Python / SQLAlchemy)

Corrigé de référence pour les 3 étapes de l'exercice. À ne consulter qu'après avoir rédigé ses propres réponses dans `starter/DIAGNOSTIC.md`.

## Diagnostic à l'aveugle

En ne soumettant que la première ligne du message d'exception (« Object ... is already attached to session '2' (this is '3') ») sans code ni contexte, l'IA ne peut proposer qu'une explication générique du mécanisme d'attachement objet/Session de SQLAlchemy : elle rappelle qu'un objet mappé ne peut être associé qu'à une seule Session à la fois, et suggère des pistes génériques (`session.merge()`, `session.expunge()`, vérifier qu'on ne partage pas d'objets entre Sessions). Ces pistes ne sont pas fausses en soi, mais elles ne permettent pas d'identifier *où* dans le code se produit le partage d'objet, ni pourquoi le problème n'apparaît que sous charge.

## Diagnostic contextualisé

En fournissant le traceback complet, l'extrait de `order_service.py` et l'information « intermittent, uniquement sous charge », le diagnostic devient précis : l'IA peut identifier que `_order_cache` (dictionnaire au niveau module, partagé par toute l'application) conserve directement l'objet `Order`, potentiellement encore attaché à la Session d'une requête précédente toujours « en vol ». Le caractère « intermittent, sous charge » s'explique par une condition de course : le bug ne se manifeste que lorsqu'une deuxième requête relit l'objet en cache alors que la Session de la première requête ne l'a pas encore libéré (fermée ou expirée).

## Correction proposée

**Cause racine** : l'objet mis en cache reste attaché à la Session de la requête qui l'a chargé ou modifié en premier ; une deuxième requête concurrente qui récupère ce même objet depuis le cache et tente de l'ajouter (`session.add`) à sa propre Session se heurte au fait qu'il est déjà attaché ailleurs, d'où l'`InvalidRequestError`.

**Piste de correction retenue** : ne pas mettre en cache l'objet ORM suivi par SQLAlchemy, mais un DTO immuable (voir `OrderSnapshot`), et recharger l'entité depuis la Session courante avant toute modification. C'est exactement ce qu'implémente `order_service.py` dans ce dossier `solution/` :
- `update_order_status` ne lit plus jamais l'entité depuis le cache : elle la recharge systématiquement via `session.get(Order, order_id)`, dans la Session ouverte pour cette requête.
- Le cache ne conserve plus que des `OrderSnapshot` (dataclass immuable, aucune référence à une Session), utilisés uniquement en lecture seule dans `get_order_snapshot`.

**Piste alternative** (mentionnée mais non retenue ici) : appeler `session.merge(order)` plutôt que `session.add(order)` — `merge` sait fusionner un objet détaché ou attaché ailleurs dans la Session courante sans lever d'exception — mais cette approche reste plus fragile : elle masque le symptôme sans supprimer la cause (un objet ORM qui vit plus longtemps que la Session qui l'a produit), et peut se comporter différemment selon l'état exact de l'objet au moment du merge.
