using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        private const int AmountTragedy = 40000;
        private const int AmountComedy = 30000;
        public static string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));
            if (plays == null) throw new ArgumentNullException(nameof(plays));
            
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = $"Statement for {invoice.Customer}\n";
            var cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var currentAmount = 0;
                switch (play.Type) 
                {
                    case "tragedy":
                        currentAmount = ComputeAmountForTragedy(perf);
                        break;
                    case "comedy":
                        currentAmount = ComputeCurrentAmountForComedy(perf);
                        break;
                    default:
                        throw new Exception("unknown type: " + play.Type);
                }
                volumeCredits = AddVolumeCredits(volumeCredits, perf, play);
                result = AddLineForOrder(result, cultureInfo, play, currentAmount, perf);
                totalAmount += currentAmount;
            }

            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += $"You earned {volumeCredits} credits\n";
            return result;
        }

        private static int ComputeCurrentAmountForComedy(Performance perf)
        {
            var currentAmount = AmountComedy;
            if (perf.Audience > 20)
            {
                currentAmount += 10000 + 500 * (perf.Audience - 20);
            }

            currentAmount += 300 * perf.Audience;
            return currentAmount;
        }

        private static int ComputeAmountForTragedy(Performance perf)
        {
            var currentAmount = AmountTragedy;
            if (perf.Audience > 30)
            {
                currentAmount += 1000 * (perf.Audience - 30);
            }

            return currentAmount;
        }

        private static string AddLineForOrder(string result, CultureInfo cultureInfo, Play play, int currentAmount,
            Performance perf)
        {
            result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name,
                Convert.ToDecimal(currentAmount / 100), perf.Audience);
            return result;
        }

        private static int AddVolumeCredits(int volumeCredits, Performance perf, Play play)
        {
            volumeCredits += Math.Max(perf.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);
            return volumeCredits;
        }
    }
}
