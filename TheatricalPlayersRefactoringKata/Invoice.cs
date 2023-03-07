using System.Collections.Generic;

namespace TheatricalPlayersRefactoringKata
{
    public class Invoice
    {
        public string Customer { get; }

        public List<Performance> Performances { get; }

        public Invoice(string customer, List<Performance> performance)
        {
            Customer = customer;
            Performances = performance;
        }

    }
}
