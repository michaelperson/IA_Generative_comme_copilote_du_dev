# Coding guidelines de l'équipe (extrait)

- Toute méthode publique async doit accepter un CancellationToken
- Pas de "magic numbers" : utiliser des constantes nommées
- Les exceptions attrapées doivent être loguées avec le contexte (jamais de catch vide)
- Les noms de variables sont en anglais, les commentaires peuvent être en français
- Toute nouvelle dépendance externe doit être justifiée en commentaire
