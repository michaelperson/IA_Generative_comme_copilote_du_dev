/**
 * Fonction legacy fournie pour l'exercice 7.
 *
 * TODO (exercice) : appliquer la méthode « expliquer avant de modifier, puis
 * refactorer par étapes nommées » décrite dans le README. Ne rien changer ici
 * tant que les étapes 1 et 2 (explication + tests de caractérisation) n'ont
 * pas été faites.
 */

export class OrderProcessor {
  processOrder(
    customerType: string,
    amount: number,
    country: string,
    isFirstOrder: boolean,
  ): string {
    let discount = 0;
    if (customerType === "VIP") {
      discount = 0.2;
    } else if (customerType === "REGULAR") {
      discount = 0.05;
    }

    if (isFirstOrder) {
      discount += 0.1;
    }

    let tax = 0;
    if (country === "BE") tax = 0.21;
    else if (country === "FR") tax = 0.2;
    else if (country === "DE") tax = 0.19;
    else tax = 0.15;

    const total = amount * (1 - discount) * (1 + tax);

    let result = "";
    if (total > 1000) {
      result = "Commande de " + total + " EUR - necessite validation manager";
      // TODO: envoyer un email au manager, jamais implémenté
    } else {
      result = "Commande de " + total + " EUR - validee automatiquement";
    }

    console.log("LOG: order processed for " + customerType + " total=" + total);

    return result;
  }
}
