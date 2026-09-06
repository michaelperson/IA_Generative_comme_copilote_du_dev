# Diagnostic de référence — exercice 6

## Cause racine

L'entité `Order` est mise en cache **alors qu'elle est encore suivie par la
session de la requête qui l'a chargée**. Ce cache est un singleton applicatif :
il survit à la requête, contrairement à la session.

À la requête suivante, le cache rend la même instance d'`Order`, dont le champ
`attachedTo` pointe toujours vers la session précédente, désormais fermée. La
nouvelle session tente de suivre cette entité : la contrainte « une entité,
une seule session » est violée, d'où l'erreur.

Cela explique les deux symptômes :

- **Intermittent** : la première requête après un vidage de cache réussit
  toujours, c'est la deuxième qui échoue.
- **Uniquement sous charge** : sans concurrence, le cache est souvent expiré
  ou la même session est réutilisée, et le conflit ne se manifeste pas.

## Correction retenue

Ne jamais mettre en cache une entité suivie par l'ORM. Le cache ne contient
qu'un **instantané immuable** (`OrderSnapshot`), sans lien avec une session.
Toute écriture recharge l'entité depuis la session courante.

Voir `src/orderService.ts` dans ce dossier.

## Contournements à refuser

Ils traitent le symptôme et pas la cause :

- Détacher l'entité du contexte avant de la mettre à jour.
- Désactiver globalement le suivi des entités.
- Entourer l'appel d'un verrou : masque le problème et crée un point de contention.
