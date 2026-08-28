namespace OrderProcessing;

// Fonction legacy fournie pour l'exercice.
// TODO (exercice) : appliquer la méthode "expliquer avant de modifier, puis refactorer
// par étapes nommées" décrite dans le README de cet exercice. Ne rien changer ici tant
// que les étapes 1 et 2 (explication + tests de caractérisation) n'ont pas été faites.
public class OrderProcessor
{
    public string ProcessOrder(string customerType, double amount, string country, bool isFirstOrder)
    {
        double discount = 0;
        if (customerType == "VIP")
        {
            discount = 0.20;
        }
        else if (customerType == "REGULAR")
        {
            discount = 0.05;
        }

        if (isFirstOrder)
        {
            discount += 0.10;
        }

        double tax = 0;
        if (country == "BE") tax = 0.21;
        else if (country == "FR") tax = 0.20;
        else if (country == "DE") tax = 0.19;
        else tax = 0.15;

        double total = amount * (1 - discount) * (1 + tax);

        string result = "";
        if (total > 1000)
        {
            result = "Commande de " + total + " EUR - necessite validation manager";
            // TODO: envoyer un email au manager, jamais implémenté
        }
        else
        {
            result = "Commande de " + total + " EUR - validee automatiquement";
        }

        Console.WriteLine("LOG: order processed for " + customerType + " total=" + total);

        return result;
    }
}
