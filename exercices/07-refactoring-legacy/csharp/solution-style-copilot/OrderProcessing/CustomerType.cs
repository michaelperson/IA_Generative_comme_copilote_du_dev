namespace OrderProcessing;

public enum CustomerType
{
    Standard,
    Regular,
    Vip
}

public static class CustomerTypeParser
{
    public static CustomerType Parse(string value)
    {
        if (value == "VIP") return CustomerType.Vip;
        if (value == "REGULAR") return CustomerType.Regular;
        return CustomerType.Standard;
    }
}
