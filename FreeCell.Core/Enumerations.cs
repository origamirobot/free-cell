using System;

namespace FreeCell.Core
{

	/// <summary>
	/// Defines the values available for card suits.
	/// </summary>
	public enum CardSuit
	{
		/// <summary>
		/// Heart card suit.
		/// </summary>
		Heart = 0,
		/// <summary>
		/// Spade card suit.
		/// </summary>
		Spade = 1,
		/// <summary>
		/// Diamond card suit.
		/// </summary>
		Diamond = 2,
		/// <summary>
		/// Club card suit.
		/// </summary>
		Club = 3
	}

	/// <summary>
	/// Defines the values available for card colors.
	/// </summary>
	public enum CardColor
	{
		/// <summary>
		/// Black card color.
		/// </summary>
		Black,
		/// <summary>
		/// Red card color.
		/// </summary>
		Red
	}

	/// <summary>
	/// Defined the values available for card values.
	/// </summary>
	public enum CardRank
	{
		/// <summary>
		/// Ace card ranking
		/// </summary>
		Ace = 1,
		/// <summary>
		/// Two card ranking.
		/// </summary>
		Two = 2,
		/// <summary>
		/// Three card ranking.
		/// </summary>
		Three = 3,
		/// <summary>
		/// Four card ranking.
		/// </summary>
		Four = 4,
		/// <summary>
		/// Five card ranking.
		/// </summary>
		Five = 5,
		/// <summary>
		/// Six card ranking.
		/// </summary>
		Six = 6,
		/// <summary>
		/// Seven card ranking.
		/// </summary>
		Seven = 7,
		/// <summary>
		/// Eight card ranking.
		/// </summary>
		Eight = 8,
		/// <summary>
		/// Nine card ranking.
		/// </summary>
		Nine = 9,
		/// <summary>
		/// Ten card ranking.
		/// </summary>
		Ten = 10,
		/// <summary>
		/// Jack card ranking.
		/// </summary>
		Jack = 11,
		/// <summary>
		/// Queen card ranking.
		/// </summary>
		Queen = 12,
		/// <summary>
		/// King card ranking.
		/// </summary>
		King = 13
	}

	/// <summary>
	/// The result of a move made.
	/// </summary>
	public enum MoveResult
	{
		/// <summary>
		/// The move succeeded.
		/// </summary>
		Success,
		/// <summary>
		/// Tried to move to a cell that is already occupied.
		/// </summary>
		OccupiedCell,
		/// <summary>
		/// Not enough free spaces to move the cards.
		/// </summary>
		NotEnoughFreeSpace,
		/// <summary>
		/// Tried to move card to an unlinking destination.
		/// </summary>
		UnlinkingCards
	}

}
