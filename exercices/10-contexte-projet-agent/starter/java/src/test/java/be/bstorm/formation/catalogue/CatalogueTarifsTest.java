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
}
