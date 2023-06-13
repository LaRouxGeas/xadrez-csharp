using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez
{
    internal class Torre : Peca
    {
        public Torre(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "T";
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.NumLinhas, tab.NumColunas];

            Posicao pos = new Posicao(0, 0);

            // Cima
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.existePeca(pos) && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Linha--;
            }

            // Abaixo
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.existePeca(pos) && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Linha++;
            }

            // Direita
            pos.definirValores(Posicao.Linha, Posicao.Coluna + 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.existePeca(pos) && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Coluna++;
            }

            // Esquerda
            pos.definirValores(Posicao.Linha, Posicao.Coluna - 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.existePeca(pos) && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Coluna--;
            }

            return mat;
        }
    }
}
