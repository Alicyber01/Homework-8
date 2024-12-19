using System;
using System.Collections;
using System.IO;

namespace Tumakovlaboratory_9
{
    public class BankTransaction
    {
        public DateTime TransactionDate { get; }
        public decimal Amount { get; }

        public BankTransaction(decimal amount)
        {
            TransactionDate = DateTime.Now;
            Amount = amount;
        }
    }

    public class BankAccount : IDisposable
    {
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
        public string AccountType { get; set; }

        private static int accountCounter = 1;
        private Queue _transactionHistory;

        public BankAccount()
        {
            AccountNumber = GenerateAccountNumber();
            Balance = 0;
            AccountType = "Basic";
            _transactionHistory = new Queue();
        }

        public BankAccount(decimal balance)
        {
            AccountNumber = GenerateAccountNumber();
            Balance = balance;
            AccountType = "Basic";
            _transactionHistory = new Queue();
        }

        public BankAccount(string accountType)
        {
            AccountNumber = GenerateAccountNumber();
            Balance = 0;
            AccountType = accountType;
            _transactionHistory = new Queue();
        }

        public BankAccount(decimal balance, string accountType)
        {
            AccountNumber = GenerateAccountNumber();
            Balance = balance;
            AccountType = accountType;
            _transactionHistory = new Queue();
        }

        private string GenerateAccountNumber()
        {
            return "AC" + accountCounter++.ToString("D6");
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            var transaction = new BankTransaction(amount);
            _transactionHistory.Enqueue(transaction);
        }

        public void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                var transaction = new BankTransaction(-amount);
                _transactionHistory.Enqueue(transaction);
            }
            else
            {
                Console.WriteLine("Недостаточно средств.");
            }
        }

        public void DisplayAccountInfo()
        {
            Console.WriteLine($"Номер счета: {AccountNumber}");
            Console.WriteLine($"Баланс: {Balance}");
            Console.WriteLine($"Тип счета: {AccountType}");
        }

        public void DisplayTransactionHistory()
        {
            foreach (BankTransaction transaction in _transactionHistory)
            {
                Console.WriteLine($"Дата транзакции: {transaction.TransactionDate}, Сумма: {transaction.Amount}");
            }
        }

        public void Dispose()
        {
            WriteTransactionsToFile();
            GC.SuppressFinalize(this);
        }

        private void WriteTransactionsToFile()
        {
            string fileName = $"{AccountNumber}_transactions.txt";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("История транзакций для счета: " + AccountNumber);
                writer.WriteLine("Дата, Сумма");

                foreach (BankTransaction transaction in _transactionHistory)
                {
                    writer.WriteLine($"{transaction.TransactionDate}, {transaction.Amount}");
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Выберите программу для выполнения:");
                Console.WriteLine("1. Программа 1 - Создание банковского счета и просмотр информации");
                Console.WriteLine("2. Программа 2 - Банковские транзакции");
                Console.WriteLine("3. Программа 3 - Запись транзакций в файл");
                Console.WriteLine("4. Выход");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Program1();
                        break;
                    case "2":
                        Program2();
                        break;
                    case "3":
                        Program3();
                        break;
                    case "4":
                        exit = true;
                        Console.WriteLine("Выход...");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите корректную опцию.");
                        break;
                }
            }
        }

        static void Program1()
        {
            Console.Clear();
            Console.WriteLine("Создание и отображение информации о банковском счете");

            Console.WriteLine("Введите начальный баланс: ");
            decimal balance = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Введите тип счета (например, Текущий, Сберегательный): ");
            string accountType = Console.ReadLine();

            BankAccount account = new BankAccount(balance, accountType);

            Console.WriteLine("\nБанковский счет успешно создан!");
            account.DisplayAccountInfo();

            Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в главное меню...");
            Console.ReadKey();
        }

        static void Program2()
        {
            BankAccount account = new BankAccount(1000, "Текущий");
            bool backToMenu = false;

            while (!backToMenu)
            {
                Console.Clear();
                Console.WriteLine("Банковские транзакции");
                Console.WriteLine("1. Пополнить счет");
                Console.WriteLine("2. Снять деньги");
                Console.WriteLine("3. Показать информацию о счете");
                Console.WriteLine("4. Показать историю транзакций");
                Console.WriteLine("5. Вернуться в главное меню");

                string transactionChoice = Console.ReadLine();

                switch (transactionChoice)
                {
                    case "1":
                        Console.WriteLine("Введите сумму для пополнения: ");
                        decimal depositAmount = decimal.Parse(Console.ReadLine());
                        account.Deposit(depositAmount);
                        Console.WriteLine($"Пополнено на: {depositAmount}");
                        break;
                    case "2":
                        Console.WriteLine("Введите сумму для снятия: ");
                        decimal withdrawalAmount = decimal.Parse(Console.ReadLine());
                        account.Withdraw(withdrawalAmount);
                        Console.WriteLine($"Снято: {withdrawalAmount}");
                        break;
                    case "3":
                        account.DisplayAccountInfo();
                        break;
                    case "4":
                        account.DisplayTransactionHistory();
                        break;
                    case "5":
                        backToMenu = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите корректную опцию.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
        }

        static void Program3()
        {
            BankAccount account = new BankAccount(1000, "Текущий");

            account.Deposit(500);
            account.Withdraw(200);

            account.Dispose();

            Console.WriteLine("Транзакции записаны в файл.");
            Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в главное меню...");
            Console.ReadKey();
        }
    }
}
