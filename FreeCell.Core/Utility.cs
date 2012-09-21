using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCell.Core
{

	/// <summary>
	/// 
	/// </summary>
	public static class Utility
	{

		/// <summary>
		/// Gets the number of possible moves that can be made in the specified game
		/// </summary>
		/// <param name="game">The game to get possible moves for.</param>
		/// <returns>Number of possible moves for specified game</returns>
		public static int GetNumberOfPossibleMoves(Game game)
		{
			int moves = 0;
			foreach (Cell cell in game.Cells)
			{
				if (cell.HasCard)
				{
					foreach (Cascade cascade in game.Cascades)
					{
						if (game.Rules.CanMoveToCascade(cell.Card, cascade))
							moves++;
					}
					foreach (Foundation foundation in game.Foundations)
					{
						if (game.Rules.CanMoveToFoundation(cell.Card, foundation))
							moves++;
					}
				}
			}

			foreach (Cascade cascade in game.Cascades)
			{
				if (cascade.Count != 0)
				{
					List<Card> tableaux = GetTableauxFromCascade(cascade, game.Rules);
					foreach (Cascade c2 in game.Cascades)
					{
						if (cascade != c2)
						{
							if (game.Rules.CanMoveToCascade(tableaux, c2, game))
								moves++;
						}
					}

					foreach (Foundation foundation in game.Foundations)
					{

						if (tableaux.Count != 0 && game.Rules.CanMoveToFoundation(tableaux.Last(), foundation))
							moves++;
					}

					foreach (Cell cell in game.Cells)
					{
						if (!cell.HasCard)
							moves++;
					}

				}
			}
	
			
			return moves;
		}

		/// <summary>
		/// Gets a tableaux from cascade of cards.
		/// </summary>
		/// <param name="cascade">The cascade to get tableaux from.</param>
		/// <param name="rules">The rules to use when determining what makes a tableaux.</param>
		/// <returns>
		/// A list of linking cards from the top of a cascade
		/// </returns>
		public  static List<Card> GetTableauxFromCascade(Cascade cascade, IRules rules)
		{
			List<Card> tableaux = new List<Card>();
			for (int i = (cascade.Count - 1); i > 0; i--)
			{
				tableaux.Add(cascade[i]);
				if (!rules.DoCardsLinkInCascade(cascade[i], cascade[i - 1]))
					break;
			}
			tableaux.Reverse();
			return tableaux;
		}

		/// <summary>
		/// Gets the maximum number of moves for the specified game.
		/// </summary>
		/// <param name="game">The game to get maximum number of moves for.</param>
		/// <param name="destinationIsEmpty">if set to <c>true</c> destination cascade is empty.</param>
		/// <returns></returns>
		public static int GetMaximumMovesForGame(Game game, bool destinationIsEmpty)
		{
			int emptyCells = (from c in game.Cells where !c.HasCard select c).Count<Cell>();
			int emptyCascades = (from c in game.Cascades where c.Count == 0 select c).Count<Cascade>();

			if (destinationIsEmpty)
				emptyCascades--;

			int maxMoves = ((emptyCells + 1) * (emptyCascades + 1));
			if (destinationIsEmpty)
				maxMoves = ((emptyCells + 1) * (emptyCascades));
			return maxMoves;
		}

	}

}
