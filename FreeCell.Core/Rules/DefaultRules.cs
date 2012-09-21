using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCell.Core
{

	/// <summary>
	/// 
	/// </summary>
	public class DefaultRules : IRules
	{

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultRules"/> class.
		/// </summary>
		public DefaultRules()
		{
			EnforceRules = true;
		}



		#endregion CONSTRUCTORS 

		#region PUBLIC ACCESSORS


		/// <summary>
		/// Gets the number of cells used to temporarily store cards.
		/// </summary>
		public virtual int NumberOfCells
		{
			get { return 4; }
		}

		/// <summary>
		/// Gets the number of cascades to deal cards to.
		/// </summary>
		public virtual int NumberOfCascades
		{
			get { return 8; }
		}

		/// <summary>
		/// Gets a value indicating if the game should automatically send cards to the bank as they're uncovered.
		/// </summary>
		public virtual bool BankCardsAutomatically
		{
			get { return true; }
			set { }
		}

		/// <summary>
		/// Gets or sets a value indicating if the rule engine should enforce the rules.
		/// </summary>
		/// <remarks>This property is primarily used durring the shuffle to allow cards to be placed randomly.</remarks>
		public bool EnforceRules { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow moves from the foundation back into the play area.
		/// </summary>
		/// <value><c>true</c> if allow moves from foundation; otherwise, <c>false</c>.</value>
		public virtual bool AllowMoveFromFoundation
		{
			get { return true; }
			set { }
		}


		#endregion PUBLIC ACCESSORS

		#region PUBLIC METHODS


		/// <summary>
		/// Checks to see if it is possible to move the specified <see cref="Card"/> to the specified <see cref="Foundation"/>.
		/// </summary>
		/// <param name="card">The <see cref="Card"/> to check if its possible to move to the specified <see cref="Foundation"/></param>
		/// <param name="foundation">The <see cref="Foundation"/> to check if its possible to move to the specified <see cref="Card"/> to</param>
		/// <returns><c>true</c> if the specified <see cref="Card"/> can be moved to the specified <see cref="Foundation"/>; otherwise <c>false</c></returns>
		public virtual bool CanMoveToFoundation(Card card, Foundation foundation)
		{
			if (!EnforceRules)
				return true;
			if (foundation.Count == 0)
			{
				if (card.Rank == CardRank.Ace)
					return true;
			}
			else
			{
				if ((foundation.Last<Card>().Suit == card.Suit) && ((foundation.Last<Card>().Rank + 1) == card.Rank))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Checks to see if it is possible to move the specified <see cref="Card"/> to the specified <see cref="Cell"/>.
		/// </summary>
		/// <param name="card">The <see cref="Card"/> to check if its possible to move to the specified <see cref="Cell"/></param>
		/// <param name="cell">The destination <see cref="Cell"/> to hold the specified <see cref="Card"/></param>
		/// <returns><c>true</c> if the specified <see cref="Card"/> can be moved to the specified <see cref="Cell"/>; otherwise <c>false</c></returns>
		public virtual bool CanMoveToCell(Card card, Cell cell)
		{
			if (!EnforceRules)
				return true;

			if (!cell.HasCard)
				return true;
			return false;
		}

		/// <summary>
		/// Checks to see if it is possible to move the specified <see cref="Card"/> to the specified <see cref="Cascade"/>.
		/// </summary>
		/// <param name="card">The <see cref="Card"/> to check if its possible to move to the specified <see cref="Cascade"/></param>
		/// <param name="cascade">The <see cref="Cascade"/> that will be holding the specified <see cref="Card"/></param>
		/// <returns><c>true</c> if the specified <see cref="Card"/> can be moved to the specified <see cref="Stack"/>; otherwise <c>false</c></returns>
		public virtual bool CanMoveToCascade(Card card, Cascade cascade)
		{
			if (!EnforceRules)
				return true;

			if (cascade.Count == 0)
				return true;
			if (DoCardsLinkInCascade(card, cascade.Last()))
				return true;
			return false;
		}

		/// <summary>
		/// Checks to see if it is possible to move the specified <see cref="Card"/> to the specified <see cref="Cascade"/>.
		/// </summary>
		/// <param name="tableaux">The tableaux to try to move to the cascade.</param>
		/// <param name="cascade">The <see cref="Cascade"/> that will be holding the specified <see cref="Card"/></param>
		/// <returns><c>true</c> if the specified <see cref="Card"/> can be moved to the specified <see cref="Stack"/>; otherwise <c>false</c></returns>
		public virtual bool CanMoveToCascade(List<Card> tableaux, Cascade cascade, Game game)
		{
			if (!EnforceRules)
				return true;

			if (cascade.Count == 0)
				return true;

			Card landingCard = cascade.Last();

			if (tableaux.Count == 1)
			{
				// MOVING ONE CARD
				if (this.CanMoveToCascade(tableaux.Last(), cascade))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				// MOVING MULTIPLE CARDS... FIND THE MAXIMUM NUMBER OF MOVES AVAILABLE.
				int maxMoves = Utility.GetMaximumMovesForGame(game, (cascade.Count == 0));
				int level = 1;
				// WALK BACKWARDS OVER TABLEAUX TRYING EACH CARD AS A LINK TO LANDING CARD.
				for (int i = (tableaux.Count - 1); i >= 0; i--)
				{
					// CHECK FOR THE LINK BETWEEN CURRENT TABLEAUX CARD AND LANDING CARD.
					if (this.DoCardsLinkInCascade(tableaux[i], landingCard))
					{
						// CARDS LINK, CHECK TO SEE IF WE'VE EXCEEDED OUT MAX NUMBER OF MOVES ALLOWED.
						if (level > maxMoves)
						{
							// MAX NUMBER OF MOVES EXCEEDED, MOVE FAILS
							return false;
						}
						else
						{
							// MOVE SUCCESSFUL
							return true;
						}
					}
					else
					{
						// CARDS DO NOT LINK, CONTINUE ONTO THE NEXT TABLEAUX CARD.
						if (i == 0)
						{
							// THIS IS THE LAST CARD IN THE TABLEAUX, MOVE FAILS BECAUSE NO LINKING CARDS FOUND IN STACK.
							return false;
						}
					}
					level++;
				}
			}

			
			
			return false;
		}

		/// <summary>
		/// Gets a value indicating if the top card can sit on top of the bottom card while moving to a cascade.
		/// </summary>
		/// <param name="topCard">The top card (moving card)</param>
		/// <param name="bottomCard">The bottom card (destination card).</param>
		/// <returns><c>true</c> if these two cards can link in the cascade; otherwise <c>false</c>.</returns>
		public virtual bool DoCardsLinkInCascade(Card topCard, Card bottomCard)
		{
			if (!EnforceRules)
				return true;

			if (bottomCard == null)
				return true;

			if ((topCard.Color != bottomCard.Color) && ((topCard.Rank + 1) == bottomCard.Rank))
				return true;
			return false;
		}

		/// <summary>
		/// Gets a value indicating if the top card can sit on top of the bottom card in the foundation.
		/// </summary>
		/// <param name="topCard">The top card (moving card)</param>
		/// <param name="bottomCard">The bottom card (destination card).</param>
		/// <returns><c>true</c> if these two cards can link in the foundation; otherwise <c>false</c>.</returns>
		public virtual bool DoCardsLinkInFoundation(Card topCard, Card bottomCard)
		{
			if (!EnforceRules)
				return true;

			if ((topCard.Suit == bottomCard.Suit) && ((topCard.Rank - 1) == bottomCard.Rank))
				return true;
			return false;
		}


		#endregion PUBLIC METHODS

	}

}
