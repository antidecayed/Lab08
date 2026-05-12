using System;

namespace BankApp
{
    public class BankAccount
    {
        public delegate void BalanceHandler(decimal currentBalance);

        private decimal balance;

        public event BalanceHandler? BalanceChangedDeposit;
        public event BalanceHandler? BalanceChangedWithdraw;

        public BankAccount(decimal initialBalance)
        {
            balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            balance += amount;
            BalanceChangedDeposit?.Invoke(balance);
        }

        public void Withdraw(decimal amount)
        {
            if (balance >= amount)
            {
                balance -= amount;
                BalanceChangedWithdraw?.Invoke(balance);
            }
            else
            {
                Console.WriteLine("Недостатньо коштів на рахунку.");
            }
        }
    }

    public class AccountMonitor
    {
        public void OnBalanceChanged(decimal newBalance)
        {
            Console.WriteLine($"Баланс змінено. Поточний стан рахунку: {newBalance} грн.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            BankAccount account = new BankAccount(1000);
            AccountMonitor monitor = new AccountMonitor();

            account.BalanceChangedDeposit += monitor.OnBalanceChanged;
            account.BalanceChangedWithdraw += monitor.OnBalanceChanged;

            Console.WriteLine("--- Операції з рахунком ---");
            account.Deposit(500);
            account.Withdraw(200);
            account.Deposit(1000);

            Console.ReadLine();
        }
    }
}