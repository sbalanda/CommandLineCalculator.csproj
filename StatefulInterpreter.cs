using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using System.Runtime.Serialization.Formatters.Binary;

namespace CommandLineCalculator
{
    public interface IBaseStorage
    {
        BaseStorage BStorage { get; }
    }

    [Serializable]
    public class BaseStorage : IBaseStorage
    {
        [NonSerialized]
        private Storage storage;
        private string input;

        protected internal static CultureInfo Culture => CultureInfo.InvariantCulture;
        public BaseStorage BStorage => this;
        public Storage Storage
        {
            get => storage;
            set => storage = value;
        }
        public string Input
        {
            get => input;
            set
            {
                input = value;
                SaveState(this);
            }
        }
        public long X { get; set; } = 420L;

        public void SaveState(IBaseStorage second)
        {
            using var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, second); 
            second.BStorage.Storage.Write(stream.GetBuffer());
        }

        public static IBaseStorage GetSavedState(byte[] inputFile)
        {
            var formatter = new BinaryFormatter();
            using var stream = new MemoryStream();
            stream.Write(inputFile, 0, inputFile.Length);
            stream.Seek(0, SeekOrigin.Begin);
            var savedOperation = (IBaseStorage)formatter.Deserialize(stream);
            return savedOperation;
        }

