using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SupportBank
{
    public class Transaction

    {
        private static readonly ILogger
            Logger = LogManager.GetCurrentClassLogger();
            [JsonProperty("FromAccount")]
        public string FromName { get; set; }
[JsonProperty("ToAccount")]
        public string ToName { get; set; }

        public string Narrative { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}