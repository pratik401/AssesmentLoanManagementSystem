namespace entities
{
    public class HomeLoan : Loan
    {
        public string PropertyAddress { get; set; }
        public int PropertyValue { get; set; }

        public HomeLoan() { }

        public HomeLoan(int loanID, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus, string propertyAddress, int propertyValue)
            : base(loanID, customer, principalAmount, interestRate, loanTerm, loanType, loanStatus)
        {
            PropertyAddress = propertyAddress;
            PropertyValue = propertyValue;
        }

        public void PrintHomeLoanInfo()
        {
            PrintLoanInfo();
            Console.WriteLine($"PropertyAddress: {PropertyAddress}, PropertyValue: {PropertyValue}");
        }
    }
}
