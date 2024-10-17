
using entities;
using System.Collections.Generic;

namespace dao
{
    public interface ILoanRepository
    {
        void ApplyLoan(Loan loan);
        decimal CalculateInterest(int loanID);
        string LoanStatus(int loanID);
        decimal CalculateEMI(int loanID);
        void LoanRepayment(int loanID, decimal amount);
        List<Loan> GetAllLoans();
        Loan GetLoanById(int loanID);
    }
}
