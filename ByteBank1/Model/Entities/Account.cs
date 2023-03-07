using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteBank.Service;

namespace ByteBank.Model.Entities
{
    public class Account
    {
        public string Cpf { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public long AccountNumber { get; private set; }
        public double Balance { get; set; } //Tem que ser private
        public bool IsBlocked { get; set; } //Tem que ser private
        public string Permission { get; set; }//Tem que ser private
        public Account(string cpf, string name, string password, long accountNumber)
        {
            Cpf = cpf;
            Name = name;
            Password = password;
            AccountNumber = accountNumber;
            Balance = 0.00;
            IsBlocked = false;
            Permission = "user";
        }

        public void Deposit(double amount)
        {
            Balance+=amount;

        }
        public void Withdraw(double amount)
        {
            Balance -= amount;

        }

        public void Transfer(double amount,Account destinationAccount)
        {
            Balance -= amount;
            destinationAccount.Balance += amount;

        }

        public void BlockAccount() {
            IsBlocked = true;
        }


        public void UnblockAccount()
        {
            IsBlocked = false;
        }

    }
}
