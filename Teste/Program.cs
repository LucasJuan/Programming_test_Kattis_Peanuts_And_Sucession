using System;
using System.Collections.Generic;
using System.Linq;

namespace Teste
{
    class Program
    {
        public List<Variavel> Variaveis { get; set; }

        static void Main(string[] args)
        {
            var p = new Program() {
                Variaveis = new List<Variavel>()
            };

            Console.WriteLine("Pronto!");

            while (p.Iterar()){

            }
        }

        public bool Iterar()
        {
            string linha = Console.ReadLine();

            if (linha.Equals("fim"))
            {
                return false;
            }

            GetVariavelExpressao(linha, out string nomeVariavel, out string expressao);

            var itensExpressao = ValidarExpressao(expressao);

            var v = CriarVariavel(nomeVariavel, itensExpressao);

            SetValor(v);

            return true;
        }

        public void SetValor(Variavel v)
        {
            int valor = 0;
            int contSemValor = 0;

            foreach (var item in v.ItensExpressao)
            {
                if (int.TryParse(item, out int o))
                {
                    valor += o;
                }
                else
                {
                    var d = v.Dependencias.Where(c => c.Nome.Equals(item)).FirstOrDefault();

                    if (d == null)
                    {
                        d = CriarVariavel(item, null, v);
                        
                        v.Dependencias.Add(d);
                    }                    

                    if (d.Valor.HasValue)
                    {
                        valor += d.Valor.Value; 
                    }
                    else
                    {
                        contSemValor++;
                    }
                }
            }

            if (contSemValor == 0)
            {
                v.Valor = valor;
                Console.WriteLine("===> " + v.Nome + " = " + v.Valor.Value);

                foreach (var item in v.Dependentes)
                {
                    SetValor(item);
                }
            }
        }

        public List<string> ValidarExpressao(string s)
        {
            var a = s.Split("+");

            var l = new List<string>();

            foreach (var item in a)
            {
                l.Add(item.Trim());
            }

            return l;
        }

        public void GetVariavelExpressao(string linha, out string variavel, out string expressao)
        {
            var a = linha.Split("=");

            if (a.Length != 2)
            {
                throw new Exception("Linha irregular!");
            }

            variavel = a[0].Trim();
            expressao = a[1];
        }

        public Variavel CriarVariavel(string nome, List<string>? itensExpressao)
        {
            var v = Variaveis.Where(c => c.Nome.Equals(nome)).FirstOrDefault();

            if (v == null)
            {
                v = new Variavel()
                {
                    Nome = nome,
                    ItensExpressao = itensExpressao,
                    Dependentes = new List<Variavel>(),
                    Dependencias = new List<Variavel>()
                };

                Variaveis.Add(v);

                return v;
            }
            else
            {
                if (itensExpressao != null)
                {
                    v.ItensExpressao = itensExpressao;
                }

                return v;
            }
        }

        public Variavel CriarVariavel(string nome, List<string>? itensExpressao, Variavel dependente)
        {
            var v = CriarVariavel(nome, itensExpressao);

            v.Dependentes.Add(dependente);

            return v;
        }

    }

    class Variavel
    {
        public string Nome { get; set; }
        public int? Valor { get; set; }
        public List<string>? ItensExpressao { get; set; }
        public List<Variavel> Dependentes { get; set; }
        public List<Variavel> Dependencias { get; set; }
    }
}
