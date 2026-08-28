package be.bstorm.formation.orderprocessing;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;

/**
 * Tests de caractérisation (étape 2 de l'exercice) : ils figent le comportement
 * observé du OrderProcessor AVANT refactoring, pour au moins les 3 scénarios demandés
 * dans le prompt étape 2. Ils s'exécutent ici contre le OrderProcessor refactoré
 * (solution) sans avoir été modifiés (étape 4) : la signature publique et les valeurs
 * de retour sont restées identiques au comportement legacy d'origine.
 *
 * <p>Note : les valeurs attendues portent un suffixe ".0" (ex. "847.0 EUR"), à la
 * différence de la version C# de cet exercice ("847 EUR"). C'est une différence
 * idiomatique de langage : {@code Double.toString} en Java affiche toujours au moins
 * une décimale, alors que {@code double.ToString()} en C# moderne l'omet pour une
 * valeur entière. Le comportement métier est strictement identique.</p>
 */
class OrderProcessorCharacterizationTest {

    @Test
    void clientVipPremiereCommandeBelgique_estValideeAutomatiquement() {
        OrderProcessor processor = new OrderProcessor();

        String result = processor.processOrder("VIP", 1000, "BE", true);

        assertEquals("Commande de 847.0 EUR - validee automatiquement", result);
    }

    @Test
    void clientRegulierRecurrentFrance_estValideeAutomatiquement() {
        OrderProcessor processor = new OrderProcessor();

        String result = processor.processOrder("REGULAR", 500, "FR", false);

        assertEquals("Commande de 570.0 EUR - validee automatiquement", result);
    }

    @Test
    void montantDepassantLeSeuil_necessiteValidationManager() {
        OrderProcessor processor = new OrderProcessor();

        String result = processor.processOrder("REGULAR", 2000, "DE", false);

        assertEquals("Commande de 2261.0 EUR - necessite validation manager", result);
    }
}
