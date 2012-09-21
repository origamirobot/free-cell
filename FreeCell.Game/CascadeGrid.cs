using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using FreeCell.Core;

namespace FreeCell.Game
{

	/// <summary>
	/// 
	/// </summary>
	public class CascadeGrid : Grid
	{

		#region PRIVATE PROPERTIES

		private FreeCell.Core.Game _game;
		private int _topCardMargin = 80;

		#endregion PRIVATE PROPERTIES 

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="CascadeGrid"/> class.
		/// </summary>
		/// <param name="game">The game.</param>
		public CascadeGrid(FreeCell.Core.Game game)
		{
			_game = game;
			_game.LayoutChanged += new EventHandler(Game_LayoutChanged);
			for (int i = 0; i < _game.Rules.NumberOfCascades; i++)
			{
				ColumnDefinition cd = new ColumnDefinition();
				cd.MouseDown += new System.Windows.Input.MouseButtonEventHandler(Column_MouseDown);
				this.ColumnDefinitions.Add(cd);
			}
		}


		#endregion CONSTRUCTORS

		#region EVENT HANDLERS


		/// <summary>
		/// Handles the LayoutChanged event of the Game control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void Game_LayoutChanged(object sender, EventArgs e)
		{
			int columnIndex = 0;
			foreach (Cascade cascade in _game.Cascades)
			{
				double topOffset = 0;
				foreach (Card card in cascade)
				{
					GameCard c = new GameCard(card) { Margin = new System.Windows.Thickness(0, topOffset, 0, 0) };
					this.Children.Add(c);
					Grid.SetColumn(c, columnIndex);
					topOffset = topOffset + _topCardMargin;
				}
				columnIndex++;
			}
		}

		/// <summary>
		/// Handles the MouseDown event of the Column control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
		protected void Column_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			ColumnDefinition def = sender as ColumnDefinition;
			
		}


		#endregion EVENT HANDLERS 

		#region PUBLIC EVENTS


		/// <summary>
		/// Occurs when a cascade is selected.
		/// </summary>
		public event EventHandler<CascadeEventArgs> CascadeSelected;

		/// <summary>
		/// Occurs when a cascade  is deselected.
		/// </summary>
		public event EventHandler<CascadeEventArgs> CascadeDeselected;



		/// <summary>
		/// Called when a cascade is selected.
		/// </summary>
		protected void OnCascadeSelected()
		{
			//if (CascadeSelected != null)
			//    CascadeSelected(this, EventArgs.Empty);
		}

		/// <summary>
		/// Called when a cascade is deselected.
		/// </summary>
		protected void OnCascadeDeselected()
		{
			//if (CascadeDeselected != null)
			//    CascadeDeselected(this, EventArgs.Empty);
		}


		#endregion PUBLIC EVENTS 

	}

	/// <summary>
	/// 
	/// </summary>
	public class CascadeGridColumnDefinition : ColumnDefinition
	{

		#region PUBLIC ACCESSORS

		/// <summary>
		/// Gets the index.
		/// </summary>
		public int ColumnIndex { get; private set; }

		/// <summary>
		/// Gets the cascade.
		/// </summary>
		public Cascade Cascade { get; private set; }


		#endregion PUBLIC ACCESSORS 

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="CascadeGridColumnDefinition"/> class.
		/// </summary>
		public CascadeGridColumnDefinition(Cascade cascade, int index)
		{
			ColumnIndex = index;
			Cascade = cascade;
		}


		#endregion CONSTRUCTORS 

	}

	/// <summary>
	/// 
	/// </summary>
	public class CascadeEventArgs : EventArgs
	{

		/// <summary>
		/// Gets the cascade.
		/// </summary>
		public Cascade Cascade { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CascadeEventArgs"/> class.
		/// </summary>
		/// <param name="cascade">The cascade.</param>
		public CascadeEventArgs(Cascade cascade)
		{
			Cascade = cascade;
		}

	}




}
