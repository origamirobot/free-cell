using System;

namespace FreeCell.Core
{
	
	/// <summary>
	/// Contains event data for the GameStart events of the Game class.
	/// </summary>
	public class GameStartEventArgs : EventArgs
    {

		/// <summary>
		/// Gets or sets the game number.
		/// </summary>
        public int GameNumber { get; private set; }

		/// <summary>
		/// Gets or sets the start time.
		/// </summary>
		public DateTime StartTime { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="GameStartEventArgs"/> class.
		/// </summary>
		/// <param name="gameNumber">The game number.</param>
        public GameStartEventArgs(int gameNumber, DateTime start)
        {
            GameNumber = gameNumber;
			StartTime = start;
        }

    }

	/// <summary>
	/// Contains event data for the MoveStarted events of the Game class.
	/// </summary>
	public class MoveStartedEventArgs : EventArgs
	{

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MoveStartedEventArgs"/> is cancel.
		/// </summary>
		/// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
		public bool Cancel { get; private set; }

		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>The source.</value>
		public ITarget Source { get; private set; }

		/// <summary>
		/// Gets or sets the destination.
		/// </summary>
		/// <value>The destination.</value>
		public ITarget Destination { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MoveStartedEventArgs"/> class.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		public MoveStartedEventArgs(ITarget source, ITarget destination)
		{
			Source = source;
			Destination = destination;
		}

	}

    /// <summary>
    /// Contains event data for the MoveFinished events of the Game class.
    /// </summary>
    public class MoveFinishedEventArgs : EventArgs
    {

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="MoveFinishedEventArgs"/> class.
		/// </summary>
		public MoveFinishedEventArgs(MoveResult result)
		{
			Result = result;
		}


		#endregion CONSTRUCTORS 

		#region PUBLIC ACCESSORS

		/// <summary>
		/// Gets or sets the result of the attempted move.
		/// </summary>
		public MoveResult Result { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the attemped move was a success.
		/// </summary>
		/// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
		public bool Success { get { return (Result == MoveResult.Success); } }


		#endregion PUBLIC ACCESSORS 

		#region PRIVATE PROPERTIES

		private static MoveFinishedEventArgs _successfulResult = new MoveFinishedEventArgs(MoveResult.Success);
		private static MoveFinishedEventArgs _freeSpaceResult = new MoveFinishedEventArgs(MoveResult.NotEnoughFreeSpace);
		private static MoveFinishedEventArgs _occupiedResult = new MoveFinishedEventArgs(MoveResult.OccupiedCell);
		private static MoveFinishedEventArgs _unlinkingResult = new MoveFinishedEventArgs(MoveResult.UnlinkingCards);

		#endregion PRIVATE PROPERTIES 

		#region STATIC PROPERTIES


		/// <summary>
		/// Gets an successful result event args.
		/// </summary>
		public static MoveFinishedEventArgs SuccessfulResult { get { return _successfulResult; } }

		/// <summary>
		/// Gets an Not Enough Free Spaces result event args.
		/// </summary>
		public static MoveFinishedEventArgs NotEnoughFreeSpaceResult { get { return _freeSpaceResult; } }

		/// <summary>
		/// Gets an Occupied Cell result event args.
		/// </summary>
		public static MoveFinishedEventArgs OccupiedCellResult { get { return _occupiedResult; } }

		/// <summary>
		/// Gets an Unlinking Cards result event args.
		/// </summary>
		public static MoveFinishedEventArgs UnlinkingCardsResult { get { return _unlinkingResult; } }


		#endregion STATIC PROPERTIES 
	
	}

	/// <summary>
	/// Contains event data for the AutoBank events of the Game class.
	/// </summary>
	public class AutoBankEventArgs : EventArgs
	{

		/// <summary>
		/// Gets or sets the card that is being auto banked.
		/// </summary>
		public Card Card { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether to cancel the auto bank operation.
		/// </summary>
		/// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
		public bool Cancel { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AutoBankEventArgs"/> class.
		/// </summary>
		public AutoBankEventArgs(Card card)
		{
			Card = card;
		}

	}

}
