using FluentAssertions;
using NUnit.Framework;
using static CommandLineCalculator.Tests.TestConsole.Action;

namespace CommandLineCalculator.Tests
{
    public class StatefulInterpreterShould
    {
        public static TestCaseData[] RegularCases => new[]
        {
            new TestCaseData(
                new TestConsole(
                    (Read, "exit")
                )
            ).SetName("exit"),
            new TestCaseData(
                new TestConsole(
                    (Read, "add"),
                    (Read, "15"),
                    (Read, "60"),
                    (Write, "75"),
                    (Read, "exit")
                )
            ).SetName("add"),
            new TestCaseData(
                new TestConsole(
                    (Read, "median"),
                    (Read, "5"),
                    (Read, "17"),
                    (Read, "30"),
                    (Read, "29"),
                    (Read, "23"),
                    (Read, "20"),
                    (Write, "23"),
                    (Read, "exit")
                )
            ).SetName("odd median"),
            new TestCaseData(
                new TestConsole(
                    (Read, "median"),
                    (Read, "6"),
                    (Read, "17"),
                    (Read, "30"),
                    (Read, "29"),
                    (Read, "23"),
                    (Read, "20"),
                    (Read, "24"),
                    (Write, "23.5"),
                    (Read, "exit")
                )
            ).SetName("even median"),
            new TestCaseData(
                new TestConsole(
                    (Read, "rand"),
                    (Read, "1"),
                    (Write, "420"),
                    (Read, "rand"),
                    (Read, "2"),
                    (Write, "7058940"),
                    (Write, "528003995"),
                    (Read, "rand"),
                    (Read, "3"),
                    (Write, "760714561"),
                    (Write, "1359476136"),
                    (Write, "1636897319"),
                    (Read, "exit")
                )
            ).SetName("rand 3x"),
            new TestCaseData(
                new TestConsole(
                    (Read, "ramd"),
                    (Write, "Такой команды нет, используйте help для списка команд"),
                    (Read, "exit")
                )
            ).SetName("unknown command"),
            new TestCaseData(
                new TestConsole(
                    (Read, "help"),
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "end"),
                    (Read, "exit")
                )
            ).SetName("empty help"),
            new TestCaseData(
                new TestConsole(
                    (Read, "help"),
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "add"),
                    (Write, "Вычисляет сумму двух чисел"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "end"),
                    (Read, "exit")
                )
            ).SetName("add help"),
            new TestCaseData(
                new TestConsole(
                    (Read, "help"),
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "median"),
                    (Write, "Вычисляет медиану списка чисел"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "end"),
                    (Read, "exit")
                )
            ).SetName("median help"),
            new TestCaseData(
                new TestConsole(
                    (Read, "help"),
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "rand"),
                    (Write, "Генерирует список случайных чисел"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "end"),
                    (Read, "exit")
                )
            ).SetName("rand help"),
            new TestCaseData(
                new TestConsole(
                    (Read, "help"),
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "media"),
                    (Write, "Такой команды нет"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "end"),
                    (Read, "exit")
                )
            ).SetName("unknown help"),
            new TestCaseData(
                new TestConsole(
                    (Read, "help"),
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),

                    (Read, "media"),
                    (Write, "Такой команды нет"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),

                    (Read, "add"),
                    (Write, "Вычисляет сумму двух чисел"),
                    (Write, "Чтобы выйти из режима помощи введите end"),

                    (Read, "rand"),
                    (Write, "Генерирует список случайных чисел"),
                    (Write, "Чтобы выйти из режима помощи введите end"),

                    (Read, "median"),
                    (Write, "Вычисляет медиану списка чисел"),
                    (Write, "Чтобы выйти из режима помощи введите end"),

                    (Read, "end"),
                    (Read, "exit")
                )
            ).SetName("several commands help"),
        };

        public static TestCaseData[] InterruptionCases => new[]
        {
            //new TestCaseData(
            //    new TestConsole(
            //        (Read, "median"),
            //        (Read, "200"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Write, "13"),
            //        (Read, "exit")
            //    ),
            //    new[] {1,4, 7,11,12,13,16,17,20,21,24,25,26,27,28,29,30,32,34,37,40,43,46,48,50,53,55,60,65,70,75,80,82,84,86,88,89,90,92,95,98,99}
            //).SetName("mytestbigmedian200"),

            //new TestCaseData(
            //    new TestConsole(
            //        (Read, "median"),
            //        (Read, "100"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Read, "1"),
            //        (Read, "2"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "6"),
            //        (Read, "7"),
            //        (Read, "8"),
            //        (Read, "9"),
            //        (Read, "10"),
            //        (Read, "11"),
            //        (Read, "12"),
            //        (Read, "13"),
            //        (Read, "14"),
            //        (Read, "15"),
            //        (Read, "16"),
            //        (Read, "17"),
            //        (Read, "18"),
            //        (Read, "19"),
            //        (Read, "20"),
            //        (Read, "21"),
            //        (Read, "22"),
            //        (Read, "23"),
            //        (Read, "24"),
            //        (Read, "25"),
            //        (Write, "13"),
            //        (Read, "exit")
            //    ),
            //    new[] {1,4, 7,11,12,13,16,17,20,21,24,25,26,27,28,29,30,32,34,37,40,43,46,48,50,53,55,60,65,70,75,80,82,84,86,88,89,90,92,95,98,99}
            //).SetName("mytestbigmedian100"),

            //new TestCaseData(
            //    new TestConsole(
            //        (Read, "rand"),
            //        (Read, "100"),
            //        (Write, "420"),
            //        (Write, "7058940"),
            //        (Write, "528003995"),
            //        (Write, "760714561"),
            //        (Write, "1359476136"),
            //        (Write, "1636897319"),
            //        (Write, "2067722363"),
            //        (Write, "1629379187"),
            //        (Write, "264529365"),
            //        (Write, "653888265"),
            //        (Write, "1226248156"),
            //        (Write, "152197633"),
            //        (Write, "332594254"),
            //        (Write, "11693837"),
            //        (Write, "1117306582"),
            //        (Write, "974714306"),
            //        (Write, "1018081626"),
            //        (Write, "1895672533"),
            //        (Write, "500875239"),
            //        (Write, "74245633"),
            //        (Write, "158354924"),
            //        (Write, "738969035"),
            //        (Write, "954640644"),
            //        (Write, "794976971"),
            //        (Write, "1682183610"),
            //        (Write, "837720515"),
            //        (Write, "665905873"),
            //        (Write, "1342722994"),
            //        (Write, "1387197482"),
            //        (Write, "1545608142"),
            //        (Write, "1073848482"),
            //        (Write, "718867586"),
            //        (Write, "264519880"),
            //        (Write, "494473870"),
            //        (Write, "2008102847"),
            //        (Write, "331553277"),
            //        (Write, "1843346221"),
            //        (Write, "1520844725"),
            //        (Write, "1486926481"),
            //        (Write, "506166028"),
            //        (Write, "949706829"),
            //        (Write, "1624210499"),
            //        (Write, "1441219676"),
            //        (Write, "1111040019"),
            //        (Write, "879288668"),
            //        (Write, "1369668069"),
            //        (Write, "1134023490"),
            //        (Write, "615429305"),
            //        (Write, "1239085183"),
            //        (Write, "1155745722"),
            //        (Write, "628762539"),
            //        (Write, "1992449733"),
            //        (Write, "1390154860"),
            //        (Write, "1858136307"),
            //        (Write, "989717075"),
            //        (Write, "1914033510"),
            //        (Write, "2003654157"),
            //        (Write, "724348092"),
            //        (Write, "33587401"),
            //        (Write, "1862733093"),
            //        (Write, "938488085"),
            //        (Write, "2049341027"),
            //        (Write, "1931910203"),
            //        (Write, "1809522828"),
            //        (Write, "2134245029"),
            //        (Write, "836846562"),
            //        (Write, "1009763331"),
            //        (Write, "1676525523"),
            //        (Write, "231532774"),
            //        (Write, "130964254"),
            //        (Write, "2092962450"),
            //        (Write, "637759290"),
            //        (Write, "729504853"),
            //        (Write, "803923648"),
            //        (Write, "1725128659"),
            //        (Write, "1060653666"),
            //        (Write, "144410715"),
            //        (Write, "454365895"),
            //        (Write, "75748533"),
            //        (Write, "1795275107"),
            //        (Write, "1043482999"),
            //        (Write, "1467302791"),
            //        (Write, "1403289836"),
            //        (Write, "1426862298"),
            //        (Write, "324756437"),
            //        (Write, "1425489632"),
            //        (Write, "876679092"),
            //        (Write, "460197177"),
            //        (Write, "1445340992"),
            //        (Write, "1658521327"),
            //        (Write, "430204829"),
            //        (Write, "2022605201"),
            //        (Write, "1406964844"),
            //        (Write, "915695991"),
            //        (Write, "1234706335"),
            //        (Write, "574891384"),
            //        (Write, "670563035"),
            //        (Write, "158749789"),
            //        (Write, "933014149"),
            //        (Write, "243211849"),
            //        (Read, "exit")
            //    ),
            //    new[] {1,4, 7,11,12,13,16,17,20,21,24,25,26,27,28,29,30,32,34,37,40,43,46,48,50,53,55,60,65,70,75,80,82,84,86,88,89,90,92,95,98,99}
            //).SetName("mytestbigrand100"),

            //new TestCaseData(
            //    new TestConsole(
            //        (Read, "add"),
            //        (Read, "5"),
            //        (Read, "7"),
            //        (Write, "12"),
            //        (Read, "median"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "9"),
            //        (Write, "5"),
            //        (Read, "rand"),
            //        (Read, "5"),
            //        (Write, "420"),
            //        (Write, "7058940"),
            //        (Write, "528003995"),
            //        (Write, "760714561"),
            //        (Write, "1359476136"),
            //        (Read, "help"),
            //        (Write, "Укажите команду, для которой хотите посмотреть помощь"),
            //        (Write, "Доступные команды: add, median, rand"),
            //        (Write, "Чтобы выйти из режима помощи введите end"),
            //        (Read, "exit"),
            //        (Write, "Такой команды нет"),
            //        (Write, "Доступные команды: add, median, rand"),
            //        (Write, "Чтобы выйти из режима помощи введите end"),
            //        (Read, "end"),
            //        (Read, "exit")
            //    ),
            //    new[] {1,4, 7,11,12,13,16,17,20,21,24}
            //).SetName("mytesthelp3"),

            //new TestCaseData(
            //    new TestConsole(
            //        (Read, "add"),
            //        (Read, "5"),
            //        (Read, "7"),
            //        (Write, "12"),
            //        (Read, "median"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "9"),
            //        (Write, "5"),
            //        (Read, "rand"),
            //        (Read, "5"),
            //        (Write, "420"),
            //        (Write, "7058940"),
            //        (Write, "528003995"),
            //        (Write, "760714561"),
            //        (Write, "1359476136"),
            //        (Read, "help"),
            //        (Write, "Укажите команду, для которой хотите посмотреть помощь"),
            //        (Write, "Доступные команды: add, median, rand"),
            //        (Write, "Чтобы выйти из режима помощи введите end"),
            //        (Read, "exit"),
            //        (Write, "Такой команды нет"),
            //        (Write, "Доступные команды: add, median, rand"),
            //        (Write, "Чтобы выйти из режима помощи введите end"),
            //        (Read, "end"),
            //        (Read, "exit")
            //    ),
            //    new[] {0,2,4,6,8,10,12,14,16,18,20,22,24,26}
            //).SetName("mytesthelp4"),

            //new TestCaseData(
            //    new TestConsole(
            //        (Read, "add"),
            //        (Read, "5"),
            //        (Read, "7"),
            //        (Write, "12"),
            //        (Read, "median"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "9"),
            //        (Write, "5"),
            //        (Read, "rand"),
            //        (Read, "5"),
            //        (Write, "420"),
            //        (Write, "7058940"),
            //        (Write, "528003995"),
            //        (Write, "760714561"),
            //        (Write, "1359476136"),
            //        (Read, "help"),
            //        (Write, "Укажите команду, для которой хотите посмотреть помощь"),
            //        (Write, "Доступные команды: add, median, rand"),
            //        (Write, "Чтобы выйти из режима помощи введите end"),
            //        (Read, "exit"),
            //        (Write, "Такой команды нет"),
            //        (Write, "Доступные команды: add, median, rand"),
            //        (Write, "Чтобы выйти из режима помощи введите end"),
            //        (Read, "end"),
            //        (Read, "exit")
            //    ),
            //    new[] {1,3,5,7,9,11,13,15,17,19,21,23,25,27}
            //).SetName("mytesthelp5"),

            //new TestCaseData(
            //    new TestConsole(
            //        (Read, "add"),
            //        (Read, "5"),
            //        (Read, "7"),
            //        (Write, "12"),
            //        (Read, "median"),
            //        (Read, "3"),
            //        (Read, "4"),
            //        (Read, "5"),
            //        (Read, "9"),
            //        (Write, "5"),
            //        (Read, "rand"),
            //        (Read, "5"),
            //        (Write, "420"),
            //        (Write, "7058940"),
            //        (Write, "528003995"),
            //        (Write, "760714561"),
            //        (Write, "1359476136"),
            //        (Read, "help"),
            //        (Write, "Укажите команду, для которой хотите посмотреть помощь"),
            //        (Write, "Доступные команды: add, median, rand"),
            //        (Write, "Чтобы выйти из режима помощи введите end"),
            //        (Read, "exit"),
            //        (Write, "Такой команды нет"),
            //        (Write, "Доступные команды: add, median, rand"),
            //        (Write, "Чтобы выйти из режима помощи введите end"),
            //        (Read, "end"),
            //        (Read, "exit")
            //    ),
            //    new[] {1,5,9,13,17,21,25,27}
            //).SetName("mytesthelp6"),

            new TestCaseData(
                new TestConsole(
                    (Read, "add"),                                              // 0
                    (Read, "5"),                                                            // 1
                    (Read, "7"),                                                            // 2
                    (Write, "12"),                                                          // 3
                    
                    (Read, "median"),                                                       // 4
                    (Read, "3"),                                                            // 5
                    (Read, "4"),                                                            // 6
                    (Read, "5"),                                                            // 7
                    (Read, "9"),                                                            // 8
                    (Write, "5"),                                                           // 9
                    
                    (Read, "rand"),                                                         // 10
                    (Read, "5"),                                                            // 11
                    (Write, "420"),                                                         // 12
                    (Write, "7058940"),                                                     // 13
                    (Write, "528003995"),                                                   // 14                           
                    (Write, "760714561"),                                                   // 15
                    (Write, "1359476136"),                                                  // 16
                    
                    (Read, "help"),                                                         // 17
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),       // 18
                    (Write, "Доступные команды: add, median, rand"),                        // 19
                    (Write, "Чтобы выйти из режима помощи введите end"),                    // 20
                    (Read, "exit"),                                                         // 21
                    (Write, "Такой команды нет"),                                           // 22
                    (Write, "Доступные команды: add, median, rand"),                        // 23
                    (Write, "Чтобы выйти из режима помощи введите end"),                    // 24
                    (Read, "end"),                                                          // 25
                    
                    (Read, "rand"),                                                         // 26
                    (Read, "5"),                                                            // 27
                    (Write, "1636897319"),                                                  // 28
                    (Write, "2067722363"),                                                  // 29
                    (Write, "1629379187"),                                                  // 30                           
                    (Write, "264529365"),                                                   // 31
                    (Write, "653888265"),                                                   // 32
                    
                    (Read, "median"),                                                       // 33
                    (Read, "3"),                                                            // 34
                    (Read, "4"),                                                            // 35
                    (Read, "5"),                                                            // 36
                    (Read, "9"),                                                            // 37
                    (Write, "5"),                                                           // 38
                    
                    (Read, "help"),                                                         // 39
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),       // 40
                    (Write, "Доступные команды: add, median, rand"),                        // 41
                    (Write, "Чтобы выйти из режима помощи введите end"),                    // 42
                    (Read, "exit"),                                                         // 43
                    (Write, "Такой команды нет"),                                           // 44
                    (Write, "Доступные команды: add, median, rand"),                        // 45
                    (Write, "Чтобы выйти из режима помощи введите end"),                    // 46

                    (Read, "add"),                                                          // 47
                    (Write, "Вычисляет сумму двух чисел"),                                  // 48
                    (Write, "Чтобы выйти из режима помощи введите end"),                    // 49

                    (Read, "median"),                                                       // 50
                    (Write, "Вычисляет медиану списка чисел"),                              // 51
                    (Write, "Чтобы выйти из режима помощи введите end"),                    // 52

                    (Read, "rand"),                                                         // 53
                    (Write, "Генерирует список случайных чисел"),                           // 54
                    (Write, "Чтобы выйти из режима помощи введите end"),                    // 55

                    (Read, "end"),                                                          // 56
                    (Read, "end"),                                                          // 57
                    (Write, "Такой команды нет, используйте help для списка команд"),       // 58
                    (Read, "end"),                                                          // 59
                    (Write, "Такой команды нет, используйте help для списка команд"),       // 60
                    (Read, "end"),                                                          // 61
                    (Write, "Такой команды нет, используйте help для списка команд"),       // 62
                    
                    (Read, "exit")                                                          // 63
                ),  
                new[] {1,5,9,13,17,21,25,26}
            ).SetName("mytesthelp7"),

            new TestCaseData(
                new TestConsole(
                    (Read, "help"),
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "exit"),
                    (Write, "Такой команды нет"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "end"),
                    (Read, "exit")
                ),
                new[] {0,1,2,3,4}
            ).SetName("mytesthelp2"),

            new TestCaseData(
                new TestConsole(
                    (Read, "help"),
                    (Write, "Укажите команду, для которой хотите посмотреть помощь"),
                    (Write, "Доступные команды: add, median, rand"),
                    (Write, "Чтобы выйти из режима помощи введите end"),
                    (Read, "end"),
                    (Read, "exit")
                ),
                new[] {0,1,2,3,4}
            ).SetName("mytesthelp1"),

            new TestCaseData(
                new TestConsole(
                    (Read, "add"),
                    (Read, "15"),
                    (Read, "60"),
                    (Write, "75"),
                    (Read, "exit")
                ),
                new[] {2}
            ).SetName("add"),
            new TestCaseData(
                new TestConsole(
                    (Read, "median"),
                    (Read, "3"),
                    (Read, "60"),
                    (Read, "50"),
                    (Read, "41"),
                    (Write, "50"),
                    (Read, "exit")
                ),
                new[] {1, 4}
            ).SetName("median"),
            new TestCaseData(
                new TestConsole(
                    (Read, "rand"),
                    (Read, "2"),
                    (Write, "420"),
                    (Write, "7058940"),
                    (Read, "exit")
                ),
                new[] {1}
            ).SetName("rand")

           
        };

        [Test]
        [TestCaseSource(nameof(RegularCases))]
        public void Run_As_Expected(TestConsole console)
        {
            var storage = new MemoryStorage();
            var interpreter = new StatefulInterpreter();
            interpreter.Run(console, storage);
            console.AtEnd.Should().BeTrue();
        }

        [Test]
        [TestCaseSource(nameof(InterruptionCases))]

        public void Run_With_Interruptions(TestConsole console, int[] failureSchedule)
        {
            var storage = new MemoryStorage();
            var brokenConsole = new BrokenConsole(console, failureSchedule);
            for (var i = 0; i < failureSchedule.Length; i++)
            {
                var exception = Assert.Throws<TestException>(() =>
                {
                    var interpreter = new StatefulInterpreter();
                    interpreter.Run(brokenConsole, storage);
                });
                exception.Type.Should().Be(TestException.ExceptionType.InducedFailure);
            }

            var finalInterpreter = new StatefulInterpreter();
            finalInterpreter.Run(brokenConsole, storage);

            console.AtEnd.Should().BeTrue();
        }
    }
}