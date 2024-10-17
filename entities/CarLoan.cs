namespace entities
{
    public class CarLoan : Loan
    {
        public string CarModel { get; set; }
        public int CarValue { get; set; }

        public CarLoan() { }

        public CarLoan(int loanID, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus, string carModel, int carValue)
            : base(loanID, customer, principalAmount, interestRate, loanTerm, loanType, loanStatus)
        {
            CarModel = carModel;
            CarValue = carValue;
        }

        public void PrintCarLoanInfo()
        {
            PrintLoanInfo();
            Console.WriteLine($"CarModel: {CarModel}, CarValue: {CarValue}");
        }
    }
}
