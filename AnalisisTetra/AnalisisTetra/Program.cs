using System;
using System.Collections.Generic;
using System.Collections;
using Facet.Combinatorics;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace AnalisisTetra
{
	public static class Ex
	{
		public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k)
		{
			return k == 0 ? new[] { new T[0] } :
			  elements.SelectMany((e, i) =>
				elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
		}
	}

	class MainClass
	{
        public static int c = 0;
        public static int a = 0;
        public static int lE = 0;

        public static Singleton gController = Singleton.Instance;

        public static void Main(string[] args)
		{
            menu();
            Console.ReadKey();
        }

        public static void menu()
        {
            Console.WriteLine("\t\t\tTETRAVEX");
            Console.WriteLine("\t\tCristiam Acuna & Andres Garcia");
            Console.WriteLine("\t\t\tMENU");
            Console.WriteLine("\t\t1- Brute Force ");
            Console.WriteLine("\t\t2- Discard");
            Console.WriteLine("\t\t3- Selection");
            Console.WriteLine("\n\nEnter an option: ");
            string entry = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("\t\t\tSize");
            Console.WriteLine("\t\tPDT: n = 3 and 6 are static games");
            Console.WriteLine("\t\t1- N = 2 ");
            Console.WriteLine("\t\t2- N = 3");
            Console.WriteLine("\t\t3- N = 4");
            Console.WriteLine("\t\t4- N = 5");
            Console.WriteLine("\t\t5- N = 6");
            Console.WriteLine("\n\nEnter an option: ");
            string size = Console.ReadLine();

            Tile[,] tiles = (size.Equals("1")) ? shuffle(generateTiles(2)) : (size.Equals("2")) ? gController.n3game : (size.Equals("3")) ? shuffle(generateTiles(4)) : (size.Equals("4")) ? shuffle(generateTiles(5)) : gController.n6game;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (entry.Equals("1"))
            {
                Console.Clear();
                bruteForce(shuffle(tiles));

            } else if (entry.Equals("2"))
            {
                Console.Clear();
                gController.initBacktraking(tiles);

                gController.printTempMatrixToSolve();
            } else if (entry.Equals("3"))
            {
                Console.Clear();
                Console.WriteLine("Nothing to do here! Get out!");
            } else
            {
                Console.WriteLine("Press an option available! -_- Bye!");
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }

        public static Tile[,] bruteForce(Tile[,] matrix)
        {
            int sizeMatrix = Convert.ToInt32(Math.Sqrt(matrix.Length)); a++; lE++;
            int sizeArray = matrix.Length; a++; lE++;

            //paso la matriz a array
            List<Tile> tileList = new List<Tile>(); a++; lE++;

            for (int i = 0; i < sizeMatrix; i++)
            {
                a += 2; c++; lE++;
                for (int e = 0; e < sizeMatrix; e++)
                {
                    a += 2; c++; lE++;
                    tileList.Add(matrix[i, e]); a++; lE++;
                }
            }
            //Una vez generado el array de tiles puedo referenciarlos con indices
            //genero matriz de indices
            int[] indexArray = new int[sizeArray]; a++; lE++;
            //Lleno la matriz con indices referenciando los tiles
            for (int i = 0; i < sizeArray; i++)
            {
                a += 2; c++; lE++;

                indexArray[i] = i; a++; lE++;
            }
            Permutations<int> posibleCombinations = new Permutations<int>(indexArray); a++; lE++;
            //recorro el arreglo de combinaciones
            foreach (List<int> solutionIndexes in posibleCombinations)
            {
                a += 2; c++; lE++;

                List<Tile> tilesInList = new List<Tile>(); a++; lE++;
                //recorro los indices combinados
                foreach (int combinations in solutionIndexes)
                {
                    a += 2; c++; lE++;

                    Tile tile = tileList.ElementAt(combinations); a++; lE++;
                    tilesInList.Add(tile); a++; lE++;
                }
                //necesito pasar la lista a matriz para aplicar el metodo isSolution.

                Tile[,] matrixForSolution = new Tile[sizeMatrix, sizeMatrix]; a++; lE++;

                int rows = Convert.ToInt32(Math.Sqrt(tilesInList.Count)); a++; lE++;
                int columns = Convert.ToInt32(Math.Sqrt(tilesInList.Count)); a++; lE++;
                int i = 0; a++; lE++;
                while (i < tileList.Count)
                {
                    c++; lE++;
                    for (int x = 0; x < rows; x++)
                    {
                        a += 2; c++; lE++;
                        for (int e = 0; e < columns; e++)
                        {
                            a += 2; c++; lE++;
                            matrixForSolution[x, e] = tilesInList[i]; a++; lE++;
                            Console.WriteLine("     \\" + tilesInList[i].top + "/      ");
                            Console.WriteLine("    |" + tilesInList[i].left + "|" + tilesInList[i].right + "|       ");
                            Console.WriteLine("     /" + tilesInList[i].bottom + "\\");
                            Console.WriteLine(" ");
                            i++; a++; lE++;
                        }

                    }

                }

                if (isSolution(matrixForSolution))
                {
                    c++; lE++;
                    Console.WriteLine("Se resolvió!!!");
                    lE++; return matrixForSolution;
                }
            }
            Console.WriteLine(posibleCombinations.Count);
            lE++; return matrix;
        }

        public static bool isSolution(Tile[,] matrix)
        {
            int size = Convert.ToInt32(Math.Sqrt(matrix.Length)); a++; lE++;
            for (int i = 0; i < size; i++)
            {
                c++; a += 2; lE++;
                for (int e = 0; e < size; e++)
                {
                    c++; a += 2; lE++;
                    Tile tile = matrix[i, e]; a++; lE++;
                    if (i == 0 && e == 0)//First tile (Top Right tile)
                    {
                        c += 2; lE++;

                        if (tile.right == matrix[i, e + 1].left && tile.bottom == matrix[i + 1, e].top)
                        {
                            c += 2; lE++;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (i == 0 && e > 0 && e < size - 1)//Top Tiles
                    {
                        c += 3; lE++;
                        if (tile.right == matrix[i, e + 1].left && tile.bottom == matrix[i + 1, e].top && tile.left == matrix[i, e - 1].right)
                        {
                            c += 3; lE++;

                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (i == 0 && e == size - 1)//Top Left Tile
                    {
                        c += 2; lE++;
                        if (tile.left == matrix[i, e - 1].right && tile.bottom == matrix[i + 1, e].top)
                        {
                            c += 2; lE++;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (i != 0 && e == 0 && i < size - 1)//Left Border Tiles
                    {
                        c += 3; lE++;
                        if (tile.top == matrix[i - 1, e].bottom && tile.right == matrix[i, e + 1].left && tile.bottom == matrix[i + 1, e].top)
                        {
                            c += 3; lE++;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (i != 0 && i < size - 1 && e == size - 1)//Right border
                    {
                        c += 3; lE++;
                        if (tile.top == matrix[i - 1, e].bottom && tile.bottom == matrix[i + 1, e].top && tile.left == matrix[i, e - 1].right)
                        {
                            c += 3; lE++;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (i == size - 1 && e == 0)//Bottom Left Tile
                    {
                        c += 2; lE++;
                        if (tile.top == matrix[i - 1, e].bottom && tile.right == matrix[i, e + 1].left)
                        {
                            c += 2; lE++;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (i == size - 1 && e > 0 && e < size - 1)//bottom tiles
                    {
                        c += 3; lE++;
                        if (tile.top == matrix[i - 1, e].bottom && tile.right == matrix[i, e + 1].left && tile.left == matrix[i, e - 1].right)
                        {
                            c += 3; lE++;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (i == size - 1 && e == size - 1)//bottom right Tile
                    {
                        c += 2; lE++;
                        if (tile.top == matrix[i - 1, e].bottom && tile.left == matrix[i, e - 1].right)
                        {
                            c += 2; lE++;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (i != 0 && e != 0)
                    {
                        c += 2; lE++;
                        if (tile.top == matrix[i - 1, e].bottom && tile.right == matrix[i, e + 1].left && tile.bottom == matrix[i + 1, e].top && tile.left == matrix[i, e - 1].right)
                        {
                            c += 4; lE++;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            lE++; return true;
        }

        public static Tile[,] shuffle(Tile[,] originalMatrix)
        {
            int size = Convert.ToInt32(Math.Sqrt(originalMatrix.Length)); a++; lE++;
            //Tile[] solutionArray = new Tile[originalMatrix.Length];
            List<Tile> solutionList = new List<Tile>(); a++; lE++;
            for (int i = 0; i < size; i++)
            {
                c++; a += 2; lE++;
                for (int e = 0; e < size; e++)
                {
                    c++; a += 2; lE++;
                    Tile tile = originalMatrix[i, e]; a++; lE++;
                    solutionList.Add(tile); a++; lE++;
                }
            }
            List<Tile> shufflledList = new List<Tile>(); a++; lE++;

            Random r = new Random(); a++; lE++;
            int randomIndex = 0; a++; lE++;
            while (solutionList.Count > 0)
            {
                c++; lE++;
                randomIndex = r.Next(0, solutionList.Count); a++; lE++;//Choose a random object in the list
                shufflledList.Add(solutionList[randomIndex]); a++; lE++;//add it to the new random list
                solutionList.RemoveAt(randomIndex); lE++;//remove to avoid duplicates
            }

            for (int i = 0; i < shufflledList.Count; i++)
            {
                c++; a += 2; lE++;
                Tile tile = shufflledList[i]; a++; lE++;
                Console.WriteLine(tile.top + " " + tile.right + " " + tile.bottom + " " + tile.left);

            }
            //paso el arreglo a matriz
            int rows = Convert.ToInt32(Math.Sqrt(shufflledList.Count)); a++; lE++;
            int columns = Convert.ToInt32(Math.Sqrt(shufflledList.Count)); a++; lE++;
            Tile[,] matrix = new Tile[rows, columns]; a++; lE++;
            int t = 0; a++; lE++;

            while (t < shufflledList.Count)
            {
                c++; lE++;
                for (int x = 0; x < rows; x++)
                {
                    c++; a += 2; lE++;
                    for (int e = 0; e < columns; e++)
                    {
                        c++; a += 2; lE++;
                        matrix[x, e] = shufflledList[t]; a++; lE++;
                        //Console.Write(shufflledList[t].top + " " + shufflledList[t].right + " " + shufflledList[t].bottom + " " + shufflledList[t].left);
                        t++; a++; lE++;
                    }
                    Console.WriteLine("  ");
                }

            }
            lE++; return matrix;
        }

        public static Tile[,] generateTiles(int size)
        {
            Tile[,] matrix = new Tile[size, size]; a++; lE++;
            Random rnd = new Random(); a++; lE++;
            for (int i = 0; i < size; i++)//Rows
            {
                c++; a += 2; lE++;
                for (int e = 0; e < size; e++)//Columns
                {
                    c++; a += 2; lE++;
                    if (i == 0 && e == 0)
                    { // First Tile
                        c += 2; lE++;
                        int top = rnd.Next(1, 10); a++; lE++;
                        int right = rnd.Next(1, 10); a++; lE++;
                        int bottom = rnd.Next(1, 10); a++; lE++;
                        int left = rnd.Next(1, 10); a++; lE++;
                        Tile tile = new Tile(top, right, bottom, left); a++; lE++;
                        matrix[e, i] = tile; a++; lE++;
                        Console.Write(tile.top + " " + tile.right + " " + tile.bottom + " " + tile.left);

                    }
                    else
                    {

                        int top;

                        int left;
                        Tile tile = new Tile(); a++; lE++;
                        if (i - 1 >= 0 && matrix[i - 1, e] != null)
                        {
                            c += 2; lE++;
                            top = matrix[i - 1, e].bottom; a++; lE++;

                        }
                        else
                        {
                            top = 0; a++; lE++;
                        }

                        if (e - 1 >= 0 && matrix[i, e - 1] != null)
                        {
                            c++; lE++;
                            left = matrix[i, e - 1].right; a++; lE++;
                        }
                        else
                        {
                            left = 0; a++; lE++;
                        }
                        //Console.WriteLine(top + " " + 0 + " " + 0 + " " + left);

                        tile = findTile(top, 0, 0, left); a++; lE++;

                        matrix[i, e] = tile; a++; lE++;
                        Console.Write(tile.top + " " + tile.right + " " + tile.bottom + " " + tile.left);
                    }
                    Console.WriteLine(" ");
                }
            }
            lE++; return matrix;
        }

        public static Tile findTile(int top, int right, int bottom, int left)
        {
            int tTop = 0; a++; lE++;
            int tRight = 0; a++; lE++;
            int tBottom = 0; a++; lE++;
            int tLeft = 0; a++; lE++;
            Random rand = new Random(); a++; lE++;

            Tile tile = new Tile(); a++; lE++;
            //# top to right (first line)
            if (top == 0 && right == 0 && bottom == 0 && left != 0)
            {
                c += 4; lE++;
                tTop = rand.Next(1, 10); a++; lE++;
                tRight = rand.Next(1, 10); a++; lE++;
                tBottom = rand.Next(1, 10); a++; lE++;
                tLeft = left; a++; lE++;

                tile = new Tile(tTop, tRight, tBottom, tLeft); a++; lE++;

            }
            //Under top to right (middle lines)
            if (top != 0 && right == 0 && bottom == 0 && left != 0)
            {
                c += 4; lE++;

                tTop = top; a++; lE++;
                tRight = rand.Next(1, 10); a++; lE++;
                tBottom = rand.Next(1, 10); a++; lE++;
                tLeft = left; a++; lE++;


                tile = new Tile(tTop, tRight, tBottom, tLeft); a++; lE++;
                // Console.WriteLine(tile.top + " " + tile.right + " " + tile.bottom + " " + tile.left);

                //  Console.WriteLine("Abajo");
            }
            //Bottom Left corner
            if (bottom == 0 && left == 0 && top != 0 && right == 0)
            {
                c += 4; lE++;
                tTop = top; a++; lE++;
                tRight = rand.Next(1, 10); a++; lE++;
                tBottom = rand.Next(1, 10); a++; lE++;
                tLeft = rand.Next(1, 10); a++; lE++;
                tile = new Tile(tTop, tRight, tBottom, tLeft); a++; lE++;
            }

            //Tile's sides are assigned
            lE++; return tile;
        }

    }
}
