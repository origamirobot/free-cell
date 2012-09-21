using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCell.Core
{

	/// <summary>
	/// 
	/// </summary>
	public class Cell : ITarget, ICloneable
	{

		#region PUBLIC ACCESSORS


		/// <summary>
		/// Gets or sets the card.
		/// </summary>
		public Card Card { get; set; }

		/// <summary>
		/// Gets a value indicating whether this cell has a card.
		/// </summary>
		/// <value><c>true</c> if this cell has a card; otherwise, <c>false</c>.</value>
		public bool HasCard { get { return (Card != null); } }


		#endregion PUBLIC ACCESSORS 

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="Cell"/> class.
		/// </summary>
		public Cell()
		{

		}


		#endregion CONSTRUCTORS 


		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns></returns>
		public Cell Clone()
		{
			Cell c = new Cell();
			if (this.HasCard)
				c.Card = this.Card.Clone();
			return c;
		}


	}

}
