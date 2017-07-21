using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Helper
{
    public enum ReceiptStatus
    {
        New,
        ProcessedToVendor,
        Quoted,
        SentBackToStore,
        Confirmed,
        Rejected
    }
}
