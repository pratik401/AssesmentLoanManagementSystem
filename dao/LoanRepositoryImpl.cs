using dao;
using entities;
using System;
using System.Data.SqlClient;
using exception;

namespace dao
{
    public class LoanRepositoryImpl : ILoanRepository
    {
        private string connectionString = DBUtil.GetDBConn();
       
        // Apply for a new loan and insert into the database
        public void ApplyLoan(Loan loan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Loan (CustomerID, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus) " +
                               "VALUES (@CustomerID, @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", loan.Customer.CustomerID);
                    cmd.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                    cmd.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
                    cmd.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                    cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
                    cmd.Parameters.AddWithValue("@LoanStatus", loan.LoanStatus);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Calculate interest on a loan
        public decimal CalculateInterest(int loanId)
        {
            Loan loan = GetLoanById(loanId);
            if (loan == null)
            {
                throw new InvalidLoanException("Loan not found.");
            }

            decimal interest = (loan.PrincipalAmount * loan.InterestRate * loan.LoanTerm) / 12;
            return interest;
        }

        // Calculate interest with parameters for new loan
        public decimal CalculateInterest(decimal principal, decimal interestRate, int loanTerm)
        {
            decimal interest = (principal * interestRate * loanTerm) / 12;
            return interest;
        }

        // Get loan status based on credit score
        public string LoanStatus(int loanId)
        {
            Loan loan = GetLoanById(loanId);
            if (loan == null)
            {
                throw new InvalidLoanException("Loan not found.");
            }

            if (loan.Customer.CreditScore > 650)
            {
                loan.LoanStatus = "Approved";
            }
            else
            {
                loan.LoanStatus = "Rejected";
            }

            // Update the status in the database
            UpdateLoanStatus(loanId, loan.LoanStatus);

            return loan.LoanStatus;
        }

        // Calculate EMI for a loan
        public decimal CalculateEMI(int loanId)
        {
            Loan loan = GetLoanById(loanId);
            if (loan == null)
            {
                throw new InvalidLoanException("Loan not found.");
            }

            decimal monthlyInterestRate = loan.InterestRate / 12 / 100;
            int numberOfMonths = loan.LoanTerm;
            decimal principal = loan.PrincipalAmount;

            decimal emi = (principal * monthlyInterestRate * (decimal)Math.Pow(1 + (double)monthlyInterestRate, numberOfMonths)) /
                          (decimal)(Math.Pow(1 + (double)monthlyInterestRate, numberOfMonths) - 1);

            return emi;
        }

        // Calculate EMI with parameters for new loan
        public decimal CalculateEMI(decimal principal, decimal interestRate, int loanTerm)
        {
            decimal monthlyInterestRate = interestRate / 12 / 100;
            int numberOfMonths = loanTerm;

            decimal emi = (principal * monthlyInterestRate * (decimal)Math.Pow(1 + (double)monthlyInterestRate, numberOfMonths)) /
                          (decimal)(Math.Pow(1 + (double)monthlyInterestRate, numberOfMonths) - 1);

            return emi;
        }

        // Loan repayment
        public void LoanRepayment(int loanId, decimal amount)
        {
            Loan loan = GetLoanById(loanId);
            if (loan == null)
            {
                throw new InvalidLoanException("Loan not found.");
            }

            decimal emi = CalculateEMI(loanId);

            if (amount < emi)
            {
                Console.WriteLine("Amount less than EMI. Payment rejected.");
            }
            else
            {
                // Calculate how many EMIs can be paid with the amount
                int emiCount = (int)(amount / emi);
                loan.PrincipalAmount -= emi * emiCount;

                // Update the remaining principal amount in the database
                UpdateLoanPrincipalAmount(loanId, loan.PrincipalAmount);

                Console.WriteLine($"{emiCount} EMIs paid. Remaining principal: {loan.PrincipalAmount}");
            }
        }


        public List<Loan> GetAllLoans() // Ensure this matches the interface definition
        {
            List<Loan> loans = new List<Loan>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT LoanID, CustomerID, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus FROM Loan"; // Specify columns for better readability
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Loan loan = new Loan
                            {
                                LoanId = reader.GetInt32(0),
                                Customer = new Customer { CustomerID = reader.GetInt32(1) }, // Assuming foreign key
                                PrincipalAmount = reader.GetDecimal(2),
                                InterestRate = reader.GetDecimal(3),
                                LoanTerm = reader.GetInt32(4),
                                LoanType = reader.GetString(5),
                                LoanStatus = reader.GetString(6)
                            };
                            loans.Add(loan);
                        }
                    }
                }
            }
            return loans;
        }


        // Get loan by ID from the database
        public Loan GetLoanById(int loanId)
        {
            Loan loan = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Loan WHERE LoanId = @LoanId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loanId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            loan = new Loan
                            {
                                LoanId = reader.GetInt32(0),
                                Customer = new Customer { CustomerID = reader.GetInt32(1) },
                                PrincipalAmount = reader.GetDecimal(2),
                                InterestRate = reader.GetDecimal(3),
                                LoanTerm = reader.GetInt32(4),
                                LoanType = reader.GetString(5),
                                LoanStatus = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return loan;
        }

        // Private method to update loan status in the database
        private void UpdateLoanStatus(int loanId, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Loan SET LoanStatus = @LoanStatus WHERE LoanId = @LoanId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LoanStatus", status);
                    cmd.Parameters.AddWithValue("@LoanId", loanId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Private method to update principal amount after repayment
        private void UpdateLoanPrincipalAmount(int loanId, decimal principalAmount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Loan SET PrincipalAmount = @PrincipalAmount WHERE LoanId = @LoanId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PrincipalAmount", principalAmount);
                    cmd.Parameters.AddWithValue("@LoanId", loanId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        
    }
}
