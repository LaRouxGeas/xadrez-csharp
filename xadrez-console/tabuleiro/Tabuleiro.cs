using tabuleiro.Exceptions;

namespace tabuleiro
{
    internal class Tabuleiro
    {
        public int NumLinhas { get; set; }
        public int NumColunas { get; set; }
        private Peca[,] pecas;

        public Tabuleiro(int numLinha, int numColunas)
        {
            NumLinhas = numLinha;
            NumColunas = numColunas;
            pecas = new Peca[numLinha, numColunas];
        }

        public Peca peca(int linha, int colunas)
        {
            return pecas[linha, colunas];
        }

        public Peca peca(Posicao pos)
        {
            return pecas[pos.Linha, pos.Coluna];
        }

        public bool existePeca(Posicao posicao)
        {
            validarPosicao(posicao);
            return peca(posicao) != null;
        }

        public void colocarPeca(Peca p, Posicao pos)
        {
            if (existePeca(pos))
            {
                throw new TabuleiroException("Já existe uma peça nessa posição!");
            }
            pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }

        public bool posicaoValida(Posicao posicao)
        {
            if (posicao.Linha < 0 || posicao.Linha >= NumLinhas || posicao.Coluna < 0 || posicao.Coluna >= NumColunas)
            {
                return false;
            }
            return true;
        }

        public void validarPosicao(Posicao posicao)
        {
            if (!posicaoValida(posicao))
            {
                throw new TabuleiroException("Posição inválida!");
            }
        }
    }
}