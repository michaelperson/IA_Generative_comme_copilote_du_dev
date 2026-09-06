package be.bstorm.formation.catalogue;

import org.junit.jupiter.api.Test;

import java.util.List;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

class CatalogueTarifsTest {

    private static final CatalogueTarifs CATALOGUE = new CatalogueTarifs(List.of(
            new Tarif("CLA-100", "Clavier mécanique", 12990),
            new Tarif("ECR-240", "Écran 24 pouces", 19999),
            new Tarif("SOU-010", "Souris filaire", 999)));

    @Test
    void trouverParSku_retourneLeTarif() {
        assertEquals(12990, CATALOGUE.trouverParSku("CLA-100").prixCentimes());
    }

    @Test
    void trouverParSku_leveUneErreurTypee() {
        assertThrows(TarifIntrouvableException.class, () -> CATALOGUE.trouverParSku("INEXISTANT"));
    }

    @Test
    void appliquerRemise_retourneUnNouveauTarif() {
        Tarif remise = CATALOGUE.appliquerRemise("CLA-100", 10);
        assertEquals(11691, remise.prixCentimes());
        assertEquals("CLA-100", remise.sku());
        assertEquals("Clavier mécanique", remise.libelle());
    }

    @Test
    void appliquerRemise_laisseLeTarifDorigineInchange() {
        CATALOGUE.appliquerRemise("ECR-240", 25);
        assertEquals(19999, CATALOGUE.trouverParSku("ECR-240").prixCentimes());
    }

    @Test
    void appliquerRemise_accepteLesBornes0Et100() {
        assertEquals(999, CATALOGUE.appliquerRemise("SOU-010", 0).prixCentimes());
        assertEquals(0, CATALOGUE.appliquerRemise("SOU-010", 100).prixCentimes());
    }

    @Test
    void appliquerRemise_arronditAuCentimeLoinDeZero() {
        // 999 x 95 = 94905 ; /100 = 949,05 -> 949
        assertEquals(949, CATALOGUE.appliquerRemise("SOU-010", 5).prixCentimes());
        // 19999 x 75 = 1499925 ; /100 = 14999,25 -> 14999
        assertEquals(14999, CATALOGUE.appliquerRemise("ECR-240", 25).prixCentimes());
        // 999 x 50 = 49950 ; /100 = 499,50 -> 500 (loin de zéro, pas 499)
        assertEquals(500, CATALOGUE.appliquerRemise("SOU-010", 50).prixCentimes());
    }

    @Test
    void appliquerRemise_refuseUnPourcentageInvalide() {
        assertThrows(PourcentageRemiseInvalideException.class,
                () -> CATALOGUE.appliquerRemise("CLA-100", -1));
        assertThrows(PourcentageRemiseInvalideException.class,
                () -> CATALOGUE.appliquerRemise("CLA-100", 101));
    }

    @Test
    void appliquerRemise_propageLerreurDeSkuInconnu() {
        assertThrows(TarifIntrouvableException.class,
                () -> CATALOGUE.appliquerRemise("INEXISTANT", 10));
    }
}
