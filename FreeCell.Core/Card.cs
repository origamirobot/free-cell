using System;

namespace FreeCell.Core
{

	/// <summary>
	/// Represents a single card in this game.
	/// </summary>
	public class Card : ICloneable
	{

		#region PRIVATE PROPERTIES

		private int _value;

		#endregion PRIVATE PROPERTIES

		#region PUBLIC ACCESSORS


		/// <summary>
		/// Gets the suit of this card.
		/// </summary>
		public CardSuit Suit { get; private set; }

		/// <summary>
		/// Gets the ranking of this card.
		/// </summary>
		public CardRank Rank { get; private set; }

		/// <summary>
		/// Gets the color of this card.
		/// </summary>
		public CardColor Color
		{
			get { return (Suit == CardSuit.Club || Suit == CardSuit.Spade) ? CardColor.Black : CardColor.Red; }
		}


		#endregion PUBLIC ACCESSORS

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="Card"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Card(int value)
		{
			if (value > 52 || value < 1)
				throw new ArgumentOutOfRangeException();
			Suit = (CardSuit)((int)Math.Floor((double)(value - 1) / (double)13));
			Rank = (CardRank)(value - ((int)Suit * 13));
			_value = value;
		}


		#endregion CONSTRUCTORS

		#region PUBLIC METHODS


		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format("{0} of {1}s", Rank.ToString(), Suit.ToString());
		}

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
		public Card Clone()
		{
			return new Card(_value);
		}


		#endregion PUBLIC METHODS

	}

}
