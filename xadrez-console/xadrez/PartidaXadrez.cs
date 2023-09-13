using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using tabuleiro.Exceptions;
using xadrez_console.xadrez;

namespace xadrez
{
    internal class PartidaXadrez
    {
        public Tabuleiro Tabuleiro { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        
        public PartidaXadrez()
        {
            Tabuleiro = new Tabuleiro(8,8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            terminada = false;
            Xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tabuleiro.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = Tabuleiro.retirarPeca(destino);
            Tabuleiro.colocarPeca(p, destino);
            if (pecaCapturada != null) {
                capturadas.Add(pecaCapturada);
            }

            // jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTP = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTP = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tabuleiro.retirarPeca(origemTP);
                T.incrementarQteMovimentos();
                Tabuleiro.colocarPeca(T, destinoTP);
            }

            // jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTG = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTG = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tabuleiro.retirarPeca(origemTG);
                T.incrementarQteMovimentos();
                Tabuleiro.colocarPeca(T, destinoTG);
            }

            // jogada especial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    } else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = Tabuleiro.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
                
            }

            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tabuleiro.retirarPeca(destino);
            p.decrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                Tabuleiro.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            Tabuleiro.colocarPeca(p, origem);

            // jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTP = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTP = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tabuleiro.retirarPeca(destinoTP);
                T.decrementarQteMovimentos();
                Tabuleiro.colocarPeca(T, origemTP);
            }

            // jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTG = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTG = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tabuleiro.retirarPeca(destinoTG);
                T.decrementarQteMovimentos();
                Tabuleiro.colocarPeca(T, origemTG);
            }

            // jogada especial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tabuleiro.retirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    Tabuleiro.colocarPeca(peao, posP);
                }

            }
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = Tabuleiro.peca(destino);

            // jogada especial promoção
            if(p is Peao)
            {
                if (destino.Linha == 0 || destino.Linha == 7)
                {
                    p = Tabuleiro.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(Tabuleiro, p.Cor);
                    Tabuleiro.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }

            if (estaEmXeque(adversaria(JogadorAtual)))
            {
                Xeque = true; 
            } else
            {
                Xeque = false;
            }

            if (testeXequemate(adversaria(JogadorAtual)))
            {
                terminada = true;
            } else
            {
                Turno++;
                mudaJogador();
            }

            
            // jogada especial En Passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                VulneravelEnPassant = p;
            } else
            {
                VulneravelEnPassant = null;
            }
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
            if (!Tabuleiro.peca(origem).movimentoPossivel(destino))
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

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca pecaCapturada in capturadas)
            {
                if (pecaCapturada.Cor == cor)
                {
                    aux.Add(pecaCapturada);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca pecaEmJogo in pecas)
            {
                if (pecaEmJogo.Cor == cor)
                {
                    aux.Add(pecaEmJogo);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            } else
            {
                return Cor.Branca;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca peca in pecasEmJogo(cor))
            {
                if (peca is Rei)
                {
                    return peca;
                }
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException($"Não tem rei da cor {cor} no tabuleiro!");
            }

            foreach (Peca peca in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = peca.movimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }

            return false;
        }

        public bool testeXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca peca in pecasEmJogo(cor))
            {
                bool[,] mat = peca.movimentosPossiveis();
                for (int i = 0; i < Tabuleiro.NumLinhas; i++)
                {
                    for (int j = 0; j < Tabuleiro.NumColunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = peca.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, new Posicao(i, j));
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tabuleiro.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        public void colocarPecas()
        {
            colocarNovaPeca('a', 8, new Torre(Tabuleiro, Cor.Preta));
            colocarNovaPeca('b', 8, new Cavalo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('c', 8, new Bispo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('d', 8, new Dama(Tabuleiro, Cor.Preta));
            colocarNovaPeca('e', 8, new Rei(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('f', 8, new Bispo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('g', 8, new Cavalo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('h', 8, new Torre(Tabuleiro, Cor.Preta));

            colocarNovaPeca('a', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('b', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('c', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('d', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('e', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('f', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('g', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('h', 7, new Peao(Tabuleiro, Cor.Preta, this));

            colocarNovaPeca('a', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('b', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('c', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('d', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('e', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('f', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('g', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('h', 2, new Peao(Tabuleiro, Cor.Branca, this));

            colocarNovaPeca('a', 1, new Torre(Tabuleiro, Cor.Branca));
            colocarNovaPeca('b', 1, new Cavalo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('d', 1, new Dama(Tabuleiro, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('f', 1, new Bispo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('g', 1, new Cavalo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(Tabuleiro, Cor.Branca));
        }
    }
}
