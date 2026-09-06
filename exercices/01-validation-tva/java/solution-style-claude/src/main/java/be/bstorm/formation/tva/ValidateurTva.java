package be.bstorm.formation.tva;

import java.util.regex.Pattern;

/**
 * Validation de numéros de TVA intracommunautaires belges.
 *
 * <p>Choix de conception, explicités volontairement :</p>
 * <ol>
 *   <li>Le résultat est un objet structuré plutôt qu'un booléen. Un appelant qui
 *       valide une saisie de formulaire a besoin de savoir <em>pourquoi</em> le
 *       numéro est refusé, pas seulement qu'il l'est.</li>
 *   <li>Le motif de rejet est typé ({@link MotifRejet}) en plus du message. Le
 *       message s'adresse à l'humain, le motif au code appelant, qui peut ainsi
 *       réagir selon la cause sans faire de correspondance sur du texte —
 *       fragile et non traduisible.</li>
 *   <li>Aucune exception n'est levée pour une entrée invalide : une saisie
 *       utilisateur incorrecte est un cas nominal, pas une situation
 *       exceptionnelle.</li>
 * </ol>
 */
public final class ValidateurTva {

    private static final Pattern FORMAT_BELGE = Pattern.compile("^BE\\d{10}$");
    private static final int MODULO = 97;
    private static final int LONGUEUR_NUMERO_ENTREPRISE = 8;

    private ValidateurTva() {
    }

    /**
     * Valide un numéro de TVA intracommunautaire belge.
     *
     * @param input saisie utilisateur, éventuellement {@code null}, vide ou
     *              formatée avec des séparateurs (ex. "BE 0123.456.749").
     * @return un {@link ResultatValidationTva} : validité, message destiné à
     *         l'affichage, et motif de rejet typé.
     */
    public static ResultatValidationTva validerNumeroTvaBelge(String input) {
        if (input == null || input.isBlank()) {
            return rejet(MotifRejet.ENTREE_VIDE, "Le numéro de TVA ne peut pas être vide.");
        }

        String nettoye = normaliser(input);

        if (!FORMAT_BELGE.matcher(nettoye).matches()) {
            return rejet(MotifRejet.FORMAT_INVALIDE,
                    "Format invalide : un numéro de TVA belge doit être \"BE\" suivi de 10 chiffres.");
        }

        String chiffres = nettoye.substring(2);
        int numeroEntreprise = Integer.parseInt(chiffres.substring(0, LONGUEUR_NUMERO_ENTREPRISE));
        int cleControle = Integer.parseInt(chiffres.substring(LONGUEUR_NUMERO_ENTREPRISE));

        int reste = numeroEntreprise % MODULO;

        // Cas limite mathématique : si le reste est nul, la clé attendue vaudrait 97,
        // valeur impossible à représenter sur les deux chiffres prévus par le schéma.
        // Ce n'est pas une erreur de saisie mais une impossibilité structurelle : on la
        // distingue donc d'une clé simplement fausse.
        if (reste == 0) {
            return rejet(MotifRejet.CAS_LIMITE_MODULO,
                    "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).");
        }

        int cleAttendue = MODULO - reste;

        if (cleControle != cleAttendue) {
            return rejet(MotifRejet.CLE_CONTROLE_INVALIDE,
                    String.format("Clé de contrôle invalide : attendu %02d, reçu %02d.", cleAttendue, cleControle));
        }

        return new ResultatValidationTva(true, null, null);
    }

    /**
     * Normalise la saisie : retire les séparateurs usuels et met en majuscules.
     *
     * <p>Les utilisateurs saisissent indifféremment "BE 0123.456.749",
     * "be-0123456749" ou "BE0123456749". Refuser ces variantes serait une rigueur
     * inutile, qui se paierait en support.</p>
     */
    private static String normaliser(String input) {
        return input.replace(" ", "").replace(".", "").replace("-", "").toUpperCase();
    }

    private static ResultatValidationTva rejet(MotifRejet motif, String message) {
        return new ResultatValidationTva(false, message, motif);
    }
}
