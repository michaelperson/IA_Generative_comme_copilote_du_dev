package be.bstorm.formation.tva;

import java.util.regex.Pattern;

/**
 * Validation de numéros de TVA intracommunautaires belges.
 *
 * <p>Règle métier :</p>
 * <ul>
 *   <li>Format attendu après nettoyage : "BE" suivi de 10 chiffres.</li>
 *   <li>Les 8 premiers chiffres forment le numéro d'entreprise.</li>
 *   <li>Les 2 derniers chiffres forment la clé de contrôle, qui doit être égale à
 *       97 - (numéro d'entreprise modulo 97).</li>
 *   <li>Cas limite : si (numéro d'entreprise modulo 97) == 0, la clé attendue vaudrait 97,
 *       ce qui n'est pas un cas valide pour ce schéma sur 2 chiffres : le numéro est
 *       alors considéré comme invalide, quelle que soit la clé fournie.</li>
 * </ul>
 */
public final class ValidateurTva {

    private static final Pattern FORMAT_BELGE = Pattern.compile("^BE\\d{10}$");
    private static final int MODULO = 97;

    private ValidateurTva() {
    }

    /**
     * Valide un numéro de TVA intracommunautaire belge.
     *
     * @param input le numéro de TVA saisi par l'utilisateur. Peut contenir des espaces,
     *              points ou tirets (ex : "BE 0123.456.749") et être {@code null} ou vide.
     * @return un {@link ResultatValidationTva} indiquant si le numéro est valide et,
     *         si ce n'est pas le cas, un message d'erreur explicite.
     */
    public static ResultatValidationTva validerNumeroTvaBelge(String input) {
        if (input == null || input.isBlank()) {
            return new ResultatValidationTva(false, "Le numéro de TVA ne peut pas être vide.");
        }

        String nettoye = nettoyer(input);

        if (!FORMAT_BELGE.matcher(nettoye).matches()) {
            return new ResultatValidationTva(
                    false,
                    "Format invalide : un numéro de TVA belge doit être \"BE\" suivi de 10 chiffres.");
        }

        String chiffres = nettoye.substring(2);
        int numeroEntreprise = Integer.parseInt(chiffres.substring(0, 8));
        int cleControle = Integer.parseInt(chiffres.substring(8, 10));

        int reste = numeroEntreprise % MODULO;

        if (reste == 0) {
            // Cas limite du modulo 97 : la clé attendue vaudrait 97 (97 - 0), ce qui ne
            // correspond à aucune clé de contrôle valide sur 2 chiffres pour ce schéma.
            // On traite donc explicitement ce cas comme invalide plutôt que de laisser
            // la clé attendue "déborder" à 97.
            return new ResultatValidationTva(
                    false,
                    "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).");
        }

        int cleAttendue = MODULO - reste;

        if (cleControle != cleAttendue) {
            return new ResultatValidationTva(
                    false,
                    String.format("Clé de contrôle invalide : attendu %02d, reçu %02d.", cleAttendue, cleControle));
        }

        return new ResultatValidationTva(true, null);
    }

    /**
     * Nettoie l'entrée utilisateur : supprime espaces, points et tirets, puis met en majuscule.
     */
    private static String nettoyer(String input) {
        return input
                .replace(" ", "")
                .replace(".", "")
                .replace("-", "")
                .toUpperCase();
    }
}
