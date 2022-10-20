using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Amendoins
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> linhas = LerLinhas();

            var i = 0;
            List<Box> caixas = new List<Box>();
            List<Peanut> amendoins = new List<Peanut>();

            var cont = 0;

            while (i < linhas.Count)
            {
                var qtdeBox = int.Parse(linhas[i]);

                if (qtdeBox == 0)
                {
                    break;
                }

                i++;

                cont = 0;

                while (cont < qtdeBox)
                {
                    caixas.Add(new Box(linhas[i]));
                    cont++;
                    i++;
                }

                var qtdeAmendoins = int.Parse(linhas[i]);

                i++;

                cont = 0;

                while (cont < qtdeAmendoins)
                {
                    amendoins.Add(new Peanut(linhas[i]));
                    cont++;
                    i++;
                }

                ProcessarSaida(caixas, amendoins);

                Console.WriteLine("");

                caixas.Clear();
                amendoins.Clear();
            }


        }

        private static void ProcessarSaida(List<Box> caixas, List<Peanut> amendoins)
        {
            foreach (var item in amendoins)
            {
                var caixa = caixas.Where(c => c.X1 <= item.X &&
                                              c.X2 >= item.X &&
                                              c.Y1 <= item.Y &&
                                              c.Y2 >= item.Y)
                                .FirstOrDefault();

                if (caixa == null)
                {
                    Console.WriteLine(item.Size + " floor");
                }
                else if (caixa.Size.Equals(item.Size))
                {
                    Console.WriteLine(item.Size + " correct");
                }
                else
                {
                    Console.WriteLine(item.Size + " " + caixa.Size);
                }
            }
        }

        private static List<string> LerLinhas()
        {
            var linha = Console.ReadLine();

            List<string> linhas = new List<string>();

            linhas.Add(linha);

            while (!linha.Equals("0"))
            {
                linha = Console.ReadLine();

                linhas.Add(linha);
            }

            return linhas;
        }
    }

    class Box
    {
        public string Size { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

        public Box(string linha)
        {
            string[] dados = linha.Split(' ');

            Size = dados[4];
            X1 = double.Parse(dados[0], CultureInfo.InvariantCulture);
            X2 = double.Parse(dados[2], CultureInfo.InvariantCulture);
            Y1 = double.Parse(dados[1], CultureInfo.InvariantCulture);
            Y2 = double.Parse(dados[3], CultureInfo.InvariantCulture);
        }
    }

    class Peanut
    {
        public string Size { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public Peanut(string linha)
        {
            string[] dados = linha.Split(' ');

            Size = dados[2];
            X = double.Parse(dados[0], CultureInfo.InvariantCulture);
            Y = double.Parse(dados[1], CultureInfo.InvariantCulture);
        }
    }
}
