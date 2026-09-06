# Coding guidelines de l'équipe (extrait)

- Toute méthode publique asynchrone doit accepter un moyen d'annulation (AbortSignal).
- Pas de « magic numbers » : utiliser des constantes nommées.
- Les exceptions attrapées doivent être loguées avec le contexte (jamais de catch vide).
- Les noms de variables sont en anglais, les commentaires peuvent être en français.
- Toute nouvelle dépendance externe doit être justifiée en commentaire.
