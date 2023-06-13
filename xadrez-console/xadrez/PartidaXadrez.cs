using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez
{
    internal class PartidaXadrez
    {
        public Tabuleiro Tabuleiro { get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; private set; }

        public PartidaXadrez()
        {
            Tabuleiro = new Tabuleiro(8,8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tabuleiro.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = Tabuleiro.retirarPeca(destino);
            Tabuleiro.colocarPeca(p, destino);
        }

        public void colocarPecas()
        {
            Tabuleiro.colocarPeca(new Torre(Tabuleiro, Cor.Branca), new PosicaoXadrez('a', 1).toPosicao());
            Tabuleiro.colocarPeca(new Torre(Tabuleiro, Cor.Branca), new PosicaoXadrez('h', 1).toPosicao());
            Tabuleiro.colocarPeca(new Rei(Tabuleiro, Cor.Branca), new PosicaoXadrez('d', 1).toPosicao());
            Tabuleiro.colocarPeca(new Torre(Tabuleiro, Cor.Branca), new PosicaoXadrez('d', 2).toPosicao());

            Tabuleiro.colocarPeca(new Torre(Tabuleiro, Cor.Preta), new PosicaoXadrez('a', 8).toPosicao());
            Tabuleiro.colocarPeca(new Torre(Tabuleiro, Cor.Preta), new PosicaoXadrez('h', 8).toPosicao());
            Tabuleiro.colocarPeca(new Rei(Tabuleiro, Cor.Preta), new PosicaoXadrez('d', 8).toPosicao());
        }
    }
}
