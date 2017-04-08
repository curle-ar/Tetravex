using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisTetra
{
    class Singleton
    {

        private static Singleton _instance;

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Singleton();
                return _instance;
            }
        }

        // Declare vars here...
        private int c = 0;  // Comparator counter
        private int a = 0;  // Assignment counter

        // [Top, Right, Bottom, Left]

        public Tile[,] n2game = new Tile[2, 2] { {new Tile(3,5,5,4), new Tile(1,4,7,5)},
                                                 {new Tile(5,8,9,8), new Tile(7,8,4,9)} };


        public Tile[,] n3game = new Tile[3, 3] { {new Tile(3,0,5,4), new Tile(5,8,4,0), new Tile(5,4,6,1)},
                                                  {new Tile(6,7,5,1), new Tile(5,6,0,7), new Tile(8,5,3,2)},
                                                  {new Tile(6,5,5,5), new Tile(4,5,0,6), new Tile(6,2,5,9)}};

        public Tile[,] n6game = new Tile[6, 6] { {new Tile(9,9,0,8), new Tile(1,8,7,3), new Tile(4,6,9,8), new Tile(3,9,4,3), new Tile(3,6,4,6), new Tile(5,8,6,9)},
                                                  {new Tile(7,7,0,5), new Tile(6,1,5,6), new Tile(8,6,3,6), new Tile(3,0,4,8), new Tile(4,3,1,6), new Tile(1,8,1,0)},
                                                  {new Tile(4,6,5,6), new Tile(8,9,7,9), new Tile(9,1,7,9), new Tile(9,1,0,7), new Tile(9,0,8,9), new Tile(8,6,3,0)},
                                                  {new Tile(0,4,5,0), new Tile(9,6,5,4), new Tile(4,3,5,5), new Tile(4,3,3,9), new Tile(7,8,5,8), new Tile(1,4,0,0)},
                                                  {new Tile(3,0,9,6), new Tile(0,9,8,8), new Tile(7,6,4,6), new Tile(5,0,6,6), new Tile(0,6,6,9), new Tile(6,5,9,5)},
                                                  {new Tile(0,9,7,4), new Tile(5,0,6,9), new Tile(3,6,1,3), new Tile(5,0,0,1), new Tile(5,8,9,1), new Tile(0,4,2,4)}};

        // Get's & Set's Here...
        
        // Functions Here...

        /*
		 * Basic idea of the Branch & Bounce ALgorithm
		 * 
		 * - Pull an node out of the LNV (Lista de Nodos vivos)
		 * - Generate the childs (Branches) for the specified node.
		 * - If the node not bouncing, push into the LNV
		 * 
		 * 
		 * 
		 */

        private int matrixN(Tile[,] matrix)
        {
            return Convert.ToInt32(Math.Sqrt(matrix.Length));
        }

        private List<node> matrixToLNV(Tile[,] matrix)
        {
            int size = matrixN(matrix);
            List<node> listToReturn = new List<node>();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    Tile tile = matrix[i, j];
                    node tempNode = new node();
                    tempNode.tile = tile;
                    listToReturn.Add(tempNode);
                }
            return listToReturn;
        }

        private int[] matrixPositionsAboutATreeLevel(int actualTreeLevel, int n)
        {
            int[] ij = new int[2];
            int count = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (count == actualTreeLevel)
                    {
                        if (i % 2 == 0)
                        {
                            ij[0] = i;
                            ij[1] = j;
                        }
                        else
                        {
                            ij[0] = i;
                            ij[1] = n - (j + 1);
                        }
                        return ij;
                    }
                    count++;
                }
            }
            return ij;
        }

        private node[,] tempMatrixToSolve;

        private node nodeFit(node temp, int actualLevelTree, int n)
        {
            int[] tempValues = matrixPositionsAboutATreeLevel(actualLevelTree, n);
            int i = tempValues[0];
            int j = tempValues[1];

            if (i == 0 && j == 0)//First tile (Top Right tile)
            {
                temp.isFit = (tempMatrixToSolve[i, j].tile.right == temp.tile.left) ? true : false;
                return temp;
            }

            if (i == 0 && j > 0 && j < n - 1)//Top Tiles
            {
                temp.isFit = (tempMatrixToSolve[i, j].tile.right == temp.tile.left) ? true : false;
                return temp;
            }

            if (i == 0 && j == n - 1)//Top Left Tile
            {
                temp.isFit = (tempMatrixToSolve[i, j].tile.bottom == temp.tile.top) ? true : false;
                return temp;
            }

            if (i != 0 && i < n - 1 && j == n - 1)//Right border
            {
                temp.isFit = (tempMatrixToSolve[i, j].tile.left == temp.tile.right && tempMatrixToSolve[i - 1, j - 1].tile.bottom == temp.tile.top) ? true : false;
                return temp;
            }

            if (i != 0 && j == 0 && i < n - 1)//Left Border Tiles
            {
                temp.isFit = (tempMatrixToSolve[i, j].tile.bottom == temp.tile.top) ? true : false;
                return temp;
            }

            if (i == n - 1 && j == 0)//Bottom Left Tile
            {
                temp.isFit = (tempMatrixToSolve[i, j].tile.right == temp.tile.left && tempMatrixToSolve[i - 1, j + 1].tile.bottom == temp.tile.top) ? true : false;
                return temp;
            }
            if (i == n - 1 && j > 0 && j < n - 1)//bottom tiles
            {
                temp.isFit = (tempMatrixToSolve[i, j].tile.right == temp.tile.left) ? true : false;
                return temp;
            }
           
            if (i != 0 && j != 0) // Center Tiles
            {
                if (j % 2 == 0)
                    temp.isFit = (tempMatrixToSolve[i, j].tile.right == temp.tile.left) ? true : false;
                else
                    temp.isFit = (tempMatrixToSolve[i, j].tile.left == temp.tile.right) ? true : false;
                return temp;
            }

            if (i == n - 1 && j == n - 1)//bottom right Tile
            {
                // Here maybe the algortihm doesnt need to do a shit!
            }

            return temp;
        }

        public void initBacktraking(Tile[,] matrixGame)
        {
            List<node> LNV = matrixToLNV(matrixGame);
            int nGame = matrixN(matrixGame);

            tempMatrixToSolve = new node[nGame, nGame];

            foreach (node tempN in LNV)
            {
                Console.WriteLine("Hola ");
                tempMatrixToSolve[0, 0] = tempN;

                List<node> LNVWithOutTemp = LNV.ToList();
                LNVWithOutTemp.Remove(tempN);
                tempN.possibleMoves = LNVWithOutTemp;

                bool breakingCicle = backtraking(tempN, 0, nGame);
                if (breakingCicle)
                {
                    Console.WriteLine("Solution Found!!!");
                    break;
                }
            }
        }

        private bool backtraking(node objectiveNode, int actualLevelTree, int n)
        {
            if (actualLevelTree + 1 == (n * n))
                return true;

            foreach (node tempN in objectiveNode.possibleMoves)
            {
                node compIsFit = nodeFit(tempN, actualLevelTree, n);

                if (compIsFit.isFit)
                {
                    int[] tempValues = matrixPositionsAboutATreeLevel(actualLevelTree + 1, n);
                    int i = tempValues[0];
                    int j = tempValues[1];

                    tempMatrixToSolve[i, j] = tempN;

                    List<node> LNVWithOutTempN = objectiveNode.possibleMoves.ToList();
                    LNVWithOutTempN.Remove(tempN);
                    tempN.possibleMoves = LNVWithOutTempN;

                    bool breaking = backtraking(tempN, actualLevelTree + 1, n);
                    if (breaking)
                        return true;
                }
            }
            return false;
        }

        public void printTempMatrixToSolve()
        {
            foreach (node a in tempMatrixToSolve)
                Console.WriteLine("\\" + a.tile.top + "/\n" + a.tile.left + "x" + a.tile.right + "\n/" + a.tile.bottom + "\\");
        }

        //
        // Type:
        //      0 = Corner
        //      1 = Edge
        //      2 = Middle
        //
        public int sortOutPiece(int position, int N)
        {
            int type;

            return type = (position == 0 || position == N - 1 || position == N * (N - 1) || position == (N * N) - 1) ? 0 : (
                (0 < position && position < N - 1) || (position == N * (position / N)) || (position == (N * ((position / N) + 1)) - 1) ||
                (N * (N - 1) < position && position < (N * N) - 1)) ? 1 : 2;
        }

    }
}
