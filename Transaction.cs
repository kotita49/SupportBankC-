using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace SupportBank
{
    public class Transaction
    {
        public string FromName { get; set; }

        public string ToName { get; set; }

        public string Narrative { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}