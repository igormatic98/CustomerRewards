using Infrastracture.Dtos;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Infrastracture.Services.ExternalCustomerService;

/// <summary>
/// Servis za eksterni api
/// Poziv apija, citanje xmla, i deserijalizacija u c# objekat
/// </summary>
public class ExternalCustomerService
{
    private XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
    private XNamespace tempuri = "http://tempuri.org";
    private string url = "https://www.crcind.com/csp/samples/SOAP.Demo.cls?soap_method=FindPerson";
    private string headersValue = "http://tempuri.org/FindPerson";

    private readonly HttpClient httpClient;

    public ExternalCustomerService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<FindPersonResult> GetExternalCustomer(int customerId)
    {
        var soapRequest = CreateSoapRequest(customerId);

        var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

        content.Headers.Add("SOAPAction", headersValue);

        var response = await httpClient.PostAsync(url, content);
        var responseString = await response.Content.ReadAsStringAsync();

        return ParseXmlToDto(responseString).FindPersonResult;
    }

    private string CreateSoapRequest(int customerId)
    {
        var soapRequest = new XDocument(
            new XElement(
                soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", soapenv),
                new XAttribute(XNamespace.Xmlns + "tem", tempuri),
                new XElement(soapenv + "Header"),
                new XElement(
                    soapenv + "Body",
                    new XElement(tempuri + "FindPerson", new XElement(tempuri + "id", customerId))
                )
            )
        );

        return soapRequest.ToString();
    }

    private FindPersonResponse ParseXmlToDto(string xmlResponse)
    {
        XDocument soapResponse = XDocument.Parse(xmlResponse);

        // Pronalaženje <Body> dela i izdvajanje odgovora
        XElement body = soapResponse.Descendants(soapenv + "Body").FirstOrDefault();

        if (body == null)
        {
            throw new InvalidOperationException("SOAP Body not found in the response.");
        }

        // Pretpostavljam da je pravi odgovor unutar <FindPersonResponse> elementa
        XElement responseElement = body.Elements().FirstOrDefault();
        if (responseElement == null)
        {
            throw new InvalidOperationException(
                "Expected response element not found in SOAP Body."
            );
        }

        // Deserializacija XML-a u objekat
        XmlSerializer serializer = new XmlSerializer(typeof(FindPersonResponse));
        using (var reader = responseElement.CreateReader())
        {
            return (FindPersonResponse)serializer.Deserialize(reader);
        }
    }
}
