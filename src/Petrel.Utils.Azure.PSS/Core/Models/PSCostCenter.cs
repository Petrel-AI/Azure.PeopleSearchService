namespace Petrel.Utils.Azure.PSS.Core.Models
{
    public class PSCostCenter
    {
        public string Code { get; init; }

        public string Name { get; init; }

        public string GroupCode { get; init; }

        public string DivisionCode { get; init; }

        public string DepartmentCode { get; init; }

        public string Type { get; init; }

        public string Status { get; init; }

        public string HasActiveEmployee { get; init; }

        public Department Department { get; init; }
    }
}