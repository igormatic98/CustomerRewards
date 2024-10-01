using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Dtos;

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
