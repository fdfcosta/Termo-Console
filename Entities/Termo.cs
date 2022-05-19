using System.Globalization;
using System.Text;

namespace TermoConsole.Entities
{
    public class Termo
    {
        private int Tentativas { get; set; } = 0;
        private List<string> ListaPalavras = new();
        private List<string> Alfabeto = new() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private List<string> LetrasDescartadas = new();
        private List<string> LetrasNaPalavra = new();
        private List<string> LetrasNaPosicao = new();
        private List<string> PalavrasTestadas = new();
        private string PalavraSorteada { get; set; }
        private bool Venceu = false;

        public void Partida()
        {
            GeraLista();
            PalavraSorteada = DefinePalavraSorteada();

            while (Tentativas < 6)
            {

                ImprimeMenu();
                ListaJogadas();


                string palavraTestada = DefinePalavraTestada();
                PalavrasTestadas.Add(palavraTestada); 
                Jogada(PalavraSorteada, palavraTestada);

                Tentativas++;


                if (VerificarJogada(PalavraSorteada, palavraTestada))
                {
                    Venceu = true;
                    break;
                }
                else
                {
                    Console.Clear();
                }

            }

            if (Venceu == true)
            {
                Console.Clear();

                ImprimeMenu();

                ListaJogadas();

                Console.WriteLine($"Parabéns! Você acertou a palavra secreta: {PalavraSorteada}");
            }
            else
            {
                Console.Clear();

                ImprimeMenu();
                ListaJogadas();

                Console.WriteLine();

                Console.WriteLine("Você esgotou o número de tentativas. Por favor, tente novamente.");
                Console.WriteLine($"A palavra secreta era: {PalavraSorteada}");
                Console.ReadKey();
            }
        }

