package be.bstorm.formation.tva;

/** Motif de rejet d'un numéro de TVA, exploitable par le code appelant. */
public enum MotifRejet {
    ENTREE_VIDE,
    FORMAT_INVALIDE,
    CAS_LIMITE_MODULO,
    CLE_CONTROLE_INVALIDE
}
