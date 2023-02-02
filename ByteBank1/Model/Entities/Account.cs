using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteBank.Helpers;
using ByteBank.Service;

namespace ByteBank.Model.Entities
{
    public class Account
    {
        public string Cpf { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public long AccountNumber { get; private set; }
        public double Balance { get; private set; }
        public bool IsBlocked { get; private set; }
        public string Permission { get; private set; }
        public Account(string cpf, string name, string password, long accountNumber,string permission)
        {
            Cpf = cpf;
            Name = name;
            Password = password;
            AccountNumber = accountNumber;
            Balance = 0.00;
            IsBlocked = false;
            Permission = permission;
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



    }
}
