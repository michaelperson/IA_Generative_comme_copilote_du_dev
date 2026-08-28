namespace OrderProcessing;

public static class ManagerValidation
{
    public static bool IsRequired(double total)
    {
        return total > 1000;
    }

    // TODO: envoyer un email au manager, toujours pas fait
    public static void Notify(double total)
    {
    }
}
