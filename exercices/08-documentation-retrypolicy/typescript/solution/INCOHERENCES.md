# Incohérence attendue — exercice 8

## Le défaut

```ts
throw new Error("Unreachable");
```

Après épuisement des tentatives, `RetryPolicy.execute` lève une erreur
générique au lieu de propager la dernière exception rencontrée.

## Pourquoi c'est un défaut et pas un détail

L'appelant reçoit un échec sans cause. En exploitation, cela signifie qu'on ne
peut pas distinguer un délai dépassé d'une erreur d'authentification ou d'une
réponse 500 du service distant. Le diagnostic doit alors se faire par les logs
du service appelé, quand ils existent et qu'on y a accès.

Le message `"Unreachable"` est de surcroît trompeur : il suggère que le
programme a atteint un point réputé inatteignable, alors que ce chemin est
au contraire emprunté à chaque fois que toutes les tentatives échouent.

## Ce qu'un bon prompt de documentation doit produire

La documentation doit **signaler** ce défaut, pas le décrire comme un
comportement voulu. Une documentation qui écrit « lève `Error("Unreachable")`
si toutes les tentatives échouent » est factuellement exacte et
pédagogiquement fausse : elle valide un bug en le documentant.

## Correctif

Voir `retryPolicyFixed.ts` : conserver la dernière exception et la propager.
