# Grille de relecture — exercice 10

À appliquer aux deux diffs obtenus, celui de l'étape 1 (sans contexte) et celui
de l'étape 4 (avec contexte). Comptez les retouches nécessaires dans chaque cas.

| # | Point de contrôle | Sans contexte, typiquement |
|---|-------------------|-----------------------------|
| 1 | Les montants restent des entiers en centimes | Un flottant apparaît, souvent via une multiplication par `0.9` |
| 2 | L'erreur levée est typée et propre au domaine | Une erreur générique du langage est levée |
| 3 | Les identifiants métier sont en français | L'assistant bascule tout en anglais |
| 4 | Le tarif d'origine n'est pas modifié | L'objet est muté au lieu d'en produire un nouveau |
| 5 | La méthode publique est documentée, erreurs comprises | Documentation absente ou tautologique |
| 6 | Aucune dépendance externe n'est ajoutée | Une bibliothèque d'arrondi ou de monnaie est importée |
| 7 | Les tests existants ne sont pas modifiés | Ils le sont parfois, pour « faire passer » |
| 8 | La nouvelle fonctionnalité a ses propres tests | Absents, ou limités au cas nominal |
| 9 | Les tests couvrent aussi les cas d'erreur | Le pourcentage hors bornes et le SKU inconnu sont oubliés |
| 10 | L'arrondi est celui demandé, et il est testé | L'arrondi par défaut du langage est utilisé sans le dire |

## Ce qu'il faut faire ressortir en débriefing

L'écart entre les deux colonnes n'est pas une question de qualité de l'outil.
C'est une question d'information disponible : l'assistant ne peut pas deviner
une convention qui n'est écrite nulle part, exactement comme un nouvel arrivant.

Le point à faire dire par le groupe : ces conventions **existaient** dans le
code, mais seulement sous forme d'exemples. Un exemple se généralise mal ; une
règle écrite se vérifie.

## Erreur fréquente sur le fichier lui-même

Le premier réflexe est d'écrire un fichier de quarante lignes qui décrit
l'architecture. C'est une documentation d'architecture, pas un fichier de
contexte. Le bon test : chaque ligne doit pouvoir se vérifier dans un diff. Si
elle ne peut pas, elle appartient au README.
