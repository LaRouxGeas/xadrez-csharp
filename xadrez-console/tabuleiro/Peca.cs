using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabuleiro
{
    abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; set; }
        public int QteMovimentos { get; set; }
        public Tabuleiro tab { get; set; }

        public Peca(Tabuleiro tab, Cor cor)
        {
            this.tab = tab;
            Cor = cor;
        }

        public void incrementarQteMovimentos()
        {
            QteMovimentos++;
        }

        protected bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.Cor != Cor;
        }

        public abstract bool[,] movimentosPossiveis();

    }
}