        public int ReadNumber(UserConsole console)
        {
            return int.Parse(console.ReadLine().Trim(), Culture);
        }
    }

    public sealed class StatefulInterpreter : Interpreter
    {
        public override void Run(UserConsole userConsole, Storage storage)
        {
            var baseStorage = GetInitialState(storage);
            var operationSet = new OperationSet(baseStorage);

            while (true)
            {
                baseStorage.BStorage.Input ??= userConsole.ReadLine();

                if (baseStorage.BStorage.Input == "exit")
                {
                    baseStorage.BStorage.Storage.Write(new byte[0]);
                    return;
                }

                if (operationSet.TryFindOperation(baseStorage.BStorage.Input, out IOperation operation))
                {
                    operation.Execute(userConsole);
                }
                else
                {
                    userConsole.WriteLine("Такой команды нет, используйте help для списка команд");
                }

                baseStorage.BStorage.Input = null;
            }
        }

        public IBaseStorage GetInitialState(Storage storage)
        {
            var savedFile = storage.Read();
            IBaseStorage baseStorage;

            if (savedFile.Length > 0)
            {
                baseStorage = BaseStorage.GetSavedState(savedFile);
                baseStorage.BStorage.Storage = storage;
            }
            else
            {
                baseStorage = new BaseStorage { Storage = storage };
            }

            return baseStorage;
        }
    }

    [Serializable]
    internal sealed class Rand : IBaseStorage, IOperation
    {
        private int? count;
        private int startIndex;

        public string Name { get; } = "rand";
        public int? Count
        {
            get => count;
            set
            {
                count = value;
                BStorage.SaveState(this);
            }
        }
        public int StartIndex
        {
            get => startIndex;
            set
            {
                startIndex = value;
                BStorage.SaveState(this);
            }
        }
        public BaseStorage BStorage { get; set; }

        public Rand(BaseStorage bs) => BStorage = bs;

        public void Execute(UserConsole console)
        {
            const int a = 16807;
            const int m = 2147483647;

            Count ??= BStorage.ReadNumber(console);

            for (var i = StartIndex; i < Count; i++)
            {
                console.WriteLine(BStorage.X.ToString(BaseStorage.Culture));
                BStorage.X = a * BStorage.X % m;
                StartIndex++;
            }

            Count = null;
            StartIndex = 0;
        }
    }

    [Serializable]
    internal sealed class Add : IBaseStorage, IOperation
    {
        private int? a;
        private int? b;

        public string Name { get; } = "add";
        public int? A
        {
            get => a;
            set
            {
                a = value;
                BStorage.SaveState(this);
            }
        }
        public int? B
        {
            get => b;
            set
            {
                b = value;
                BStorage.SaveState(this);
            }
        }
        public BaseStorage BStorage { get; set; }

        public Add(BaseStorage bs) => BStorage = bs;

        public void Execute(UserConsole console)
        {
            A ??= BStorage.ReadNumber(console);
            B ??= BStorage.ReadNumber(console);

            console.WriteLine((A + B)?.ToString(BaseStorage.Culture));

            A = null;
            B = null;
        }
    }

    [Serializable]
    internal sealed class Median : IBaseStorage, IOperation
    {
        private int? count;
        private int startIndex;

        public string Name { get; } = "median";
        public int[] Numbers { get; set; }
        public double Result { get; set; }
        public int? Count
        {
            get => count;
            set
            {
                count = value;
                BStorage.SaveState(this);
            }
        }
        public int StartIndex
        {
            get => startIndex;
            set
            {
                startIndex = value;
                BStorage.SaveState(this);
            }
        }
        public BaseStorage BStorage { get; set; }

        public Median(BaseStorage bs) => BStorage = bs;

        public void Execute(UserConsole console)
        {
            Count ??= BStorage.ReadNumber(console);
            Numbers ??= new int[Count ?? 0];
            for (var i = StartIndex; i < Count; i++)
            {
                Numbers[i] = BStorage.ReadNumber(console);
                StartIndex++;
            }

            Result = CalculateMedian(Numbers);

            console.WriteLine(Result.ToString(BaseStorage.Culture));

            Count = null;
            Result = 0;
            Numbers = null;
            StartIndex = 0;
        }

        private double CalculateMedian(IEnumerable<int> numbers)
        {
            var listNumbers = new List<int>(numbers);
            listNumbers.Sort();
            var numbersCount = listNumbers.Count;
            if (numbersCount == 0)
                return 0;

            if (numbersCount % 2 == 1)
                return listNumbers[numbersCount / 2];

            return (listNumbers[numbersCount / 2 - 1] + listNumbers[numbersCount / 2]) / 2.0;
        }
    }

    [Serializable]
    internal sealed class Help : IBaseStorage, IOperation
    {
        private string helpCommand;
        private int startIndexMessage;
        private int startIndex;

        public string Name { get; } = "help";
        public string HelpCommand
        {
            get => helpCommand;
            set
            {
                helpCommand = value;
                BStorage.SaveState(this);
            }
        }
        public int StartIndexMessage
        {
            get => startIndexMessage;
            set
            {
                startIndexMessage = value;
                BStorage.SaveState(this);
            }
        }
        public int StartIndex
        {
            get => startIndex;
            set
            {
                startIndex = value;
                BStorage.SaveState(this);
            }
        }
        public BaseStorage BStorage { get; set; }

        public Help(BaseStorage bs) => BStorage = bs;

        public void Execute(UserConsole console)
        {
            const string exitMessage = "Чтобы выйти из режима помощи введите end";
            const string commands = "Доступные команды: add, median, rand";
            var helpMessages = new[]
            {
                "Укажите команду, для которой хотите посмотреть помощь",
                commands,
                exitMessage
            };
            var addMessages = new[]
            {
                "Вычисляет сумму двух чисел",
                exitMessage
            };
            var medianMessages = new[]
            {
                "Вычисляет медиану списка чисел",
                exitMessage
            };
            var randMessages = new[]
            {
                "Генерирует список случайных чисел",
                exitMessage
            };
            var defaultMessages = new[]
            {
                "Такой команды нет",
                commands,
                exitMessage
            };

            for (var i = StartIndex; i < helpMessages.Count(); i++)
            {
                console.WriteLine(helpMessages[i]);
                StartIndex++;
            }

            while (true)
            {
                HelpCommand ??= console.ReadLine();
                switch (HelpCommand.Trim())
                {
                    case "end":
                        StartIndex = 0;
                        HelpCommand = null;
                        return;
                    case "add":
                        PrintMessage(addMessages);
                        break;
                    case "median":
                        PrintMessage(medianMessages);
                        break;
                    case "rand":
                        PrintMessage(randMessages);
                        break;
                    default:
                        PrintMessage(defaultMessages);
                        break;
                }
                HelpCommand = null;
            }

            void PrintMessage(string[] messages)
            {
                for (var i = StartIndexMessage; i < messages.Length; i++)
                {
                    console.WriteLine(messages[i]);
                    StartIndexMessage++;
                }
                StartIndexMessage = 0;
            }
        }
    }

    public interface IOperation
    {
        string Name { get; }
        void Execute(UserConsole console);
    }

    public class OperationSet
    {
        private readonly Dictionary<string, IOperation> operations =
            new Dictionary<string, IOperation>(StringComparer.InvariantCultureIgnoreCase);

        public OperationSet(IBaseStorage bStorage)
        {
            Add(bStorage is Add add ? add : new Add(bStorage.BStorage));
            Add(bStorage is Rand rand ? rand : new Rand(bStorage.BStorage));
            Add(bStorage is Median median ? median : new Median(bStorage.BStorage));
            Add(bStorage is Help help ? help : new Help(bStorage.BStorage));
        }

        public bool TryFindOperation(string operationName, out IOperation operation)
            => operations.TryGetValue(operationName, out operation);

        public void Add(IOperation operation)
        {
            operations.Add(operation.Name, operation);
        }
    }
}