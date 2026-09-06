package be.bstorm.formation.tva;

import java.util.regex.Pattern;

public final class ValidateurTva {

    private static final Pattern FORMAT = Pattern.compile("^BE\\d{10}$");

    private ValidateurTva() {
    }

    public static ResultatValidationTva validerNumeroTvaBelge(String input) {
        if (input == null || input.isBlank()) {
            return new ResultatValidationTva(false, "Le numéro de TVA ne peut pas être vide.");
        }

        String nettoye = input.replace(" ", "").replace(".", "").replace("-", "").toUpperCase();

        if (!FORMAT.matcher(nettoye).matches()) {
            return new ResultatValidationTva(false,
                    "Format invalide : un numéro de TVA belge doit être \"BE\" suivi de 10 chiffres.");
        }

        int numeroEntreprise = Integer.parseInt(nettoye.substring(2, 10));
        int cleControle = Integer.parseInt(nettoye.substring(10, 12));
        int reste = numeroEntreprise % 97;

        if (reste == 0) {
            return new ResultatValidationTva(false,
                    "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).");
        }

        int cleAttendue = 97 - reste;
        if (cleControle != cleAttendue) {
            return new ResultatValidationTva(false,
                    String.format("Clé de contrôle invalide : attendu %02d, reçu %02d.", cleAttendue, cleControle));
        }

        return new ResultatValidationTva(true, null);
    }
}
