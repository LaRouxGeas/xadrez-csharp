using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez
{
    internal class Rei : Peca
    {
        public Rei(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "R";
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.NumLinhas, tab.NumColunas];

            Posicao pos = new Posicao(0, 0);

            // Cima
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            // Diagonal Superior Direita
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            // Direita
            pos.definirValores(Posicao.Linha, Posicao.Coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            // Diagonal Inferior Direita
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            // Baixo
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            // Diagonal Inferior Esquerda
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            // Esquerda
            pos.definirValores(Posicao.Linha, Posicao.Coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            // Diagonal Superior Esquerda
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            return mat;
        }
    }
}
