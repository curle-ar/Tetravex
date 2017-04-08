using System;
using System.Collections.Generic;

namespace AnalisisTetra
{
    class node
    {
        public Tile tile;
        public bool isFit;
        public List<node> possibleMoves;

        public node()
        {
            this.tile = new Tile(0, 0, 0, 0);
            this.isFit = false;
            this.possibleMoves = new List<node>();
        }
    }
}
