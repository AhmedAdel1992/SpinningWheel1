using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums
{
    public enum PartyIdentification
    {
        [Description("Tax Identification Number")] TIN,
        [Description("Commercial Registration Number")] CRN,
        [Description("Iqama Number")] IQA,
        [Description("Passport ID")] PAS,
        [Description("Other ID")] OTH
    }
}
