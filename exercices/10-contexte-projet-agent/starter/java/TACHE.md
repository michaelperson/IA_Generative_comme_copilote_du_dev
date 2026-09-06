# Tâche à déléguer

Ajouter au catalogue une méthode `appliquerRemise` qui, à partir d'un SKU et
d'un pourcentage de remise, retourne le tarif remisé.

Contraintes fonctionnelles :

- la remise s'exprime en pourcentage entier, de 0 à 100 inclus ;
- un pourcentage hors de cet intervalle est refusé ;
- un SKU inconnu se comporte comme pour la recherche existante ;
- l'arrondi se fait au centime le plus proche, « loin de zéro » ;
- le tarif d'origine ne doit pas être modifié.

Rien d'autre n'est précisé volontairement. Tout le reste — représentation des
montants, type d'erreur levée, langue des identifiants, emplacement des tests,
format de documentation — relève des conventions du projet, que la tâche ne
rappelle pas.
