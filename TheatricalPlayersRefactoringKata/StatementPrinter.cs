using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        private const int TragedyBasePrice = 40000;
        private const int ComedyBasePrice = 30000;
        private const int TragedyExtraPricePerAudienceMember = 1000;
        private const int ComedyExtraPricePerAudienceMember = 500;
        private const int ComedyAudienceThreshold = 20;
        private const int TragedyAudienceThreshold = 30;

        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));
            if (plays == null) throw new ArgumentNullException(nameof(plays));

            var totalAmount = 0;
            var volumeCredits = 0;
            var result = $"Statement for {invoice.Customer}\n";
            var cultureInfo = new CultureInfo("en-US");

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                var currentAmount = CalculateCurrentAmount(play, perf);

                volumeCredits += CalculateVolumeCredits(play.Type, perf.Audience);
                result = AddLineForOrder(result, cultureInfo, play, currentAmount, perf);
                totalAmount += currentAmount;
            }

            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += $"You earned {volumeCredits} credits\n";
            return result;
        }

        private static int CalculateCurrentAmount(Play play, Performance perf)
        {
            int currentAmount;
            switch (play.Type)
            {
                case "tragedy":
                    currentAmount = CalculateAmountForTragedy(perf.Audience);
                    break;
                case "comedy":
                    currentAmount = CalculateAmountForComedy(perf.Audience);
                    break;
                default:
                    throw new Exception($"Unknown type: {play.Type}");
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

        private static int CalculateVolumeCredits(string playType, int audience)
        {
            int volumeCredits = Math.Max(audience - 30, 0);
            if (playType == "comedy")
            {
                volumeCredits += (int)Math.Floor((decimal)audience / 5);
            }

            return volumeCredits;
        }

        private static int CalculateAmountForTragedy(int audience)
        {
            int currentAmount = TragedyBasePrice;
            if (audience > TragedyAudienceThreshold)
            {
                currentAmount += TragedyExtraPricePerAudienceMember * (audience - TragedyAudienceThreshold);
            }
            return currentAmount;
        }

        private static int CalculateAmountForComedy(int audience)
        {
            int currentAmount = ComedyBasePrice;
            if (audience > ComedyAudienceThreshold)
            {
                currentAmount += 10000 + ComedyExtraPricePerAudienceMember * (audience - ComedyAudienceThreshold);
            }

            return currentAmount += 300 * audience;
        }
    }
}