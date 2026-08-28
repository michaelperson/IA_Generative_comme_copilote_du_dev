namespace OrderProcessing;

public enum Country
{
    Belgium,
    France,
    Germany,
    Other
}

public static class CountryParser
{
    public static Country Parse(string value)
    {
        if (value == "BE") return Country.Belgium;
        if (value == "FR") return Country.France;
        if (value == "DE") return Country.Germany;
        return Country.Other;
    }
}
