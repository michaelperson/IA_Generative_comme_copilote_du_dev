# Tester `compress-logs.sh` en toute sécurité

Rappel de la règle non négociable de l'exercice : **ce script doit être testé
en mode dry-run puis sur un répertoire de test isolé avant toute exécution
sur un système réel** (a fortiori sur un vrai `/var/log`).

Ne lancez jamais ce script directement sur `/var/log` d'une machine sans être
d'abord passé par les étapes ci-dessous sur un répertoire jetable.

## 1. Préparer un répertoire de test isolé

Depuis le dossier `bash/solution/` :

```bash
mkdir -p test-logs

# Un fichier "gros" (> 10 Mo) qui doit être détecté :
dd if=/dev/urandom of=test-logs/application.log bs=1M count=11

# Un fichier "petit" (< 10 Mo) qui ne doit PAS être détecté :
dd if=/dev/urandom of=test-logs/petit.log bs=1M count=1

# Un fichier déjà compressé qui ne doit PAS être retraité :
dd if=/dev/urandom of=test-logs/deja-compresse.log.gz bs=1M count=11
```

(`head -c 11M /dev/urandom > test-logs/application.log` fonctionne aussi si
`dd` n'est pas disponible.)

## 2. Lancer en mode dry-run (par défaut)

```bash
./compress-logs.sh ./test-logs
```

Vérifiez que :
- seul `application.log` est annoncé comme "serait compressé" ;
- `petit.log` et `deja-compresse.log.gz` ne sont pas mentionnés ;
- aucun fichier n'a été modifié dans `test-logs/` (`ls -la test-logs`) ;
- un fichier `compress-logs.log` a été créé à côté du script, avec un
  horodatage sur chaque ligne.

## 3. Lancer réellement avec `--apply`

```bash
./compress-logs.sh ./test-logs --apply
```

Vérifiez que :
- `application.log` a bien été remplacé par `application.log.gz` ;
- `petit.log` est resté intact ;
- `deja-compresse.log.gz` n'a pas été touché ;
- le fichier `compress-logs.log` contient une ligne "COMPRESSÉ" horodatée.

## 4. Vérifier la protection contre une exécution concurrente

Dans un terminal, lancez une exécution volontairement ralentie (ou lancez
deux instances presque simultanément sur un gros volume de fichiers) :

```bash
./compress-logs.sh ./test-logs --apply &
./compress-logs.sh ./test-logs --apply
```

La seconde instance doit se terminer immédiatement avec un message d'erreur
signalant qu'une autre instance tourne déjà (verrou `compress-logs.lock`
déjà pris), et ne doit rien traiter.

## 5. Vérifier le cas "répertoire non accessible en écriture"

```bash
mkdir -p test-logs-readonly
dd if=/dev/urandom of=test-logs-readonly/gros.log bs=1M count=11
chmod 555 test-logs-readonly

./compress-logs.sh ./test-logs-readonly --apply
```

Le script doit s'arrêter avec un message d'erreur explicite avant toute
tentative de compression (et non échouer avec une erreur `gzip` obscure).

> Sur un compte disposant des droits root, ce test peut ne pas se comporter
> comme attendu : root peut outrepasser les permissions du système de
> fichiers. Testez idéalement avec un utilisateur non privilégié.

N'oubliez pas de restaurer les permissions avant de nettoyer :
`chmod 755 test-logs-readonly`.

## 6. Nettoyer

```bash
rm -rf test-logs test-logs-readonly compress-logs.log compress-logs.lock
```

## Avant un usage réel sur `/var/log`

Une fois tous les tests ci-dessus validés sur un répertoire isolé :
1. relancez d'abord en dry-run sur `/var/log` (`./compress-logs.sh /var/log`)
   et relisez attentivement la liste des fichiers qui seraient compressés ;
2. ne lancez `--apply` sur `/var/log` qu'après validation explicite par une
   personne responsable de l'infrastructure concernée, conformément à la
   règle de relecture systématique des scripts touchant à l'infrastructure.
