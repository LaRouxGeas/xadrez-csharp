namespace tabuleiro
{
    internal class Tabuleiro
    {
        public int NumLinha { get; set; }
        public int NumColunas { get; set; }
        private Peca[,] pecas;

        public Tabuleiro(int numLinha, int numColunas)
        {
            NumLinha = numLinha;
            NumColunas = numColunas;
            pecas = new Peca[numLinha, numColunas];
        }

        public Peca Peca(int linha, int colunas)
        {
            return pecas[linha, colunas];
        }

        public void ColocarPeca(Peca p, Posicao pos)
        {
            pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }
    }
}