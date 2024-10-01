namespace Infrastracture.Dtos;

/// <summary>
/// Obzirom da nije navedeno koje podatke treba da sadrzi .csv fajl
/// ovo su osnovni podaci za koje se smatra da su dovolji da obavjeste o uspjesnoj kupovini i koristenju bona
/// </summary>
public class CustomerFileResultDto
{
    public string Name { get; set; }

    public string Ssn { get; set; }

    public DateTime Dob { get; set; }

    public int Age { get; set; }

    public virtual AddressResultDto Home { get; set; }
}

public class AddressResultDto
{
    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Zip { get; set; }
}
