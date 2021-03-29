namespace Petrel.Utils.Azure.PSS.Core.Models
{
    // TODO Maxim: remove the default values
    public class PSSubPerson
    {
        public string EnglishName { get; init; }

        public string EnglishFirstName { get; init; }

        public string EnglishMiddleName { get; init; }

        public string EnglishLastName { get; init; }

        public string CyrllicName { get; init; }

        public string CyrllicFirstName { get; init; }

        public string CyrllicLastName { get; init; }

        public string CyrllicMiddleName { get; init; }

        public string BadgeNumber { get; init; }

        public string CanonicalID { get; init; }

        public string CAI { get; init; }

        public string NetworkID { get; init; }

        public string EmploymentStatusCode { get; init; }

        public PSCostCenter CostCenter { get; init; }

        public PSPosition Position { get; init; }

        public string EmploymentStartDate { get; init; }

        public string EmploymentEndDate { get; init; }

        public bool HasSubOrdinate { get; init; }

        public string SearchType { get; init; }

        public JobCategory JobCategory { get; init; }

        public HomeCompany HomeCompany { get; init; }
    }
}