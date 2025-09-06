namespace EvolvingClinic.Domain.Shared;

public readonly record struct PhoneNumber
{
    public string CountryCode { get; }
    public string Number { get; }

    public PhoneNumber(string countryCode, string number)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            throw new ArgumentException("Country code is required");   
        }

        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Phone number is required");   
        }

        countryCode = countryCode.Trim();
        number = number.Trim();

        if (!countryCode.StartsWith('+') || countryCode.Length <= 1 || !countryCode[1..].All(char.IsDigit))
        {
            throw new ArgumentException("Country code must start with + followed by digits");   
        }

        if (!number.All(char.IsDigit))
        {
            throw new ArgumentException("Phone number must contain only digits");   
        }

        CountryCode = countryCode;
        Number = number;
    }

    public string FullNumber => $"{CountryCode}{Number}";

    public override string ToString() => FullNumber;
}