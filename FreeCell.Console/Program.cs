using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeCell.Core;

namespace FreeCell.DOS
{

    /// <summary>
    /// 
    /// </summary>
    class Program
    {

        static int CardWidth = 5;
        static int CardHeight = 5;
        static int ConsoleTop = 2;
        static int ConsoleLeft = 1;
        static Location FocusLocation = Location.Cells;
        static int FocusIndex = -1;
        static Location SelectedLocation = Location.Cells;
        static int SelectedIndex = -1;
        static ConsoleColor FocusedColor = ConsoleColor.Yellow;
        static ConsoleColor SelectedColor = ConsoleColor.Green;
        static ConsoleColor NormalColor = ConsoleColor.White;
        static Game Game;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            Game = new Game();
            Game.LayoutChanged += new EventHandler(Game_LayoutChanged);
			Game.MoveFinished += new EventHandler<MoveFinishedEventArgs>(Game_MoveFinished);
            Game.StartGame(1947380511);
            DrawGame();
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
					case ConsoleKey.U:
						Game.Undo();
						break;
                    case ConsoleKey.LeftArrow:
                        FocusIndex -= 1;
                        break;
                    case ConsoleKey.RightArrow:
                        FocusIndex += 1;
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        if (FocusLocation == Location.Banks || FocusLocation == Location.Cells)
                        {
                            FocusLocation = Location.Stacks;
                        }
                        else
                        {
                            if (FocusIndex > 3) FocusLocation = Location.Banks;
                            else FocusLocation = Location.Cells;
                        }
                        break;
                    case ConsoleKey.Enter :
                        if (FocusIndex == SelectedIndex && FocusLocation == SelectedLocation)
                        {
                            SelectedIndex = -1;
                        }
                        else
                        {
							if (SelectedIndex == -1)
							{
								SelectedLocation = FocusLocation;
								SelectedIndex = FocusIndex;
							}
							else
							{
								// TRY TO MOVE A CARD
								ITarget source = null;
								ITarget dest = null;
								if (SelectedLocation == Location.Stacks) source = Game.Cascades[SelectedIndex];
								if (SelectedLocation == Location.Cells) source = Game.Cells[SelectedIndex];
								if (FocusLocation == Location.Stacks) dest = Game.Cascades[FocusIndex];
								if (FocusLocation == Location.Cells) dest = Game.Cells[FocusIndex];
								if (FocusLocation == Location.Banks) dest = Game.Foundations[FocusIndex];
								Game.Move(source, dest);
							}
                        }
                        break;
                    case ConsoleKey.Escape: break;
                }

                if (FocusLocation == Location.Cells && FocusIndex >= Game.Rules.NumberOfCells)
                {
                    FocusIndex = 0;
                    FocusLocation = Location.Banks;
                }
                if (FocusLocation == Location.Cells && FocusIndex < 0)
                {
                    FocusIndex = 3;
                    FocusLocation = Location.Banks;
                }
                if (FocusLocation == Location.Banks && FocusIndex >= 4)
                {
                    FocusIndex = 0;
                    FocusLocation = Location.Cells;
                }
                if (FocusLocation == Location.Banks && FocusIndex < 0)
                {
                    FocusIndex = (Game.Rules.NumberOfCells - 1);
                    FocusLocation = Location.Cells;
                }
                if (FocusLocation == Location.Stacks && FocusIndex >= Game.Rules.NumberOfCascades) FocusIndex = 0;
				if (FocusLocation == Location.Stacks && FocusIndex < 0) FocusIndex = (Game.Rules.NumberOfCascades - 1);

                DrawGame();
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void Game_MoveFinished(object sender, MoveFinishedEventArgs e)
		{
			if (!e.Success)
				ShowMessage("Illegal Move", true);
		}

