using System;
using System.Collections.Generic;
using System.Linq;

namespace Sucessao
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> linhas = LerLinhas();

            var linha1 = linhas[0].Split(" ");

            int qtdeRelacao = int.Parse(linha1[0]);
            int qtdeReclamantes = int.Parse(linha1[1]);

            List<Pessoa> pessoas = new List<Pessoa>();

            pessoas.Add(Pessoa.Fundador(linhas[1]));

            int cont = 2;

            for (int i = 0; i < qtdeRelacao; i++)
            {
                if (linhas[cont].Any(c => char.IsDigit(c)))
                {
                    cont++;
                    continue;
                }

                Pessoa.Relacao(linhas[cont], pessoas);
                cont++;
            }

            for (int i = 0; i < qtdeReclamantes; i++)
            {
                if (linhas[cont].Any(c => char.IsDigit(c)))
                {
                    cont++;
                    continue;
                }

                Pessoa.Reclamante(linhas[cont], pessoas);
                cont++;
            }

            var sucessor = pessoas
                            .Where(c => c.EhReclamante)
                            .OrderByDescending(c => c.QtdeSangueReal)
                            .FirstOrDefault();

            Console.WriteLine(sucessor.Nome);
        }

        private static List<string> LerLinhas()
        {
            var linha = Console.ReadLine();

            List<string> linhas = new List<string>();

            linhas.Add(linha);

            while (!linha.Equals(""))
            {
                linha = Console.ReadLine();

                linhas.Add(linha);
            }

            return linhas;
        }
    }

    class Pessoa
    {
        public string Nome { get; set; }
        public double QtdeSangueReal { get; set; }
        public bool EhReclamante { get; set; }

        private Pessoa(string nome, double qtdeSangueReal, bool reclamante)
        {
            Nome = nome;
            QtdeSangueReal = qtdeSangueReal;
            EhReclamante = reclamante;
        }

        public static Pessoa Fundador(string linha)
        {
            return new Pessoa(linha, 1.0, false);
        }

        public static void Reclamante(string linha, List<Pessoa> pessoas)
        {
            var reclamante = pessoas.Where(c => c.Nome.Equals(linha)).FirstOrDefault();

            if (reclamante == null)
            {
                pessoas.Add(new Pessoa(linha, 0.5, true));
            }
            else
            {
                reclamante.EhReclamante = true;
            }
        }

        public static void Relacao(string linha, List<Pessoa> pessoas)
        {
            string[] dados = linha.Split(' ');

            var pai = pessoas.Where(c => c.Nome.Equals(dados[1])).FirstOrDefault();

            if (pai == null)
            {
                pai = new Pessoa(dados[1], 0.0, false);

                pessoas.Add(pai);
            }

            var mae = pessoas.Where(c => c.Nome.Equals(dados[2])).FirstOrDefault();

            if (mae == null)
            {
                mae = new Pessoa(dados[2], 0.0, false);

                pessoas.Add(mae);
            }

            var filho = new Pessoa(dados[0], (pai.QtdeSangueReal / 2.0) + (mae.QtdeSangueReal / 2.0), false);

            pessoas.Add(filho);
        }
    }
}
