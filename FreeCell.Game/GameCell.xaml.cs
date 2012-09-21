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
	/// Interaction logic for GameCell.xaml
	/// </summary>
	public partial class GameCell : UserControl
	{

		/// <summary>
		/// Gets or sets the cell.
		/// </summary>
		public Cell Cell { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="GameCell"/> class.
		/// </summary>
		public GameCell(Cell cell)
		{
			InitializeComponent();
		}

		/// <summary>
		/// Refreshes this instance.
		/// </summary>
		public void Refresh()
		{
			if (Cell.HasCard)
			{
				mask.Background = App.Current.Resources["CardBackground"] as SolidColorBrush;
				mask.BorderBrush = App.Current.Resources["CardDefaultBorder"] as SolidColorBrush;
				Glyph1.Visibility = System.Windows.Visibility.Visible;
				Glyph2.Visibility = System.Windows.Visibility.Visible;
				Rank1.Visibility = System.Windows.Visibility.Visible;
				switch (Cell.Card.Suit)
				{
					case CardSuit.Club: Glyph1.Background = Glyph2.Background = App.Current.Resources["ClubGlyph"] as DrawingBrush; break;
					case CardSuit.Diamond: Glyph1.Background = Glyph2.Background = App.Current.Resources["DiamondGlyph"] as DrawingBrush; break;
					case CardSuit.Heart: Glyph1.Background = Glyph2.Background = App.Current.Resources["HeartGlyph"] as DrawingBrush; break;
					case CardSuit.Spade: Glyph1.Background = Glyph2.Background = App.Current.Resources["SpadeGlyph"] as DrawingBrush; break;
				}
				switch (Cell.Card.Rank)
				{
					case CardRank.Ace: Rank1.Content = "A"; break;
					case CardRank.Two: Rank1.Content = "2"; break;
					case CardRank.Three: Rank1.Content = "3"; break;
					case CardRank.Four: Rank1.Content = "4"; break;
					case CardRank.Five: Rank1.Content = "5"; break;
					case CardRank.Six: Rank1.Content = "6"; break;
					case CardRank.Seven: Rank1.Content = "7"; break;
					case CardRank.Eight: Rank1.Content = "8"; break;
					case CardRank.Nine: Rank1.Content = "9"; break;
					case CardRank.Ten: Rank1.Content = "10"; break;
					case CardRank.Jack: Rank1.Content = "J"; break;
					case CardRank.Queen: Rank1.Content = "Q"; break;
					case CardRank.King: Rank1.Content = "K"; break;
				}
			}
			else
			{
				Glyph1.Visibility = System.Windows.Visibility.Hidden;
				Glyph2.Visibility = System.Windows.Visibility.Hidden;
				Rank1.Visibility = System.Windows.Visibility.Hidden;
				mask.Background = App.Current.Resources["EmptyCellBackground"] as SolidColorBrush;
				mask.BorderBrush = App.Current.Resources["EmptyCellBorder"] as SolidColorBrush;
			}
		}

	}

}
