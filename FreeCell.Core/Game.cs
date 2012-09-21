using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCell.Core
{

	/// <summary>
	/// 
	/// </summary>
	public class Game
	{

		#region CONSTANTS

		private const int DECK_SIZE = 52;

		#endregion CONSTANTS 

		#region PRIVATE PROPERTIES

		private DateTime _startTime;
		private IRules _rules;
		private Stack<State> _states;

		#endregion PRIVATE PROPERTIES 

		#region PUBLIC ACCESSORS

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Game"/> is finished.
		/// </summary>
		/// <value><c>true</c> if finished; otherwise, <c>false</c>.</value>
		public bool Finished { get; private set;  }

		/// <summary>
		/// Gets or sets the cascades.
		/// </summary>
		public List<Cascade> Cascades { get; set; }

		/// <summary>
		/// Gets or sets the foundations.
		/// </summary>
		public List<Foundation> Foundations { get; set; }

		/// <summary>
		/// Gets or sets the cells.
		/// </summary>
		public List<Cell> Cells { get; set; }

		/// <summary>
		/// Gets the total number of moves made so far this game.
		/// </summary>
		public int TotalMovesMade { get; private set; }

		/// <summary>
		/// Gets the elapsed amount of time since this game was started.
		/// </summary>
		public TimeSpan ElapsedTime { get { return (DateTime.Now - _startTime); } }

		/// <summary>
		/// Gets the game number.
		/// </summary>
		public int GameNumber { get; private set; }


		/// <summary>
		/// Gets the rules.
		/// </summary>
		public IRules Rules { get { return _rules; } }

		#endregion PUBLIC ACCESSORS 

		#region CONSTRUCTORS

		
		/// <summary>
		/// Initializes a new instance of the <see cref="Game"/> class.
		/// </summary>
		public Game() : this(new DefaultRules())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Game"/> class.
		/// </summary>
		/// <param name="rules">The rules.</param>
		public Game(IRules rules)
		{
			_states = new Stack<State>();
			_rules = rules;
			InitializeGame();
		}


		#endregion CONSTRUCTORS 

		#region PRIVATE METHODS


		/// <summary>
		/// Simulates a random shuffle by returning an array of card values in random order.
		/// </summary>
		/// <param name="seed">The seed number is used to seed the randomizer. This is the argument that allows for game numbers.</param>
		/// <returns>A random array of integers representing card values.</returns>
		private int[] Shuffle(int seed)
		{
			List<int> deal = new List<int>();
			int totalDealt = 0;
			Random rnd = new Random(seed);
			while (totalDealt < DECK_SIZE)
			{
				int next = rnd.Next((DECK_SIZE + 1));
				while (deal.Contains(next) && next != 0)
				{
					next = (next + 1) % DECK_SIZE;
				}
				if (next == 0)
					continue;
				deal.Add(next);
				totalDealt++;
			}
			return deal.ToArray();
		}

		/// <summary>
		/// Initializes all the items in the game.
		/// </summary>
		private void InitializeGame()
		{

			// INITIALIZE FOUNDATIONS
			Foundations = new List<Foundation>();
			for (int i = 0; i < 4; i++)
				Foundations.Add(new Foundation());

			// INITIALIZE CELLS
			Cells = new List<Cell>();
			for (int i = 0; i < _rules.NumberOfCells; i++)
				Cells.Add(new Cell());

			// INITIALIZE CASCADES
			Cascades = new List<Cascade>();
			for (int i = 0; i < _rules.NumberOfCascades; i++)
				Cascades.Add(new Cascade());

		}

		/// <summary>
		/// Analyzes the current game board and determins which cards (if any) it can automatically send to the foundation stacks.
		/// </summary>
		protected virtual void AutoBank()
		{
			if (!_rules.BankCardsAutomatically)
				return;


		}

		/// <summary>
		/// Checks for game for a finish.
		/// </summary>
		protected virtual bool IsGameFinished()
		{
			foreach (Foundation foundation in Foundations)
			{
				if (foundation.Count != 13)
					return false;
			}
			return true;
		}



		#endregion PRIVATE METHODS 

		#region PUBLIC METHODS


		/// <summary>
		/// Starts the game.
		/// </summary>
		public void StartGame()
		{
			// CALL START GAME BUT WITH A RANDOM GAME NUMBER
			StartGame((int)(DateTime.Now.Ticks % Int32.MaxValue));
		}

		/// <summary>
		/// Starts the game.
		/// </summary>
		/// <param name="gameNumber">The game number.</param>
		public void StartGame(int gameNumber)
		{
			if (gameNumber < 1)
				throw new ArgumentOutOfRangeException("The game number argument cannot be less than one");

			GameNumber = gameNumber;
			TotalMovesMade = 0;

			int[] shuffleData = Shuffle(GameNumber);
			InitializeGame();

			int column = 0;
			foreach (int value in shuffleData)
			{
				Cascades[column].Add(new Card(value));
				column++;
				if (column == Cascades.Count)
					column = 0;
			}

			_startTime = DateTime.Now;
			OnLayoutChanged();
		}

		/// <summary>
		/// Resets the game.
		/// </summary>
		public void ResetGame()
		{
			if (GameNumber == 0)
				throw new InvalidOperationException("You cannot reset a game before a game is started. You must start a new game before you can use this method.");

			// CALL START GAME PASSING IT THE CURRENT GAME NUMBER (RESETTING GAME).
			StartGame(GameNumber);
		}

		/// <summary>
		/// Moves the specified source.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		public virtual void Move(ITarget from, ITarget to)
		{
			MoveStartedEventArgs args = new MoveStartedEventArgs(from, to);
			OnMoveStarted(args);
			if (args.Cancel)
				return;

			if ((from is Cascade) && (to is Cascade))
			{
				if (((Cascade)from).Count == 0)
					throw new InvalidOperationException("There are no cards in the specified cascade to move to other cascade");

				Cascade source = ((Cascade)from);
				Cascade dest = ((Cascade)to);

				Card landingCard = dest.LastOrDefault();
				List<Card> tableaux = Utility.GetTableauxFromCascade(source, _rules);

				if (tableaux.Count == 1)
				{
					// MOVING ONE CARD
					if (_rules.CanMoveToCascade(source.Last(), dest))
					{
						dest.Add(source.Last());
						source.Remove(source.Last());
						OnMoveFinished(MoveFinishedEventArgs.SuccessfulResult);
						return;
					}
					else
					{
						OnMoveFinished(MoveFinishedEventArgs.UnlinkingCardsResult);
						return;
					}
				}
				else
				{
					// MOVING MULTIPLE CARDS... FIND THE MAXIMUM NUMBER OF MOVES AVAILABLE.
					int maxMoves = Utility.GetMaximumMovesForGame(this, (dest.Count == 0));
					int level = 1;
					// WALK BACKWARDS OVER TABLEAUX TRYING EACH CARD AS A LINK TO LANDING CARD.
					for (int i = (tableaux.Count - 1); i >= 0; i--)
					{
						// CHECK FOR THE LINK BETWEEN CURRENT TABLEAUX CARD AND LANDING CARD.
						if(_rules.DoCardsLinkInCascade(tableaux[i], landingCard))
						{
							// CARDS LINK, CHECK TO SEE IF WE'VE EXCEEDED OUT MAX NUMBER OF MOVES ALLOWED.
							if (level > maxMoves)
							{
								// MAX NUMBER OF MOVES EXCEEDED, MOVE FAILS
								OnMoveFinished(MoveFinishedEventArgs.NotEnoughFreeSpaceResult);
							}
							else
							{
								// MOVE CARDS STARTING BY ADDING ALL THE CARDS FROM TABLEAUX AND ADDING THEM TO DESTINATION.
								for (int j = i; j < tableaux.Count; j++)
								{
									dest.Add(tableaux[j]);
									source.Remove(tableaux[j]);
								}
								OnMoveFinished(MoveFinishedEventArgs.SuccessfulResult);
								return;
							}
						}
						else
						{
							// CARDS DO NOT LINK, CONTINUE ONTO THE NEXT TABLEAUX CARD.
							if (i == 0)
							{
								// THIS IS THE LAST CARD IN THE TABLEAUX, MOVE FAILS BECAUSE NO LINKING CARDS FOUND IN STACK.
								OnMoveFinished(MoveFinishedEventArgs.UnlinkingCardsResult);
							}
						}
						level++;
					}
				}
			}
			else if ((from is Cascade) && (to is Cell))
			{
				if (((Cascade)from).Count == 0)
					throw new InvalidOperationException("There are no cards in the specified cascade to move to cell");

				if (((Cell)to).HasCard)
				{
					OnMoveFinished(MoveFinishedEventArgs.OccupiedCellResult);
					return;
				}

				Card movingCard = ((Cascade)from).Last();
				((Cascade)from).Remove(movingCard);
				((Cell)to).Card = movingCard;
				OnMoveFinished(MoveFinishedEventArgs.SuccessfulResult);
				return;
			}
			else if ((from is Cascade) && (to is Foundation))
			{
				if (((Cascade)from).Count == 0)
					throw new InvalidOperationException("There are no cards in the specified cascade to move to a foundation");

				Card movingCard = ((Cascade)from).Last();
				if (_rules.CanMoveToFoundation(movingCard, ((Foundation)to)))
				{
					((Cascade)from).Remove(movingCard);
					((Foundation)to).Add(movingCard);
					OnMoveFinished(MoveFinishedEventArgs.SuccessfulResult);
					return;
				}
				else
				{
					OnMoveFinished(MoveFinishedEventArgs.UnlinkingCardsResult);
					return;
				}
			}
			else if ((from is Cell) && (to is Cell))
			{
				if (!((Cell)from).HasCard)
					throw new InvalidOperationException("There are no cards in the specified cell to move to cell");

				if (((Cell)to).HasCard)
				{
					OnMoveFinished(MoveFinishedEventArgs.OccupiedCellResult);
					return;
				}
				else
				{
					((Cell)to).Card = ((Cell)from).Card.Clone();
					((Cell)from).Card = null;
					OnMoveFinished(MoveFinishedEventArgs.SuccessfulResult);
					return;
				}
			}
			else if ((from is Cell) && (to is Cascade))
			{
				if (!((Cell)from).HasCard)
					throw new InvalidOperationException("There are no cards in the specified cell to move to cascade");

				Cascade cascade = ((Cascade)to);
				Cell cell = ((Cell)from);

				if (_rules.CanMoveToCascade(cell.Card, cascade))
				{
					Card movingCard = cell.Card.Clone();
					cell.Card = null;
					cascade.Add(movingCard);
					OnMoveFinished(MoveFinishedEventArgs.SuccessfulResult);
					return;
				}
				else
				{
					OnMoveFinished(MoveFinishedEventArgs.UnlinkingCardsResult);
					return;
				}
			}
			else if ((from is Cell) && (to is Foundation))
			{
				if (!((Cell)from).HasCard)
					throw new InvalidOperationException("There are no cards in the specified cell to move to foundation");

				Card movingCard = ((Cell)from).Card.Clone();
				if (_rules.CanMoveToFoundation(movingCard, ((Foundation)to)))
				{
					((Cell)from).Card = null;
					((Foundation)to).Add(movingCard);
					OnMoveFinished(MoveFinishedEventArgs.SuccessfulResult);
					return;
				}
				else
				{
					OnMoveFinished(MoveFinishedEventArgs.UnlinkingCardsResult);
					return;
				}
			}
			else if ((from is Foundation) && (to is Cell))
			{
				if (_rules.AllowMoveFromFoundation)
				{
					if (((Foundation)from).Count == 0)
						throw new InvalidOperationException("There are no cards in the specified foundation to move to a cell");

					if (((Cell)to).HasCard)
					{
						OnMoveFinished(MoveFinishedEventArgs.OccupiedCellResult);
						return;
					}
					else
					{
						((Cell)to).Card = ((Foundation)from).Last();
						OnMoveFinished(MoveFinishedEventArgs.SuccessfulResult);
						return;
					}
				}
			}
			else if ((from is Foundation) && (to is Cascade))
			{
				if (_rules.AllowMoveFromFoundation)
				{
					if (((Foundation)from).Count == 0)
						throw new InvalidOperationException("There are no cards in the specified foundation to move to a cascade");

					Card movingCard = ((Foundation)from).Last();
					if (_rules.CanMoveToCascade(movingCard, ((Cascade)to)))
					{
						((Foundation)from).Remove(movingCard);
						((Cascade)to).Add(movingCard);
						OnMoveFinished(MoveFinishedEventArgs.SuccessfulResult);
						return;
					}
					else
					{
						OnMoveFinished(MoveFinishedEventArgs.UnlinkingCardsResult);
						return;
					}

				}
			}
			else if ((from is Foundation) && (to is Foundation))
			{
				throw new InvalidOperationException("You cannot move a card from one foundation to another.");
			}
		}

		/// <summary>
		/// Undoes this instance.
		/// </summary>
		public virtual void Undo()
		{
			State prevState = _states.Pop();
			Cascades = prevState.Cascades;
			Foundations = prevState.Foundations;
			Cells = prevState.Cells;
			OnLayoutChanged();
		}


		#endregion PUBLIC METHODS 

		#region PUBLIC EVENTS


		/// <summary>
		/// Occurs when a move is started.
		/// </summary>
		public event EventHandler<MoveStartedEventArgs> MoveStarted;

		/// <summary>
		/// Occurs when a move is completed.
		/// </summary>
		public event EventHandler<MoveFinishedEventArgs> MoveFinished;

		/// <summary>
		/// Occurs when the game layout is changed.
		/// </summary>
		/// <remarks>This is essentially the redraw event. It tells the UI to redraw based on the current game.</remarks>
		public event EventHandler LayoutChanged;

		/// <summary>
		/// Occurs when the game is started
		/// </summary>
		public event EventHandler<GameStartEventArgs> GameStarted;

		/// <summary>
		/// Occurs when the game is starting
		/// </summary>
		public event EventHandler<GameStartEventArgs> GameStarting;

		/// <summary>
		/// Occurs when the game starts auto banking a card.
		/// </summary>
		public event EventHandler<AutoBankEventArgs> AutoBankingCard;

		/// <summary>
		/// Occurs when the game has auto banked card.
		/// </summary>
		public event EventHandler<AutoBankEventArgs> AutoBankedCard;

		/// <summary>
		/// Occurs when the game is finished.
		/// </summary>
		public event EventHandler GameFinished;



		/// <summary>
		/// Raises the <see cref="E:LayoutChanged"/> event.
		/// </summary>
		public void OnLayoutChanged()
		{
			// FIRE THE LAYOUT CHANGED EVENT
			if (LayoutChanged != null)
				LayoutChanged(this, EventArgs.Empty);

			// AUTO BANK ANY CARDS THAT ARE ELIGIBLE
			if (_rules.BankCardsAutomatically)
				AutoBank();

			// CHECK TO SEE IF THE GAME IS COMPLETED
			if (IsGameFinished())
				OnGameFinished();

			_states.Push(new State(this));
		}

		/// <summary>
		/// Raises the <see cref="E:MoveStarted"/> event.
		/// </summary>
		/// <param name="args">The <see cref="FreeCell.Core.MoveStartedEventArgs"/> instance containing the event data.</param>
		public void OnMoveStarted(MoveStartedEventArgs args)
		{
			if (MoveStarted != null)
				MoveStarted(this, args);
		}

		/// <summary>
		/// Raises the <see cref="E:MoveFinished"/> event.
		/// </summary>
		/// <param name="args">The <see cref="FreeCell.Core.MoveFinishedEventArgs"/> instance containing the event data.</param>
		public void OnMoveFinished(MoveFinishedEventArgs args)
		{
			if (MoveFinished != null)
				MoveFinished(this, args);
			if(args.Success)
				OnLayoutChanged();
		}

		/// <summary>
		/// Raises the <see cref="E:GameStarting"/> event.
		/// </summary>
		/// <param name="args">The <see cref="FreeCell.Library.GameStartEventArgs"/> instance containing the event data.</param>
		protected void OnGameStarting(GameStartEventArgs args)
		{
			if (GameStarting != null)
				GameStarting(this, args);
		}

		/// <summary>
		/// Raises the <see cref="E:GameStarted"/> event.
		/// </summary>
		/// <param name="args">The <see cref="FreeCell.Library.GameStartEventArgs"/> instance containing the event data.</param>
		protected void OnGameStarted(GameStartEventArgs args)
		{
			if (GameStarted != null)
				GameStarted(this, args);
		}

		/// <summary>
		/// Raises the <see cref="E:AutoBankingCard"/> event.
		/// </summary>
		/// <param name="args">The <see cref="FreeCell.Library.AutoBankEventArgs"/> instance containing the event data.</param>
		protected void OnAutoBankingCard(AutoBankEventArgs args)
		{
			if (AutoBankingCard != null)
				AutoBankingCard(this, args);
		}

		/// <summary>
		/// Raises the <see cref="E:AutoBankedCard"/> event.
		/// </summary>
		/// <param name="args">The <see cref="FreeCell.Library.AutoBankEventArgs"/> instance containing the event data.</param>
		protected void OnAutoBankedCard(AutoBankEventArgs args)
		{
			if (AutoBankedCard != null)
				AutoBankedCard(this, args);
		}

		/// <summary>
		/// Called when the game is finished.
		/// </summary>
		protected void OnGameFinished()
		{
			Finished = true;
			if (GameFinished != null)
				GameFinished(this, EventArgs.Empty);
		}


		#endregion PUBLIC EVENTS 

	}

}