        /// <summary>
        /// Handles the LayoutChanged event of the Game control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void Game_LayoutChanged(object sender, EventArgs e)
        {
			SelectedIndex = -1;
            DrawGame();
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        static void DrawGame()
        {
			Console.Clear();
			Console.SetCursorPosition(ConsoleLeft, ConsoleTop);
            // DRAW FREE CELLS
            int cardTopPos = ConsoleTop;
            int cardLeftPos = ConsoleLeft;
            int index = 0;
            foreach (Cell cell in Game.Cells)
            {
                Card card = (!cell.HasCard) ? null : cell.Card;
                if (FocusLocation == Location.Cells && FocusIndex == index) Console.ForegroundColor = FocusedColor;
                if (SelectedLocation == Location.Cells && SelectedIndex == index) Console.ForegroundColor = SelectedColor;

                DrawCard(cardTopPos, cardLeftPos, card);
                cardLeftPos += (CardWidth + 3);
                index++;
                Console.ForegroundColor = NormalColor;
            }
            index = 0;
            cardLeftPos += 8;
            // DRAW BANKS
            foreach (Foundation bank in Game.Foundations)
            {
                Card card = (bank.Count == 0) ? null : bank.Last();
                if (FocusLocation == Location.Banks && FocusIndex == index) Console.ForegroundColor = FocusedColor;
                if (SelectedLocation == Location.Banks && SelectedIndex == index) Console.ForegroundColor = SelectedColor;
                DrawCard(cardTopPos, cardLeftPos, card);
                cardLeftPos += (CardWidth + 3);
                index++;
                Console.ForegroundColor = NormalColor;
            }

            cardLeftPos = ConsoleLeft;
            cardTopPos = cardTopPos += (CardHeight + 3);
            int stackTopPos = cardTopPos;
            index = 0;
            // DRAW STACKS
            foreach (Cascade stack in Game.Cascades)
            {
                cardTopPos = stackTopPos;
                int i = 1;
                foreach (Card card in stack)
                {
                    Console.SetCursorPosition(cardLeftPos, cardTopPos);
                    if (FocusLocation == Location.Stacks && FocusIndex == index && i == stack.Count) Console.ForegroundColor = FocusedColor;
                    if (SelectedLocation == Location.Stacks && SelectedIndex == index && i == stack.Count) Console.ForegroundColor = SelectedColor;
                    DrawCard(cardTopPos, cardLeftPos, card);
                    cardTopPos += 2;
                    Console.ForegroundColor = NormalColor;
                    i++;
                }
                cardLeftPos += (CardWidth + 4);
                index++;
            }
			int moves = Utility.GetNumberOfPossibleMoves(Game);
			ShowMessage("Possible Moves: " + moves, false);
        }

        /// <summary>
        /// Converts a card to text to be displayed in the console.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        static string CardToText(Card card)
        {
            string output = "";
            switch (card.Suit)
            {
                case CardSuit.Club:     output += "♣"; break;
                case CardSuit.Diamond:  output += "♦"; break;
                case CardSuit.Heart:    output += "♥"; break;
                case CardSuit.Spade:    output += "♠"; break;
            }
            switch (card.Rank)
            {
                case CardRank.Ace:     output += "A "; break;
				case CardRank.Two: output += "2 "; break;
				case CardRank.Three: output += "3 "; break;
				case CardRank.Four: output += "4 "; break;
				case CardRank.Five: output += "5 "; break;
				case CardRank.Six: output += "6 "; break;
				case CardRank.Seven: output += "7 "; break;
				case CardRank.Eight: output += "8 "; break;
				case CardRank.Nine: output += "9 "; break;
				case CardRank.Ten: output += "T"; break;
				case CardRank.Jack: output += "J "; break;
				case CardRank.Queen: output += "Q "; break;
				case CardRank.King: output += "K "; break;
            }
            return output;
        }

        /// <summary>
        /// Draws the card.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="left">The left.</param>
        /// <param name="card">The card.</param>
        static void DrawCard(int top, int left, Card card)
        {
            Console.SetCursorPosition(left, top);
            Console.Write("┌");
            for (int i = 0; i < CardWidth; i++)
                Console.Write("─");
            Console.Write("┐");
            
            Console.SetCursorPosition(left, ++top);            
            Console.Write("│");
            if(card != null)
            {
                string text = CardToText(card);
                Console.Write(text.PadRight(CardWidth));
            }
            else
            {
                for(int i = 0; i < CardWidth; i++)
                    Console.Write(" ");
            }
            Console.Write("│");
            for (int i = 0; i < (CardHeight-1); i++)
            {
                Console.SetCursorPosition(left, ++top);
                Console.Write("│");
                for (int j = 0; j < CardWidth; j++)
                    Console.Write(" ");
                Console.Write("│");
            }
            Console.SetCursorPosition(left, ++top);
            Console.Write("└");
            for (int i = 0; i < CardWidth; i++)
                Console.Write("─");
            Console.Write("┘");
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="isError"></param>
		static void ShowMessage(string message, bool isError)
		{
			if (isError)
				Console.ForegroundColor = ConsoleColor.Red;
			Console.SetCursorPosition(1, 1);
			Console.Write(message);
			Console.ForegroundColor = NormalColor;
		}

        /// <summary>
        /// 
        /// </summary>
        enum Location
        {
            /// <summary>
            /// 
            /// </summary>
            Cells,
            /// <summary>
            /// 
            /// </summary>
            Stacks,
            /// <summary>
            /// 
            /// </summary>
            Banks
        }

    }
}
