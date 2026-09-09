using System.Collections.Generic;

namespace ProbashiShield.Domain.Services.Admin.Contracts;

public interface ICurrencyService
{
    decimal GetRate(string currencyCode);
    Dictionary<string, decimal> GetAllRates();
}