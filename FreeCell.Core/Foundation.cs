using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCell.Core
{

	/// <summary>
	/// 
	/// </summary>
	public class Foundation : List<Card>, ITarget, ICloneable
	{

		#region CONSTRUCTORS

		/// <summary>
		/// Initializes a new instance of the <see cref="Foundation"/> class.
		/// </summary>
		public Foundation()
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
		public Foundation Clone()
		{
			Foundation f = new Foundation();
			foreach (Card card in this)
				f.Add(card.Clone());
			return f;
		}


	}

}
