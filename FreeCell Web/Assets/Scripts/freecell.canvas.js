
SUIT = { Heart: 0, Spade: 1, Diamond: 2, Club: 3 };
COLOR = { Black: 'Black', Red: 'Red' };
VALUE = { Ace: 1, Two: 2, Three: 3, Four: 4, Five: 5, Six: 6, Seven: 7, Eight: 8, Nine: 9, Ten: 10, Jack: 11, Queen: 12, King: 13 };
LOCATION = { Stack: 0, Cell: 1, Bank: 2 };


function Card(number) {
	if (number > 52 || number < 1)
		throw "Invalid Card Number " + number;

	this.Suit = Math.floor((number - 1) / 13);
	this.Color = (this.Suit == SUIT.Heart || this.Suit == SUIT.Club) ? COLOR.Red : COLOR.Black;
	this.Value = (number - (this.Suit * 13));
	switch (this.Value) {
		case VALUE.Ace: this.Text = 'Ace '; this.ValueText = 'Ace'; break;
		case VALUE.Two: this.Text = 'Two '; this.ValueText = 'Two'; break;
		case VALUE.Three: this.Text = 'Three '; this.ValueText = 'Three'; break;
		case VALUE.Four: this.Text = 'Four '; this.ValueText = 'Four'; break;
		case VALUE.Five: this.Text = 'Five '; this.ValueText = 'Five'; break;
		case VALUE.Six: this.Text = 'Six '; this.ValueText = 'Six'; break;
		case VALUE.Seven: this.Text = 'Seven '; this.ValueText = 'Seven'; break;
		case VALUE.Eight: this.Text = 'Eight '; this.ValueText = 'Eight'; break;
		case VALUE.Nine: this.Text = 'Nine '; this.ValueText = 'Nine'; break;
		case VALUE.Ten: this.Text = 'Ten '; this.ValueText = 'Ten'; break;
		case VALUE.Jack: this.Text = 'Jack '; this.ValueText = 'Jack'; break;
		case VALUE.Queen: this.Text = 'Queen '; this.ValueText = 'Queen'; break;
		case VALUE.King: this.Text = 'King '; this.ValueText = 'King'; break;
	}
	switch (this.Suit) {
		case SUIT.Heart: this.Text += 'of Hearts'; this.SuitText = 'Hearts'; break;
		case SUIT.Spade: this.Text += 'of Spades'; this.SuitText = 'Spades'; break;
		case SUIT.Diamond: this.Text += 'of Diamonds'; this.SuitText = 'Diamonds'; break;
		case SUIT.Club: this.Text += 'of Clubs'; this.SuitText = 'Clubs'; break;
	}
}



var Game = {
	rules: defaultRules,
	shuffler: defaultShuffler,
	stacks: [],
	startTime: null,
	init: function (rules, shuffler) {
		Game.rules = rules;
		Game.shuffler = shuffler;
		for (i = 0; i < rules.numberOfStacks; i++) {
			Game.stacks[i] = {
				cards: [],
				topCard: null
			};
		}

		currentStack = 0;
		shuffleData = shuffler.deal();
		for (i = 0; i < shuffleData.length; i++) {

			card = new Card(shuffleData[i] + 1);

			Game.stacks[currentStack].cards.push(card);
			Game.stacks[currentStack].topCard = card;
			currentStack++;
			if (currentStack >= rules.numberOfStacks) {
				currentStack = 0;
			}
		}

	}
};






var defaultRules = {
	numberOfStacks: 8,
	numberOfCells: 4,
	bankCardsAutomatically: true,
	enforceRules: true,
	allowMoveFromBank: true,
	canMoveToBank: function (card, bank) {
		if (!this.enforceRules) { return true; }
		if (bank.topCard == null && card.Value == VALUE.Ace) { return true; }
		else { if ((bank.topCard.Suit == card.Suit) && ((bank.topCard.Value + 1) == card.Value)) { return true; } }
		return false;
	},
	canMoveToCell: function (card, cell) {
		if (!this.enforceRules) { return true; }
		if (cell == null) { return true; }
		return false;
	},
	canMoveToStack: function (card, stack) {
		if (!this.enforceRules) { return true; }
		if (stack.topCard == null) { return true; }
		if (this.doCardsLinkInStack(card, stack.topCard)) { return true; }
		return false;
	},
	doCardsLinkInStack: function (topCard, bottomCard) {
		if (!this.enforceRules) { return true; }
		if (bottomCard == null) { return true; }
		if ((topCard.Color != bottomCard.Color) && ((topCard.Value + 1) == bottomCard.Value)) { return true; }
		return false;
	},
	doCardsLinkInBank: function (topCard, bottomCard) {
		if (!this.enforceRules) { return true; }
		if ((topCard.Suit == bottomCard.Suit) && ((topCard.Value - 1) == bottomCard.Value)) { return true; }
		return false;
	}
};


var defaultShuffler = {
	deckSize: 52,
	deal: function (seed) {
		if (seed == null) {
			seed = Math.floor(Math.random() * 10000);
		}
		dealData = [];
		totalDelt = 0;
		while (totalDelt < this.deckSize) {
			next = Math.floor(Math.random() * (52 - 1 + 1) + 1);
			if (next == 0)
				continue;
			if (dealData[next] == null) {
				dealData[next] = totalDelt;
				totalDelt++;
			}
		}
		return dealData;
	}
};



 
