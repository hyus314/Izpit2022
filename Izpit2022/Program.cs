namespace Izpit2022
{
    using System.Text;
    public class Program
    {
        static void Main(string[] args)
        {
            // променяне на енкодирането на конзолата за работа с UTF8, за да може да работи със знаци на кирилица
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            //въвеждане на броя на стоките
            Console.Write("Въведете колко стоки искате да регистрирате: ");
            int n = int.Parse(Console.ReadLine());
            if (n < 0 || n > 1000)
            {
                // валидация - хвърляне на грешка при недопустими стойности
                throw new Exception("Допустимият интервал е от 0 до 1000");
            }

            // инициализиране на лист от класа Stoka
            var stoki = new List<Stoka>();

            for (int i = 0; i < n; i++)
            {
                //въвеждане и валидиране на входните данни.
                Console.WriteLine("Въвеждане на нова стока.");
                Console.Write("Въведете код: ");
                string kod = Console.ReadLine();
                if (kod.Length > 5)
                {
                    throw new Exception("Броят на знаците на кода не трябва да превишава 5.");
                }
                Console.Write("Въведете име: ");
                string ime = Console.ReadLine();
                if (ime.Length > 60)
                { 
                    throw new Exception("Броят на знаците на името не трябва да превишава 60."); 
                }
                Console.Write("Въведете килограми: ");
                int kilogrami = int.Parse(Console.ReadLine());
                Console.Write("Въведете трайност: ");
                int traynost = int.Parse(Console.ReadLine());
                Console.Write("Въведете групата на стоката (G/S): ");
                char grupaStoki = char.Parse(Console.ReadLine());
                Console.Write("Въведете датата на постъпване в склада: ");
                string dataZaPostupvane = Console.ReadLine();
                Console.Write("Въведете позицията: ");
                string poziciq = Console.ReadLine();
                if (poziciq.Length > 10)
                { 
                    throw new Exception("Броят на знаците на позицията не трябва да превишава 10");
                }

                // ползваме условни оператори и по този начин нашият алгоритъм придобива разклонена семантика
                // при специалните стоки се създава инстанция от класа SpecialnaStoka
                // а при нормалните стоки - NormalStoka
                if (grupaStoki == 'S')
                {
                    //разликата между двата класа се състои в един допълнителен атрибут в класа за специалните стоки и това е температурата
                    Console.Write("Въведете температура за специалната стока: ");
                    double temp = Double.Parse(Console.ReadLine());
                    Stoka specialnaStoka = new SpecialnaStoka(kod, ime, kilogrami, traynost, dataZaPostupvane, poziciq, temp);
                    stoki.Add(specialnaStoka);
                }
                else if (grupaStoki == 'G')
                {
                    // тук няма температура
                    Stoka normalStoka = new NormalStoka(kod, ime, kilogrami, traynost, dataZaPostupvane, poziciq);
                    stoki.Add(normalStoka);
                }
                else
                {
                    throw new Exception("Инвалидна група за стока.");
                }
            }

            // въвели сме стоките и те са регистрирани в нашата структура от данни.

            Console.WriteLine("Справка на всички стоки: ");

            foreach (var stoka in stoki.OrderBy(x => x.dataZaPostupvane))
            {
                // тук няма разклонителни оператори за двата класа-наследници, понеже и в двата класа е презаписан метода ToString()
                // разликата е, че в специалните стоки ще изпечата и температурата на конзолата
                Console.WriteLine(stoka.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Справка на СПЕЦИАЛНИ стоки: ");
            Console.WriteLine();

            //нареждане на списъка за стоките в реда по условие и извеждане на данните на конзоата.
            
            foreach (var stoka in stoki.Where(x => x.GetType().Name == nameof(SpecialnaStoka)).OrderBy(x => x.dataZaPostupvane).ThenBy(x => x.traynost))
            {
                Console.WriteLine(stoka.ToString());
            }

            // началото на функционалността за търсене
            // въвеждане на кода, по който ще се търсят стоките 

            Console.WriteLine();
            Console.Write("Въведете код за търсене на определена стока: ");
            string kodZaTursene = Console.ReadLine();
            int obshtoKolichestvo = 0;
            double minimalnaTemperatura = int.MaxValue;
            
            // проверка за наличието на продукт с такъв код

            if (stoki.Where(x => x.kod == kodZaTursene).Any())
            {
                Console.WriteLine("Не са налични продукти с този код.");
            }
            else
            {
                // използвано е итеративен цикъл за стоките, защото могат да съществуват няколко стоки с един и същ код
                foreach (var stoka in stoki.Where(x => x.kod == kodZaTursene))
                {
                    obshtoKolichestvo += stoka.kilogrami;
                    Console.WriteLine(stoka.ToString());
                    if (stoka.GetType().Name == nameof(SpecialnaStoka))
                    {
                        // намиране на минималната нужна температура
                        var specialnaStoka = (SpecialnaStoka)stoka;
                        if (specialnaStoka.temp < minimalnaTemperatura)
                        {
                            minimalnaTemperatura = specialnaStoka.temp;
                        }
                    }
                }

                // извеждане на резултата
                Console.WriteLine($"Общо количество: {obshtoKolichestvo}");
                if (stoki.Any(x => x.GetType().Name == nameof(SpecialnaStoka)))
                {
                    Console.WriteLine($"Минимална нужна температура за специалните стоки: {minimalnaTemperatura.ToString("f1")}");
                }
            }
        }
    }
    // инициализиране на абстрактен клас, който ще бъде родител на двата класа - обикновено изделие и специално
    // като разликата ще се състои в един атрибут и то е именно - температурата.
    public abstract class Stoka
    {
        public Stoka(string kod, string ime, int kilogrami, int traynost, string dataZaPostupvane, string poziciq)
        {
            this.kod = kod;
            this.ime = ime;
            this.kilogrami = kilogrami;
            this.traynost = traynost;
            this.dataZaPostupvane = dataZaPostupvane;
            this.poziciq = poziciq;
        }

        public string kod;
        public string ime;
        public int kilogrami;
        public int traynost;
        public string dataZaPostupvane;
        public string poziciq;
    }

    public class NormalStoka : Stoka
    {
        public NormalStoka(string kod, string ime, int kilogrami, int traynost, string dataZaPostupvane, string poziciq) :
            base(kod, ime, kilogrami, traynost, dataZaPostupvane, poziciq)
        {
            this.kod = kod;
            this.ime = ime;
            this.kilogrami = kilogrami;
            this.traynost = traynost;
            this.dataZaPostupvane = dataZaPostupvane;
            this.poziciq = poziciq;
        }

        public override string ToString()
        {
            return $"{this.poziciq}, {this.kod}, {this.ime}, {this.kilogrami} кг., {this.dataZaPostupvane}, {this.traynost}";
        }
    }

    public class SpecialnaStoka : Stoka
    {
        public SpecialnaStoka(string kod, string ime, int kilogrami, int traynost, string dataZaPostupvane, string poziciq, double temp) :
            base(kod, ime, kilogrami, traynost, dataZaPostupvane, poziciq)
        {
            this.kod = kod;
            this.ime = ime;
            this.kilogrami = kilogrami;
            this.traynost = traynost;
            this.dataZaPostupvane = dataZaPostupvane;
            this.poziciq = poziciq;
            this.temp = temp;
        }
        public double temp;
        public override string ToString()
        {
            return $"{this.poziciq}, {this.kod}, {this.ime}, {this.kilogrami} кг., {this.dataZaPostupvane}, {this.traynost}, tC={this.temp.ToString("f1")}";
        }
    }
}