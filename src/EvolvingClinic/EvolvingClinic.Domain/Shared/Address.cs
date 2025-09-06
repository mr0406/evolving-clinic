namespace EvolvingClinic.Domain.Shared;

public readonly record struct Address
{
    public string Street { get; }
    public string HouseNumber { get; }
    public string? Apartment { get; }
    public string City { get; }
    public string PostalCode { get; }

    public Address(string street, string houseNumber, string? apartment, string postalCode, string city)
    {
        street = street.Trim();
        houseNumber = houseNumber.Trim();
        apartment = apartment?.Trim();
        city = city.Trim();
        postalCode = postalCode.Trim();

        if (string.IsNullOrWhiteSpace(street))
        {
            throw new ArgumentException("Street is required");
        }

        if (string.IsNullOrWhiteSpace(houseNumber))
        {
            throw new ArgumentException("House number is required");
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City is required");
        }

        if (string.IsNullOrWhiteSpace(postalCode))
        {
            throw new ArgumentException("Postal code is required");
        }

        Street = street;
        HouseNumber = houseNumber;
        Apartment = string.IsNullOrWhiteSpace(apartment) ? null : apartment;
        City = city;
        PostalCode = postalCode;
    }

    public string FullAddress
    {
        get
        {
            var addressPart = $"{Street} {HouseNumber}";
            
            if (!string.IsNullOrEmpty(Apartment))
            {
                addressPart += $"/{Apartment}";   
            }

            return $"{addressPart}, {PostalCode} {City}";
        }
    }

    public override string ToString() => FullAddress;
}