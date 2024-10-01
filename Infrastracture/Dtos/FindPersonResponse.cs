using System.Xml.Serialization;

namespace Infrastracture.Dtos;

/// <summary>
/// DTO klasa za parsiranje XML podataka
/// Ova klasa definiše strukturu koja odgovara XML dokumentu i koristi se za
/// mapiranje XML elemenata na C# objekte putem procesa deserializacije.
/// </summary>
[XmlRoot("FindPersonResponse", Namespace = "http://tempuri.org")]
public class FindPersonResponse
{
    [XmlElement("FindPersonResult")]
    public FindPersonResult FindPersonResult { get; set; }
}

public class FindPersonResult
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("SSN")]
    public string SSN { get; set; }

    [XmlElement("DOB")]
    public DateTime DOB { get; set; }

    [XmlElement("Home")]
    public Address Home { get; set; }

    [XmlElement("Office")]
    public Address Office { get; set; }

    [XmlElement("Age")]
    public int Age { get; set; }
}

public class Address
{
    [XmlElement("Street")]
    public string Street { get; set; }

    [XmlElement("City")]
    public string City { get; set; }

    [XmlElement("State")]
    public string State { get; set; }

    [XmlElement("Zip")]
    public string Zip { get; set; }
}
