using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace SupportBank
{
    public class Account
    {
        public string Name {get; set;}

        public List<Transaction> IncomingTransactions {get; set;}

        public List<Transaction> OutgoingTransactions {get; set;}
    }
}