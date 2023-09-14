using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using tabuleiro.Exceptions;
using xadrez;

namespace xadrez_console
{
    internal class Tela
    {
        public static void imprimirPartida(PartidaXadrez partida)
        {
            imprimirTabuleiro(partida.Tabuleiro);
            Console.WriteLine();

            if (partida.PeaoParaPromocao != null)
            {
                Console.Write("Promoção do Peão! ");
                imprimirPecasPromocao(partida);
                lerEscolhaPromocao(partida);
                Console.Clear();
                imprimirTabuleiro(partida.Tabuleiro);
                Console.WriteLine();
            } 
            
            imprimirPecasCapturadas(partida);

            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.Turno);

            if (!partida.terminada)
            {
                Console.WriteLine("Aguardando jogada: " + partida.JogadorAtual);

                if (partida.Xeque)
                {
                    Console.WriteLine("Xeque!");
                }
            } else
            {
                Console.WriteLine("Xequemate!");
                Console.WriteLine("Vencedor: " + partida.JogadorAtual);
            }
            
        }

        public static void imprimirPecasCapturadas(PartidaXadrez partida)
        {
            Console.WriteLine("Peças capturadas:");

            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.Write("Brancas: ");
            imprimirConjunto(partida.pecasCapturadas(Cor.Branca));
            Console.WriteLine();
            Console.ForegroundColor = aux;

            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.Write("Pretas: ");
            imprimirConjunto(partida.pecasCapturadas(Cor.Preta));
            Console.WriteLine();
            Console.ForegroundColor = aux;
        }

        public static void imprimirPecasPromocao(PartidaXadrez partida)
        {
            Console.WriteLine("Escolha uma das seguintes opções para promover o Peão: ");

            ConsoleColor aux = Console.ForegroundColor;
            if (partida.PeaoParaPromocao.Cor == Cor.Branca)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            } else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }

            Console.Write("Dama [D], Cavalo [C], Bispo [B], Torre [T]");
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void imprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write('[');
            foreach (Peca peca in conjunto)
            {
                Console.Write(peca + " ");
            }
            Console.Write("]");
        }

        public static void imprimirTabuleiro(Tabuleiro tab)
        {
            Console.WriteLine("  A B C D E F G H");
            for (int i = 0; i < tab.NumLinhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.NumColunas; j++)
                {
                    imprimirPeca(tab.peca(i, j));
                }
                Console.Write(8 - i);
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
        }

        public static void imprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            Console.WriteLine("  A B C D E F G H");
            for (int i = 0; i < tab.NumLinhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.NumColunas; j++)
                {
                    if (posicoesPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    imprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.Write(8 - i);
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
        }

        public static void imprimirPeca(Peca peca)
        {
            if (peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (peca.Cor == Cor.Branca)
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        public static void lerEscolhaPromocao(PartidaXadrez partida)
        {
            char[] entradasPermitidas = {'D', 'C', 'B', 'T'};
            string entrada = Console.ReadLine();
            if (entrada == null || entrada.Length != 1)
            {
                throw new TabuleiroException("Erro na entrada de dados! Escolha uma opção");
            }
            if (!char.IsLetter(entrada[0]))
            {
                throw new TabuleiroException("Erro na entrada de dados! Informar uma letra primeiro");
            }
            if (!entradasPermitidas.Contains(char.ToUpper(entrada[0])) ) {
                throw new TabuleiroException("Erro na entrada de dados! Informar uma letra primeiro");
            }
            partida.promover(char.ToUpper(entrada[0]));
        }


        public static PosicaoXadrez lerPosicaoXadrez()
        {
            string entrada = Console.ReadLine();
            if (entrada.Length != 2)
            {
                throw new TabuleiroException("Erro na entrada de dados! Tamanho errado");
            }
            char coluna = entrada[0];
            if (!char.IsLetter(coluna))
            {
                throw new TabuleiroException("Erro na entrada de dados! Informar uma letra primeiro");
            }
            coluna = char.ToLower(coluna);
            // ASCII 97 = a, 104 = h
            if ((coluna < 97 || coluna > 104)) 
            {
                throw new TabuleiroException("Erro na entrada de dados! A letra precisa ser entre 'A' até 'H'");
            }
            if (!Char.IsDigit(entrada[1]))
            {
                throw new TabuleiroException("Erro na entrada de dados! Informar um número");
            }
            int linha = int.Parse(entrada[1] + "");
            if (linha < 0 && linha > 9)
            {
                throw new TabuleiroException("Erro na entrada de dados! Informar um número entre 1 a 8");
            }
            return new PosicaoXadrez(coluna, linha);
        }
    }
}
