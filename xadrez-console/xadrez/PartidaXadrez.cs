using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using tabuleiro.Exceptions;

namespace xadrez
{
    internal class PartidaXadrez
    {
        public Tabuleiro Tabuleiro { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool terminada { get; private set; }

        public PartidaXadrez()
        {
            Tabuleiro = new Tabuleiro(8,8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
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

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            executaMovimento(origem, destino);
            Turno++;
            mudaJogador();
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (Tabuleiro.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (JogadorAtual != Tabuleiro.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!Tabuleiro.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tabuleiro.peca(origem).podeMoverPeca(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!"); 
            }
        }

        private void mudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            } else
            {
                JogadorAtual = Cor.Branca;
            }
            
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
