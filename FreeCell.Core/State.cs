using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCell.Core
{

	/// <summary>
	/// 
	/// </summary>
	public class State
	{
	
		#region PUBLIC ACCESSORS


		/// <summary>
		/// Gets the time stamp.
		/// </summary>
		public DateTime TimeStamp { get; private set; }

		/// <summary>
		/// Gets the cells.
		/// </summary>
		public List<Cell> Cells { get; private set; }

		/// <summary>
		/// Gets the cascades.
		/// </summary>
		public List<Cascade> Cascades { get; private set; }

		/// <summary>
		/// Gets the foundations.
		/// </summary>
		public List<Foundation> Foundations { get; private set; }


		#endregion PUBLIC ACCESSORS 

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="State"/> class.
		/// </summary>
		/// <param name="game">The game.</param>
		public State(Game game)
		{
			Cascades = new List<Cascade>();
			foreach (Cascade c in game.Cascades)
				Cascades.Add(c.Clone());

			Foundations = new List<Foundation>();
			foreach (Foundation f in game.Foundations)
				Foundations.Add(f.Clone());

			Cells = new List<Cell>();
			foreach (Cell c in game.Cells)
				Cells.Add(c.Clone());

		}


		#endregion CONSTRUCTORS 

	}

}
