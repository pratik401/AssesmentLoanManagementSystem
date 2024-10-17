namespace entities
{
    public class Loan
    {
        public int LoanId { get; set; }
        public Customer Customer { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }

        public Loan() { }

        public Loan(int loanID, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus)
        {
            LoanId = loanID;
            Customer = customer;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = loanStatus;
        }

        public void PrintLoanInfo()
        {
            Console.WriteLine($"LoanID: {LoanId}, Principal: {PrincipalAmount}, InterestRate: {InterestRate}, Term: {LoanTerm}, Type: {LoanType}, Status: {LoanStatus}");
        }
    }
}
