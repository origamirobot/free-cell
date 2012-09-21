using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCell.Core
{

	/// <summary>
	/// 
	/// </summary>
	public class Cascade : List<Card>, ITarget, ICloneable
	{

		#region PUBLIC ACCESSORS

		


		#endregion PUBLIC ACCESSORS 

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="Cascade"/> class.
		/// </summary>
		public Cascade()
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
		public Cascade Clone()
		{
			Cascade c = new Cascade();
			foreach (Card card in this)
				c.Add(card.Clone());
			return c;
		}

	}

}
