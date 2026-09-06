package be.bstorm.formation.catalogue;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Consultation des tarifs du catalogue produit.
 *
 * <p>Lecture et calcul uniquement : aucune persistance, aucun appel réseau.</p>
 */
public class CatalogueTarifs {

    private final Map<String, Tarif> parSku = new LinkedHashMap<>();

    public CatalogueTarifs(List<Tarif> tarifs) {
        for (Tarif tarif : tarifs) {
            parSku.put(tarif.sku(), tarif);
        }
    }

    /**
     * Retourne le tarif correspondant au SKU demandé.
     *
     * @param sku référence produit recherchée.
     * @return le tarif correspondant.
     * @throws TarifIntrouvableException si aucun tarif ne correspond.
     */
    public Tarif trouverParSku(String sku) {
        Tarif tarif = parSku.get(sku);
        if (tarif == null) {
            throw new TarifIntrouvableException(sku);
        }
        return tarif;
    }

    /**
     * Retourne une nouvelle instance de tarif, remisée du pourcentage indiqué.
     *
     * <p>Le calcul reste entièrement en centimes entiers : le produit
     * prix &times; (100 &minus; pourcentage) est exact, et seule la division
     * finale par 100 demande un arrondi, fait « loin de zéro » au centime le
     * plus proche.</p>
     *
     * @param sku         référence produit à remiser.
     * @param pourcentage pourcentage de remise, entier de 0 à 100 inclus.
     * @return un nouveau {@link Tarif} ; le tarif d'origine est inchangé.
     * @throws TarifIntrouvableException          si aucun tarif ne correspond au SKU.
     * @throws PourcentageRemiseInvalideException si le pourcentage sort de l'intervalle.
     */
    public Tarif appliquerRemise(String sku, int pourcentage) {
        if (pourcentage < 0 || pourcentage > 100) {
            throw new PourcentageRemiseInvalideException(pourcentage);
        }

        Tarif tarif = trouverParSku(sku);

        long produit = tarif.prixCentimes() * (100L - pourcentage);
        long quotient = produit / 100;
        long reste = produit % 100;
        long prixRemiseCentimes = reste * 2 >= 100 ? quotient + 1 : quotient;

        return new Tarif(tarif.sku(), tarif.libelle(), prixRemiseCentimes);
    }
}
