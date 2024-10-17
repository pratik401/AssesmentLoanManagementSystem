namespace entities
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CreditScore { get; set; }

        public Customer() { }

        public Customer(int customerID, string name, string email, string phoneNumber, string address, int creditScore)
        {
            CustomerID = customerID;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            CreditScore = creditScore;
        }

        public void PrintCustomerInfo()
        {
            Console.WriteLine($"ID: {CustomerID}, Name: {Name}, Email: {Email}, Phone: {PhoneNumber}, Address: {Address}, Credit Score: {CreditScore}");
        }
    }
}
