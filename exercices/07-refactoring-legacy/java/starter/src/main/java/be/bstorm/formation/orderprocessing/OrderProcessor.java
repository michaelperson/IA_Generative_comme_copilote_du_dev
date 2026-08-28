package be.bstorm.formation.orderprocessing;

// OrderProcessor.java
// Fonction legacy fournie pour l'exercice.
// TODO (exercice) : appliquer la méthode "expliquer avant de modifier, puis refactorer
// par étapes nommées" décrite dans le README de cet exercice. Ne rien changer ici tant
// que les étapes 1 et 2 (explication + tests de caractérisation) n'ont pas été faites.
public class OrderProcessor {

    public String processOrder(String customerType, double amount, String country, boolean isFirstOrder) {
        double discount = 0;
        if (customerType.equals("VIP")) {
            discount = 0.20;
        } else if (customerType.equals("REGULAR")) {
            discount = 0.05;
        }

        if (isFirstOrder) {
            discount += 0.10;
        }

        double tax;
        if (country.equals("BE")) tax = 0.21;
        else if (country.equals("FR")) tax = 0.20;
        else if (country.equals("DE")) tax = 0.19;
        else tax = 0.15;

        double total = amount * (1 - discount) * (1 + tax);

        String result;
        if (total > 1000) {
            result = "Commande de " + total + " EUR - necessite validation manager";
            // TODO: envoyer un email au manager, jamais implémenté
        } else {
            result = "Commande de " + total + " EUR - validee automatiquement";
        }

        System.out.println("LOG: order processed for " + customerType + " total=" + total);

        return result;
    }
}
