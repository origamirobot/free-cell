using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FreeCell.Core;

namespace FreeCell.Game
{

	/// <summary>
	/// Interaction logic for GameBoard.xaml
	/// </summary>
	public partial class GameBoard : Window
	{

		#region PRIVATE PROPERTIES

		private FreeCell.Core.Game _game;
		private int _verticalCardSpacing = 80;
		private CascadeGrid _grdCascades;

		#endregion PRIVATE PROPERTIES 

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="GameBoard"/> class.
		/// </summary>
		public GameBoard()
		{
			InitializeComponent();
			_game = new Core.Game();
			_grdCascades = new CascadeGrid(_game) { Margin = new Thickness(16, 16, 16, 16), HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, VerticalAlignment = System.Windows.VerticalAlignment.Stretch };
			grdMain.Children.Add(_grdCascades);
			Initialize();
		}


		#endregion CONSTRUCTORS 

		#region PROTECTED METHODS


		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected void Initialize()
		{
			
			_game.StartGame();

		}


		#endregion PROTECTED METHODS 

	}


}
