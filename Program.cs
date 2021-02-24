using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace SupportBank
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Training\SupportBank\Transactions2014.csv";

            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                string[] createText = { "Hello", "And", "Welcome" };
                File.WriteAllLines (path, createText);
            }

           Console.WriteLine("What account do you want?");
            string PersonName = Console.ReadLine();

            // Open the file to read from.
            var transactions = ReadCSV(path);
            var accountlist = new List<Account>();
            foreach (var transaction in transactions)
            {
                var accountMatchingName =
                    accountlist
                        .Where(x => x.Name == transaction.FromName)
                        .ToList();
                if (accountMatchingName.Count > 0)
                {
                    var account = accountMatchingName[0];
                    account.IncomingTransactions.Add (transaction);
                }
                else
                {
                    accountlist
                        .Add(new Account {
                            Name = transaction.FromName,
                            IncomingTransactions =
                                new List<Transaction> { transaction },
                            OutgoingTransactions = new List<Transaction>()
                        });
                }

                var accountMatchingName2 =
                    accountlist
                        .Where(x => x.Name == transaction.ToName)
                        .ToList();
                if (accountMatchingName2.Count > 0)
                {
                    var account = accountMatchingName2[0];
                    account.OutgoingTransactions.Add (transaction);
                }
                else
                {
                    accountlist
                        .Add(new Account {
                            Name = transaction.ToName,
                            IncomingTransactions = new List<Transaction>(),
                            OutgoingTransactions =
                                new List<Transaction> { transaction }
                        });
                }
            }
            foreach (var account in accountlist)
            {
                if (account.Name == PersonName)
                {
                    foreach (var transaction in account.IncomingTransactions)
                    {
                        Console
                            .WriteLine(PersonName +
                            " lent: " +
                            transaction.Amount +
                            "to " +
                            transaction.ToName +
                            " on " +
                            transaction.Date +
                            " for " +
                            transaction.Narrative);
                    }
                    foreach (var transaction in account.OutgoingTransactions)
                    {
                        Console
                            .WriteLine(PersonName +
                            " borrowed: " +
                            transaction.Amount +
                            "to " +
                            transaction.FromName +
                            " on " +
                            transaction.Date +
                            " for " +
                            transaction.Narrative);
                    }
                }
            }

            // foreach(var account in accountlist){
            //     if(account.Name == PersonName){
            //        }
            // }
            foreach (var account in accountlist)
            {
                decimal MoneyReceived = 0;
                decimal MoneySpent = 0;

                foreach (var transaction in account.IncomingTransactions)
                {
                    MoneyReceived += transaction.Amount;
                }

                foreach (var transaction in account.OutgoingTransactions)
                {
                    MoneySpent += transaction.Amount;
                }
                decimal balance = MoneyReceived - MoneySpent;
                Console.WriteLine(account.Name + " has balance: " + balance);
            }
        }

        public static List<Transaction> ReadCSV(string path)
        {
            var allTransactions = new List<Transaction>();
            var readText = File.ReadAllLines(path).Skip(1);
            foreach (string line in readText)
            {
                var values = line.Split(',');

                var transaction =
                    new Transaction {
                        FromName = values[1],
                        ToName = values[2],
                        Narrative = values[3],
                        Amount = Convert.ToDecimal(values[4]),
                        Date = DateTime.Parse(values[0])
                    };
                allTransactions.Add (transaction);
            }
            return allTransactions;
        }
    }
}
