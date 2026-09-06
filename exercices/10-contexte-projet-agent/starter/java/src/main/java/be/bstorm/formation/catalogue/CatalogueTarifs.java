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
}
