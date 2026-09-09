using ProbashiShield.Domain.Services.Admin.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class CurrencyService : ICurrencyService
{
    private readonly Dictionary<string, decimal> _rates;

    public CurrencyService()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Helpers", "CurrencyConversionList.json");
        var json = File.ReadAllText(filePath);

        _rates = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json)
                 ?? new Dictionary<string, decimal>();
    }

    public decimal GetRate(string currencyCode)
    {
        return _rates.TryGetValue(currencyCode.ToUpper(), out var rate)
            ? rate
            : throw new Exception($"Rate not found for {currencyCode}");
    }

    public Dictionary<string, decimal> GetAllRates()
    {
        return _rates;
    }
}