        private void GeraLista()
        {
            string sourcePath = @"C:\Users\User\repos\projetos\TermoConsole\palavras.txt";

            string[] Files;

            Files = File.ReadAllLines(sourcePath);

            StringBuilder sb = new();

            foreach (string i in Files)
            {
                var PalavraComAcento = i.Normalize(NormalizationForm.FormD).ToCharArray();

                int QtdLetras = 0;

                while (QtdLetras < 5)
                {
                    foreach (char letter in PalavraComAcento)
                    {

                        if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                            sb.Append(letter);

                        QtdLetras++;

                    }
                }
                sb.AppendLine();

            }

            string[] ArrayPalavrasSemAcento = sb.ToString().Split(@"
");

            foreach (string i in ArrayPalavrasSemAcento)
            {
                ListaPalavras.Add(i.ToUpper());
            }

        }

        private void ImprimeMenu()
        {

            Console.WriteLine(@"                        =========TERMO.CONSOLE==========

Como jogar?

    Você terá 6 tentativas para encontrar a palavra secreta.
    A cada tentativa, as cores mostram o quão perto você está da solução.
    
    Letras em azul fazem parte da palavra secreta e estão na posição correta
    Letras em magenta fazem parte da palavra secreta, mas não estão na posição correta.
    

");

            ImprimeAlfabeto();
            

            Console.WriteLine();

            Console.WriteLine(@$"Tentativas restantes: {6 - Tentativas}
");

        }

        private void ImprimeAlfabeto()
        {
            Console.WriteLine("ALFABETO RESTANTE:");
            foreach (var i in Alfabeto)
            {
                if (!LetrasDescartadas.Contains(i))
                {
                    if (LetrasNaPalavra.Contains(i))
                    {

                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.Write($"{i}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");

                    }
                    else if (LetrasNaPosicao.Contains(i))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write($"{i}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write($"{i} ");
                    }


                }
            }
        }

        public void Jogada(string palavraSorteada, string palavraTestada)
        {

            foreach (var palavra in PalavrasTestadas)
            {
                for (byte i = 0; i < palavra.Length; i++)
                {
                    if (palavraSorteada[i] == palavra[i])
                    {
                        Console.Write("[");
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write($"{palavra[i]}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("] ");

                        LetrasNaPosicao.Add(palavra[i].ToString());

                    }
                    else if (palavraSorteada.Contains(palavra[i]))
                    {
                        Console.Write("[");
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.Write($"{palavra[i]}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("] ");

                        LetrasNaPalavra.Add(palavra[i].ToString());
                    }
                    else
                    {
                        Console.Write($"[{palavra[i]}] ");
                        LetrasDescartadas.Add(palavra[i].ToString().ToUpper()) ; ;

                    }
                }

                Console.WriteLine();
            }

            int QuantidadeLinhasSemJogadas = 6 - Tentativas;

            for (byte i = 0; i < QuantidadeLinhasSemJogadas; i++)
            {
                Console.WriteLine("[_] [_] [_] [_] [_]");
            }


        }

        private void ListaJogadas()
        {

            foreach (var palavra in PalavrasTestadas)
            {
                for (byte i = 0; i < palavra.Length; i++)
                {
                    if (PalavraSorteada[i] == palavra[i])
                    {
                        Console.Write("[");
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write($"{palavra[i]}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("] ");

                    }
                    else if (PalavraSorteada.Contains(palavra[i]))
                    {
                        Console.Write("[");
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.Write($"{palavra[i]}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("] ");
                    }
                    else
                    {
                        Console.Write($"[{palavra[i]}] ");
                        LetrasDescartadas.Add(palavra[i].ToString());
                    }
                }

                Console.WriteLine();
            }

            int QuantidadeLinhasSemJogadas = 6 - Tentativas;

            for (byte i = 0; i < QuantidadeLinhasSemJogadas; i++)
            {
                Console.WriteLine("[_] [_] [_] [_] [_]");
            }


        }

        private string DefinePalavraSorteada()
        {
            string palavraSorteada = ListaPalavras[new Random().Next(ListaPalavras.Count)];

            return palavraSorteada;
        }

        private string DefinePalavraTestada()
        {

            string palavraTestada = "";

            try
            {
                bool boolean = false;


                while (!boolean)
                {
                    Console.WriteLine("Informe uma palavra de até 5 caracteres:");
                    palavraTestada = Console.ReadLine().Trim().ToUpper();

                    if (VerificaPalavraTestada(palavraTestada))
                    {
                        boolean = true;
                    }
                        
                    else
                    {
                        boolean = false;
                    }

                }

            }
            catch (Exception ex)
            {
                palavraTestada = "";
            }

            return palavraTestada.ToUpper();
        }

        public bool VerificaPalavraTestada(string palavra)
        {
            bool PossuiDigitoOuNumero = false;
            bool LetraJaUtilizada = false;
            bool PalavraExistente = ListaPalavras.Contains(palavra);

            foreach (char letra in palavra)
            {
                if (LetrasDescartadas.Contains(letra.ToString().ToUpper()))
                {
                    LetraJaUtilizada = true;
                    break;
                }
                else if (!char.IsLetter(letra) || char.IsDigit(letra))
                {
                    PossuiDigitoOuNumero = true;
                    break;
                }
                
                
            }

            if (palavra.Length == 5 && !PossuiDigitoOuNumero && !LetraJaUtilizada && PalavraExistente)
            {
                return true;
            }
            else if (PossuiDigitoOuNumero)
            {
                Console.WriteLine("Erro. Todos os caracteres precisam ser letras e não possuir acento.");
                return false;
            }
            else if (LetraJaUtilizada)
            {
                Console.WriteLine("A palavra utilizada possui letra(s) já utilizada(s). Verifique o alfabeto acima e insira uma palavras com as letras disponíveis.");
                return false;
            }
            else if (!PalavraExistente)
            {
                Console.WriteLine("Esta palavra não é aceita.");
                return false;
            }
            else
            {
                Console.WriteLine("Palavra deve ter 5 caracteres.");
                return false;
            }



        }

        private bool VerificarJogada(string palavraSorteada, string palavraTestada)
        {
            if (palavraSorteada == palavraTestada)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

    }

}



