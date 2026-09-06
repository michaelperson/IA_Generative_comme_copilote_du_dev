export interface ResultatValidationTva {
  estValide: boolean;
  messageErreur: string | null;
}

const FORMAT = /^BE\d{10}$/;

export function validerNumeroTvaBelge(entree: string | null | undefined): ResultatValidationTva {
  if (!entree || !entree.trim()) {
    return { estValide: false, messageErreur: "Le numéro de TVA ne peut pas être vide." };
  }

  const nettoye = entree.replace(/[\s.\-]/g, "").toUpperCase();

  if (!FORMAT.test(nettoye)) {
    return {
      estValide: false,
      messageErreur: 'Format invalide : un numéro de TVA belge doit être "BE" suivi de 10 chiffres.',
    };
  }

  const numeroEntreprise = Number.parseInt(nettoye.slice(2, 10), 10);
  const cleControle = Number.parseInt(nettoye.slice(10, 12), 10);
  const reste = numeroEntreprise % 97;

  if (reste === 0) {
    return {
      estValide: false,
      messageErreur:
        "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).",
    };
  }

  const cleAttendue = 97 - reste;
  if (cleControle !== cleAttendue) {
    return {
      estValide: false,
      messageErreur: `Clé de contrôle invalide : attendu ${String(cleAttendue).padStart(2, "0")}, reçu ${String(cleControle).padStart(2, "0")}.`,
    };
  }

  return { estValide: true, messageErreur: null };
}
