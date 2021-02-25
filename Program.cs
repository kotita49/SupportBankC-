using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;
using Newtonsoft.Json;

namespace SupportBank
{
    class Program
    {
        private static readonly ILogger
            Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var target =
                new FileTarget {
                    FileName = @"C:\Training\SupportBank\Logs\SupportBank.log",
                    Layout = @"${longdate} ${level} - ${logger}: ${message}"
                };
            config.AddTarget("File Logger", target);
            config
                .LoggingRules
                .Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
            Logger.Info("The program has started");
           // string path = @"C:\Training\SupportBank\DodgyTransactions2015.csv";
            string path1 = @"C:\Training\SupportBank\Transactions2013.json";

            // This text is added only once to the file.
            if (!File.Exists(path1))
            {
                Logger.Fatal("could not find file at " + path1);
            }
            var transactions = ReadJson(path1);
            var accountlist = new List<Account>();

            string PersonName = "";

            // Open the file to read from.
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
            while (String.IsNullOrEmpty(PersonName))
                {
                    Console.WriteLine("What account do you want?");
                    var userInput = Console.ReadLine();
                
                if (accountlist.Exists(x => x.Name == userInput))
                {
                    PersonName = userInput;
                }
                else
                {
                    Console
                        .WriteLine("Invalid name - please try a different name");
                    Logger.Info("User entered an invalid name");
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
            string[] readText = null;
            try
            {
                readText = File.ReadAllLines(path).Skip(1).ToArray();
            }
            catch (FileNotFoundException e)
            {
                Logger.Error(e + "Invalid file path: " + path);
            }

            foreach (string line in readText)
            {
                var values = line.Split(',');

                try
                {
                    var date = DateTime.Parse(values[0]);
                }
                catch (System.Exception e)
                {
                    Console
                        .WriteLine("Invalid date in the file " +
                        path +
                        "on line " +
                        (Array.IndexOf(readText, line) + 2));
                    Logger.Error(e + "Invalid date entered by user");
                    continue;
                }

                try
                {
                    var amount = Convert.ToDecimal(values[4]);
                }
                catch (System.Exception e)
                {
                    Console
                        .WriteLine("Invalid amount entered in file on line " +
                        (Array.IndexOf(readText, line) + 2) +
                        path);
                    Logger.Info(e + "invalid amount format");
                    continue;
                }

                //yield return
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

        public static List<Transaction> ReadJson(string path1){
var jsonstring = File.ReadAllText(path1);
var allTransactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonstring);

 return allTransactions;
        }
    }
}
