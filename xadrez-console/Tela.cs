﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez_console
{
    internal class Tela
    {
        public static void imprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.NumLinha; i++)
            {
                for (int j = 0; j < tab.NumColunas; j++)
                {
                    if (tab.Peca(i, j) == null)
                    {
                        Console.Write("- ");
                    } else
                    {
                        Console.Write(tab.Peca(i, j) + " ");
                    }
                    
                }
                Console.WriteLine();
            }
        }
    }
}
