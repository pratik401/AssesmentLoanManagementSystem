using System;
using System.Collections.Generic;
using entities;
using dao;

namespace LoanManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            ILoanRepository loanRepository = new LoanRepositoryImpl();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nLoan Management System:");
                Console.WriteLine("1. Apply Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan by ID");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. Exit");
                Console.Write("Enter choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ApplyLoan(loanRepository);
                        break;
                    case 2:
                        GetAllLoans(loanRepository);
                        break;
                    case 3:
                        GetLoanById(loanRepository);
                        break;
                    case 4:
                        LoanRepayment(loanRepository);
                        break;
                    case 5:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void ApplyLoan(ILoanRepository loanRepository)
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            Console.Write("Enter Principal Amount: ");
            decimal principalAmount = decimal.Parse(Console.ReadLine());

            Console.Write("Enter Interest Rate (%): ");
            decimal interestRate = decimal.Parse(Console.ReadLine());

            Console.Write("Enter Loan Term (months): ");
            int loanTerm = int.Parse(Console.ReadLine());

            Console.Write("Enter Loan Type (HomeLoan/CarLoan): ");
            string loanType = Console.ReadLine();

            Console.Write("Enter Loan Status (Pending/Approved): ");
            string loanStatus = Console.ReadLine(); // Default to Pending, can be modified if needed

            Loan loan;

            if (loanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Enter Property Address: ");
                string propertyAddress = Console.ReadLine();

                Console.Write("Enter Property Value: ");
                int propertyValue = int.Parse(Console.ReadLine());

                // Correct constructor call for HomeLoan with the correct parameter order
                loan = new HomeLoan(0, new Customer { CustomerID = customerId }, principalAmount, interestRate, loanTerm, loanType, loanStatus, propertyAddress, propertyValue);
            }
            else if (loanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Enter Car Model: ");
                string carModel = Console.ReadLine();

                Console.Write("Enter Car Value: ");
                int carValue = int.Parse(Console.ReadLine());

                // Correct constructor call for CarLoan
                loan = new CarLoan(0, new Customer { CustomerID = customerId }, principalAmount, interestRate, loanTerm, loanType, loanStatus, carModel, carValue);
            }
            else
            {
                Console.WriteLine("Invalid Loan Type.");
                return;
            }

            loanRepository.ApplyLoan(loan);
            Console.WriteLine("Loan applied successfully.");
        }

        static void GetAllLoans(ILoanRepository loanRepository)
        {
            List<Loan> loans = loanRepository.GetAllLoans(); // Call the correct method

            if (loans.Count == 0)
            {
                Console.WriteLine("No loans found.");
                return;
            }

            Console.WriteLine("All Loans:");
            foreach (var loan in loans)
            {
                Console.WriteLine($"Loan ID: {loan.LoanId}, Customer ID: {loan.Customer.CustomerID}, Amount: {loan.PrincipalAmount}, Status: {loan.LoanStatus}");
            }
        }


        static void GetLoanById(ILoanRepository loanRepository)
        {
            Console.Write("Enter Loan ID: ");
            int loanId = int.Parse(Console.ReadLine());
            Loan loan = loanRepository.GetLoanById(loanId);
            if (loan != null)
            {
                Console.WriteLine($"Loan ID: {loan.LoanId}, Customer ID: {loan.Customer.CustomerID}, Amount: {loan.PrincipalAmount}, Status: {loan.LoanStatus}");
            }
            else
            {
                Console.WriteLine("Loan not found.");
            }
        }

        static void LoanRepayment(ILoanRepository loanRepository)
        {
            Console.Write("Enter Loan ID: ");
            int loanId = int.Parse(Console.ReadLine());

            Console.Write("Enter Payment Amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            try
            {
                loanRepository.LoanRepayment(loanId, amount);
                Console.WriteLine("Repayment processed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
