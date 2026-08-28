using RemiseFidelite;
using Xunit;

namespace RemiseFidelite.Tests;

public class CalculateurRemiseTests
{
    [Theory]
    [InlineData(1000, 0, false, 0.00)]
    [InlineData(1000, 3, false, 50.00)]
    [InlineData(1000, 7, false, 100.00)]
    [InlineData(1000, 10, false, 150.00)]
    [InlineData(1000, 3, true, 100.00)]
    [InlineData(1000, 10, true, 200.00)]
    public void CalculerMontantRemise_ReturnsExpectedAmount(double montant, int annees, bool premium, double attendu)
    {
        var result = CalculateurRemise.CalculerMontantRemise((decimal)montant, annees, premium);

        Assert.Equal((decimal)attendu, result);
    }

    [Fact]
    public void MontantZero_ReturnsZero()
    {
        var result = CalculateurRemise.CalculerMontantRemise(0m, 10, true);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void MontantNegatif_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CalculateurRemise.CalculerMontantRemise(-10m, 5, false));
    }
}
