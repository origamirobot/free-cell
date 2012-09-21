using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCell.Core
{

	/// <summary>
	/// 
	/// </summary>
	public interface IRules
	{

		#region PUBLIC ACCESSORS


		/// <summary>
		/// Gets the number of cells used to temporarily store cards.
		/// </summary>
		int NumberOfCells { get; }

		/// <summary>
		/// Gets the number of cascades to deal cards to.
		/// </summary>
		int NumberOfCascades { get; }

		/// <summary>
		/// Gets or sets a value indicating if the rule engine should enforce the rules.
		/// </summary>
		/// <remarks>This property is primarily used durring the shuffle to allow cards to be placed randomly.</remarks>
		bool EnforceRules { get; set; }

		/// <summary>
		/// Gets a value indicating if the game should automatically send cards to the foundations as they're uncovered.
		/// </summary>
		bool BankCardsAutomatically { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow moves from the foundation back into the play area.
		/// </summary>
		/// <value><c>true</c> if allow moves from foundation; otherwise, <c>false</c>.</value>
		bool AllowMoveFromFoundation { get; set; }


		#endregion PUBLIC ACCESSORS 

		#region PUBLIC METHODS


		/// <summary>
		/// Checks to see if it is possible to move the specified <see cref="Card"/> to the specified <see cref="Foundation"/>.
		/// </summary>
		/// <param name="card">The <see cref="Card"/> to check if its possible to move to the specified <see cref="Foundation"/></param>
		/// <param name="foundation">The <see cref="Foundation"/> to check if its possible to move to the specified <see cref="Card"/> to</param>
		/// <returns><c>true</c> if the specified <see cref="Card"/> can be moved to the specified <see cref="Foundation"/>; otherwise <c>false</c></returns>
		bool CanMoveToFoundation(Card card, Foundation foundation);

		/// <summary>
		/// Checks to see if it is possible to move the specified <see cref="Card"/> to the specified <see cref="Cell"/>.
		/// </summary>
		/// <param name="card">The <see cref="Card"/> to check if its possible to move to the specified <see cref="Cell"/></param>
		/// <param name="cell">The destination <see cref="Cell"/> to hold the specified <see cref="Card"/></param>
		/// <returns><c>true</c> if the specified <see cref="Card"/> can be moved to the specified <see cref="Cell"/>; otherwise <c>false</c></returns>
		bool CanMoveToCell(Card card, Cell cell);

		/// <summary>
		/// Checks to see if it is possible to move the specified <see cref="Card"/> to the specified <see cref="Cascade"/>.
		/// </summary>
		/// <param name="card">The <see cref="Card"/> to check if its possible to move to the specified <see cref="Cascade"/></param>
		/// <param name="cascade">The <see cref="Cascade"/> that will be holding the specified <see cref="Card"/></param>
		/// <returns><c>true</c> if the specified <see cref="Card"/> can be moved to the specified <see cref="Cascade"/>; otherwise <c>false</c></returns>
		bool CanMoveToCascade(Card card, Cascade cascade);

		/// <summary>
		/// Checks to see if it is possible to move the specified <see cref="Card"/> to the specified <see cref="Cascade"/>.
		/// </summary>
		/// <param name="tableaux">The tableaux to try to move to the cascade.</param>
		/// <param name="cascade">The <see cref="Cascade"/> that will be holding the specified <see cref="Card"/></param>
		/// <param name="game">The game is needed to determine the number of possible moves available.</param>
		/// <returns><c>true</c> if the specified <see cref="Card"/> can be moved to the specified <see cref="Cascade"/>; otherwise <c>false</c></returns>
		bool CanMoveToCascade(List<Card> tableaux, Cascade cascade, Game game);

		/// <summary>
		/// Gets a value indicating if the top card can sit on top of the bottom card while moving to a cascade.
		/// </summary>
		/// <param name="topCard">The top card (moving card)</param>
		/// <param name="bottomCard">The bottom card (destination card).</param>
		/// <returns><c>true</c> if these two cards can link in the cascade; otherwise <c>false</c>.</returns>
		bool DoCardsLinkInCascade(Card topCard, Card bottomCard);

		/// <summary>
		/// Gets a value indicating if the top card can sit on top of the bottom card in the foundation.
		/// </summary>
		/// <param name="topCard">The top card (moving card)</param>
		/// <param name="bottomCard">The bottom card (destination card).</param>
		/// <returns><c>true</c> if these two cards can link in the foundation; otherwise <c>false</c>.</returns>
		bool DoCardsLinkInFoundation(Card topCard, Card bottomCard);


		#endregion PUBLIC METHODS 

	}


}
