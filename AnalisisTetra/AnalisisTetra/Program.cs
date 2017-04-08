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
		public static void Main(string[] args)
		{
            Singleton gController = Singleton.Instance;

            Console.WriteLine("\t\t\tTETRAVEX");
            Console.WriteLine("\t\tCristiam Acuna & Andres Garcia");
            Console.WriteLine("\t\t\tMENU");
            Console.WriteLine("\t\t1- Brute Force ");
            Console.WriteLine("\t\t2- Discard");
            Console.WriteLine("\t\t3- Selection");
            Console.WriteLine("SOLUTION\n");

            Tile[,] tiles = shuffle(generateTiles(5));

            Console.WriteLine("\t\t\tTETRAVEX");
			Console.WriteLine("\t\tCristiam Acuna & Andres Garcia");
			Console.WriteLine("Random Shuffled Solution");

			bruteForce(shuffle(tiles));

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            gController.initBacktraking(tiles);

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            gController.printTempMatrixToSolve();

            Console.ReadKey();
        }

		public static Tile[,] bruteForce(Tile[,] matrix)
		{
			int sizeMatrix = Convert.ToInt32(Math.Sqrt(matrix.Length));
			int sizeArray = matrix.Length;
			//Console.WriteLine(sizeMatrix + " " + sizeArray);
			//paso la matriz a array
			List<Tile> tileList = new List<Tile>();

			for (int i = 0; i < sizeMatrix; i++)
			{
				for (int e = 0; e < sizeMatrix; e++)
				{
					tileList.Add(matrix[i, e]);
				}
			}
			//Una vez generado el array de tiles puedo referenciarlos con indices
			//genero matriz de indices
			int[] indexArray = new int[sizeArray];
			//Lleno la matriz con indices referenciando los tiles
			for (int i = 0; i < sizeArray; i++)
			{
				indexArray[i] = i;
			}
			Permutations<int> posibleCombinations = new Permutations<int>(indexArray);
			//recorro el arreglo de combinaciones
			foreach (List<int> solutionIndexes in posibleCombinations)
			{
				
				List<Tile> tilesInList = new List<Tile>();
				//recorro los indices combinados
				foreach (int combinations in solutionIndexes)
				{
					
					Tile tile = tileList.ElementAt(combinations);
					tilesInList.Add(tile);
				}
				//necesito pasar la lista a matriz para aplicar el metodo isSolution.
				//Console.WriteLine(tileList.Count);

				Tile[,] matrixForSolution = new Tile[sizeMatrix, sizeMatrix];


				int rows = Convert.ToInt32(Math.Sqrt(tilesInList.Count));
				int columns = Convert.ToInt32(Math.Sqrt(tilesInList.Count));
				int i = 0;
				while (i < tileList.Count)
				{
					for (int x = 0; x < rows; x++)
					{
						for (int e = 0; e < columns; e++)
						{
							matrixForSolution[x, e] = tilesInList[i];
							/*Console.WriteLine("     |" +tilesInList[i].top + "|      " );
							Console.WriteLine("    |" + tilesInList[i].left + "|" + tilesInList[i].right + "|       ");
							Console.WriteLine("     |" + tilesInList[i].bottom + "|");
							Console.WriteLine(" ");*/
							i++;
						}

					}

				}

				if (isSolution(matrixForSolution))
				{
					Console.WriteLine("Se resolvió!!!");
					return matrixForSolution;
				}
				else
				{
					//Console.WriteLine("No se re solvió");
				}

		
				/*Console.WriteLine("  ");
				Console.WriteLine(" ------------- ");
				Console.WriteLine("  ");*/
			}
			Console.WriteLine(posibleCombinations.Count);
			return matrix;
		}
	
		public static bool isSolution(Tile[,] matrix)
		{
			int size = Convert.ToInt32(Math.Sqrt(matrix.Length));
			for (int i = 0; i < size; i++)
			{
				for (int e = 0; e < size; e++)
				{
					Tile tile = matrix[i, e];
					if (i == 0 && e == 0)//First tile (Top Right tile)
					{
						
						if (tile.right == matrix[i, e + 1].left && tile.bottom == matrix[i + 1, e].top)
						{
							continue;	
						}
						else
						{
							return false;
						}
					}
					if (i==0 && e >0 && e < size-1)//Top Tiles
					{
						if (tile.right == matrix[i, e + 1].left && tile.bottom == matrix[i + 1, e].top && tile.left == matrix[i, e - 1].right)
						{
							
							continue;	
						}
						else
						{
							return false;
						}
					}
					if (i == 0 && e == size - 1)//Top Left Tile
					{
						if (tile.left == matrix[i, e - 1].right && tile.bottom == matrix[i + 1, e].top)
						{
							continue;
						}
						else
						{
							return false;
						}
					}

					if (i != 0 && e == 0 && i < size-1)//Left Border Tiles
					{
						if (tile.top == matrix[i - 1, e].bottom && tile.right == matrix[i, e + 1].left && tile.bottom == matrix[i + 1, e].top)
						{
							continue;
						}
						else
						{
							return false;
						}
					}
					if (i != 0 && i < size-1 && e == size-1)//Right border
					{
						if (tile.top == matrix[i-1,e].bottom && tile.bottom == matrix[i+1,e].top && tile.left == matrix[i,e-1].right)
						{
							continue;
						}
						else
						{
							return false;
						}
					}
					if (i == size-1 && e == 0)//Bottom Left Tile
					{
						if (tile.top == matrix[i - 1, e].bottom && tile.right == matrix[i, e + 1].left)
						{
							continue;
						}
						else
						{
							return false;
						}
					}
					if (i== size-1 && e > 0 && e < size-1)//bottom tiles
					{
						if (tile.top == matrix[i - 1, e].bottom && tile.right == matrix[i, e + 1].left && tile.left == matrix[i, e - 1].right)
						{
							continue;
						}
						else
						{
							return false;
						}
					}
					if (i == size-1 && e == size-1)//bottom right Tile
					{
						if (tile.top == matrix[i - 1, e].bottom && tile.left == matrix[i, e - 1].right)
						{
							continue;
						}
						else
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		public static Tile[,] shuffle(Tile[,] originalMatrix)
		{
			int size = Convert.ToInt32(Math.Sqrt(originalMatrix.Length));
			//Tile[] solutionArray = new Tile[originalMatrix.Length];
			List<Tile> solutionList = new List<Tile>();
			for (int i = 0; i < size; i++)
			{
				for (int e = 0; e < size; e++)
				{
					Tile tile = originalMatrix[i, e];
					solutionList.Add(tile);
				}
			}
			List<Tile> shufflledList = new List<Tile>();

			Random r = new Random();
			int randomIndex = 0;
			while (solutionList.Count > 0)
			{
				randomIndex = r.Next(0, solutionList.Count); //Choose a random object in the list
				shufflledList.Add(solutionList[randomIndex]); //add it to the new, random list
				solutionList.RemoveAt(randomIndex); //remove to avoid duplicates
			}

			for (int i = 0; i < shufflledList.Count; i++)
			{
				Tile tile = shufflledList[i];
				Console.WriteLine(tile.top + " " + tile.right + " " + tile.bottom + " " + tile.left);

			}
			//paso el arreglo a matriz
			int rows = Convert.ToInt32(Math.Sqrt(shufflledList.Count));
			int columns = Convert.ToInt32(Math.Sqrt(shufflledList.Count));
			Tile[,] matrix = new Tile[rows, columns];
			int t = 0;

			while (t < shufflledList.Count)
			{
				for (int x = 0; x < rows; x++)
				{
					for (int e = 0; e < columns; e++)
					{
						matrix[x, e] = shufflledList[t];
						//Console.Write(shufflledList[t].top + " " + shufflledList[t].right + " " + shufflledList[t].bottom + " " + shufflledList[t].left);
						t++;
					}
					Console.WriteLine("  ");
				}

			}
			return matrix;
		}

		public static Tile[,] generateTiles(int size)
		{
			Tile[,] matrix = new Tile[size, size];
			Random rnd = new Random();
			for (int i = 0; i < size; i++)//Rows
			{
				for (int e = 0; e < size; e++)//Columns
				{
					if (i == 0 && e == 0)
					{ // First Tile

						int top = rnd.Next(1, 10);
						int right = rnd.Next(1, 10);
						int bottom = rnd.Next(1, 10);
						int left = rnd.Next(1, 10);
						Tile tile = new Tile(top, right, bottom, left);
						matrix[e, i] = tile;
						Console.Write(tile.top + " " + tile.right + " " + tile.bottom + " " + tile.left);

					}
					else
					{
						int top;

						int left;
						Tile tile = new Tile();
						if (i - 1 >= 0 && matrix[i - 1, e] != null)
						{
							top = matrix[i - 1, e].bottom;

						}
						else
						{
							top = 0;
						}

						if (e - 1 >= 0 && matrix[i, e - 1] != null)
						{
							left = matrix[i, e - 1].right;
						}
						else
						{
							left = 0;
						}
						//Console.WriteLine(top + " " + 0 + " " + 0 + " " + left);

						tile = findTile(top, 0, 0, left);

						matrix[i, e] = tile;
						Console.Write(tile.top + " " + tile.right + " " + tile.bottom + " " + tile.left);

					}
					Console.WriteLine(" ");
				}
			}
			return matrix;
		}

		public static Tile findTile(int top, int right, int bottom, int left)
		{
			int tTop = 0;
			int tRight = 0;
			int tBottom = 0;
			int tLeft = 0;
			Random rand = new Random();

			Tile tile = new Tile();

			//# top to right (first line)
			if (top == 0 && right == 0 && bottom == 0 && left != 0)
			{
				tTop = rand.Next(1, 10);
				tRight = rand.Next(1, 10);
				tBottom = rand.Next(1, 10);
				tLeft = left;

				tile = new Tile(tTop, tRight, tBottom, tLeft);

			}
			//Under top to right (middle lines)
			if (top != 0 && right == 0 && bottom == 0 && left != 0)
			{
				tTop = top;
				tRight = rand.Next(1, 10);
				tBottom = rand.Next(1, 10);
				tLeft = left;

				tile = new Tile(tTop, tRight, tBottom, tLeft);
				// Console.WriteLine(tile.top + " " + tile.right + " " + tile.bottom + " " + tile.left);
				//  Console.WriteLine("Abajo");
			}
			//Bottom Left corner
			if (bottom == 0 && left == 0 && top != 0 && right == 0)
			{
				tTop = top;
				tRight = rand.Next(1, 10);
				tBottom = rand.Next(1, 10);
				tLeft = rand.Next(1, 10);

				tile = new Tile(tTop, tRight, tBottom, tLeft);
			}

			//Tile's sides are assigned
			return tile;
		}
        
	}
}
