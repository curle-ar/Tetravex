using System;
using System.Collections;
using System.Linq;

namespace AnalisisTetra
{
	public class Tile
	{
		public int top;
		public int right;
		public int bottom;
		public int left;

		public Tile(int top, int right, int bottom, int left)
		{
			this.top = top;
			this.right = right;
			this.bottom = bottom;
			this.left = left;
		}

		public Tile()
		{
		}
	}
}
