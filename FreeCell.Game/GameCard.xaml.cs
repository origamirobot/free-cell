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
	/// Interaction logic for GameCard.xaml
	/// </summary>
	public partial class GameCard : UserControl
	{

		#region PRIVATE PROPERTIES

		private Card _card;
		

		#endregion PRIVATE PROPERTIES 

		#region PUBLIC ACCESSORS

		/// <summary>
		/// Gets or sets a value indicating whether this instance is selected.
		/// </summary>
		/// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
		public bool IsSelected { get; set; }

		#endregion PUBLIC ACCESSORS 

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="GameCard"/> class.
		/// </summary>
		public GameCard(Card card)
		{
			InitializeComponent();
			_card = card;
			Initialize();
		}


		#endregion CONSTRUCTORS 

		#region PROTECTED METHODS


		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected void Initialize()
		{
			switch (_card.Suit)
			{
				case CardSuit.Club: Glyph1.Background = Glyph2.Background = App.Current.Resources["ClubGlyph"] as DrawingBrush; break;
				case CardSuit.Diamond: Glyph1.Background = Glyph2.Background = App.Current.Resources["DiamondGlyph"] as DrawingBrush; break;
				case CardSuit.Heart: Glyph1.Background =  Glyph2.Background = App.Current.Resources["HeartGlyph"] as DrawingBrush; break;
				case CardSuit.Spade: Glyph1.Background = Glyph2.Background = App.Current.Resources["SpadeGlyph"] as DrawingBrush; break;
			}
			switch (_card.Rank)
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


		#endregion PROTECTED METHODS 

	}

}
