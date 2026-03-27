namespace EventManagementSystem.Helpers;

/// <summary>
/// Currency conversion: 1 USD = 300 LKR
/// </summary>
public static class CurrencyHelper
{
    public const decimal UsdToLkr = 300m;

    public static decimal ToLkr(decimal usd) => usd * UsdToLkr;

    /// <summary>Convert user-entered LKR to base (USD) for database query.</summary>
    public static decimal? LkrToBase(decimal? lkr) => lkr.HasValue && lkr.Value > 0 ? lkr.Value / UsdToLkr : null;
}
