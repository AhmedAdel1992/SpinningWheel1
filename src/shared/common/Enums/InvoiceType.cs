using Common.Attriputes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums
{
    public enum InvoiceType
    {
        [NameValue(Name = "010000", Value = "388")]
        Tax,

        [NameValue(Name = "020000", Value = "388")]
        Simplified,

        [NameValue(Name = "010000", Value = "381")]
        TaxCredit,

        [NameValue(Name = "020000", Value = "381")]
        SimplifiedCredit,

        [NameValue(Name = "010000", Value = "383")]
        TaxDepit,

        [NameValue(Name = "020000", Value = "383")]
        SimplifiedDepit,
    }
}
