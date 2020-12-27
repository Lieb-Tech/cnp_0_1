using Medication;
using System;
using System.IO;
using System.Linq;

namespace cnp_0_1
{
    class Program
    {
        static void Main(string[] args)
        {
            // generateTestMeds();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Hello World!");

            // var text = File.ReadAllLines(@"C:\\NLP\\n2c2\\1.txt");
            // var text = File.ReadAllLines(@"C:\\NLP\\n2c2\\2.txt");
            // var text = File.ReadAllLines(@"C:\\NLP\\n2c2\\3.txt");
            // var text = File.ReadAllLines(@"C:\\NLP\\n2c2\\4.txt");
            // var text = File.ReadAllLines(@"C:\\NLP\\n2c2\\5.txt");
            //var text = File.ReadAllLines(@"C:\\NLP\\n2c2\\6.txt");
            // var text = File.ReadAllLines(@"C:\\NLP\\n2c2\\7.txt");
            //var text = File.ReadAllLines(@"C:\\NLP\\n2c2\\8.txt");
            var text = File.ReadAllLines(@"C:\\NLP\\n2c2\\9.txt");

            var sp = new TextParser.SectionParser();
            var sections = sp.ParseText(text);

            var mp = new MedicationProcessor();
            foreach (var section in sections)
            {
                bool displayed = false;
                foreach (var line in section.Lines)
                {
                    var result = mp.Process(line.Original);

                    foreach (var med in result.medications.Where(z => z.Confidence < 10).OrderBy(z => z.Confidence))
                    {
                        if (!displayed)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"{section.Header}");
                            displayed = true;
                        }

                        string conf = med.Confidence.ToString("N2");
                        if (med.Confidence > 4.999)
                            Console.ForegroundColor = ConsoleColor.Green;
                        else
                            Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine($"{conf}\t\t{med}\t\t {med.OriginalText}");
                        
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void generateTestMeds()
        {
            var med = @"C:\\NLP\\n2c2\\med.txt";
            if (File.Exists(med))
                File.Delete(med);

            var files = new DirectoryInfo(@"C:\\NLP\\n2c2").GetFiles("*.txt");
            foreach (var file in files)
            {
                if (file.Name == "med.txt")
                    continue;

                var text = File.ReadAllLines(file.FullName);

                foreach (var line in text.Where(z => z.ToLower().Trim() != "stable"))
                {
                    if (line.Contains("MG", StringComparison.InvariantCultureIgnoreCase)
                        || line.Contains("TAB", StringComparison.InvariantCultureIgnoreCase)
                        || line.Contains("CAP", StringComparison.InvariantCultureIgnoreCase))
                    {
                        File.AppendAllText(med, line + "\r\n");
                    }
                }
            }

        }
    }
}
