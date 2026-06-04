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
            // Налаштування кодування для коректного відображення української мови
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.Write("Введіть початковий баланс рахунку (грн): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal initialBalance))
            {
                initialBalance = 0; // Значення за замовчуванням, якщо введення некоректне
            }

            // Створення об'єктів
            BankAccount account = new BankAccount(initialBalance);
            AccountMonitor monitor = new AccountMonitor();

            // Підписка метода монітора на обидві події змінення балансу
            account.BalanceChangedDeposit += monitor.OnBalanceChanged;
            account.BalanceChangedWithdraw += monitor.OnBalanceChanged;

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Оберіть операцію ---");
                Console.WriteLine("1. Поповнити рахунок (Deposit)");
                Console.WriteLine("2. Зняти кошти (Withdraw)");
                Console.WriteLine("3. Вийти з програми");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        Console.Write("Введіть суму для поповнення: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount) && depositAmount > 0)
                        {
                            account.Deposit(depositAmount);
                        }
                        else
                        {
                            Console.WriteLine("Некоректна сума поповнення.");
                        }
                        break;

                    case "2":
                        Console.Write("Введіть суму для зняття: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount) && withdrawAmount > 0)
                        {
                            account.Withdraw(withdrawAmount);
                        }
                        else
                        {
                            Console.WriteLine("Некоректна сума зняття.");
                        }
                        break;

                    case "3":
                        running = false;
                        Console.WriteLine("Програму завершено.");
                        break;

                    default:
                        Console.WriteLine("Неправильний вибір, спробуйте ще раз.");
                        break;
                }
            }
        }
    }
}