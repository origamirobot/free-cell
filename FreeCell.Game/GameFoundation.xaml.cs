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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FreeCell.Core;

namespace FreeCell.Game
{
	
	/// <summary>
	/// Interaction logic for GameFoundation.xaml
	/// </summary>
	public partial class GameFoundation : UserControl
	{

		private Foundation _foundation;

		/// <summary>
		/// Initializes a new instance of the <see cref="GameFoundation"/> class.
		/// </summary>
		/// <param name="foundation">The foundation.</param>
		public GameFoundation(Foundation foundation)
		{
			InitializeComponent();
			_foundation = foundation;
		}
	
	}

}
