using System;
using System.Linq;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

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
                File.WriteAllLines(path, createText);
            }

            List<decimal>Amountowed = new List<decimal>();

            var TransactionDictionary = new Dictionary<string, List<SingleTransaction>>();

           
            // Open the file to read from.
             var readText = File.ReadAllLines(path).Skip(1);
            foreach (string line in readText)
            {
                
                var values = line.Split(',');

            //    foreach(string el in values){
            //        Console.WriteLine(el);
            //    }
            var PersonName = values[1];

                var Transaction = new SingleTransaction{
                 FromName =PersonName,
                 ToName =values[2],
                 Narrative=values[3],
                 Amount = Convert.ToDecimal(values[4]),
                 Date = DateTime.Parse(values[0])
                };
               
            //    if(Transaction.FromName == "Jon A"){
            //        Amountowed.Add(Transaction.Amount);
            //    }
List<SingleTransaction>OldTransactions;
               if (TransactionDictionary.TryGetValue(PersonName, out OldTransactions))
        {
            OldTransactions.Add(Transaction);
            TransactionDictionary[PersonName] = OldTransactions;
        } else {
            List<SingleTransaction> TransactionList = new List<SingleTransaction>();
            TransactionList.Add(Transaction);
            TransactionDictionary.Add(PersonName,TransactionList );
        }  
    }

foreach (KeyValuePair<string, List<SingleTransaction>> pair in TransactionDictionary)
        {
            Console.WriteLine(pair.Key);
            decimal sumAmount = 0;
            foreach(SingleTransaction Trans in pair.Value){
                    sumAmount = sumAmount + Trans.Amount;
                    Console.WriteLine(Trans.Date + " " + Trans.ToName + " " + Trans.Narrative + " " + Trans.Amount);
            }
            Console.WriteLine(sumAmount);
        }




        Console.WriteLine("What account do you want?");
        string AccountName = Console.ReadLine();

        List<SingleTransaction>Transactions;
               if (TransactionDictionary.TryGetValue(AccountName, out Transactions))
        {
           foreach(SingleTransaction Trans in Transactions){
                    Console.WriteLine(Trans.Date + " " + Trans.ToName + " " + Trans.Narrative + " " + Trans.Amount);
            }
        } else {
            Console.WriteLine("Unknown prson name: " + AccountName);
        }  

    //  decimal sumAmount = 0;
    //         foreach(SingleTransaction Trans in TransactionDictionary["Jon A"]){
    //                 sumAmount = sumAmount + Trans.Amount;
    //         }
    //         Console.WriteLine(sumAmount);

    }
    }
    

    public class SingleTransaction
    {
        public string FromName { get ; set; }
         public string ToName { get ; set; }
        public string Narrative {get ; set;}
        public decimal Amount {get;set;}
        public DateTime Date{get;set;}
   
   
    }

    public class SinglePersonListAccount
    {
      


    //   public string Narrative {get ; set;}
    //   public decimal AmountHeOwes{get ; set;}
    //   public DateTime Date{get;set;}
    //   public decimal AmountOwed{get ; set;}

      public void TotalAmountOwed(List<decimal> Amountowed){

         foreach(decimal i in Amountowed){
            Console.WriteLine(i.ToString());
             }
              decimal Totalowed =Amountowed.Sum();
             Console.WriteLine(Totalowed);
        }


          }


}

