using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CardGame
{
    //========================================GAME CLASS============================================//
    public class Game
    {
        private Deck mainDeck;
        private List<Card> discardPile;

        private Player[] players;
        private Hand[] pHands;


        int NUM_CARDS;
        Random rnd = new Random();
        //==========================================GAME CLASS====================================//
        public Game()
        {
            

        }

        public void Play()
        {
            //set up objects
            Setup();
            //Shuffle the deck
            mainDeck.Shuffle();
            //Deal 9 cards to each player
            Deal();

            //get the right player
            int count = 0;
            int playerChoice = 0;

            //Add a card to the discard pile to initialize it.
            //Set its faceup value to true.  NOTE: All cards in discard pile
            //are face up.
            discardPile.Add(mainDeck.Draw());

            //Set up logic to turn 3 random cards face up per player
            int randomPosition;
            for(int i = 0; i < 3; i++)
            {
                randomPosition = rnd.Next(0, 9);

                //Loops keep checking the player hand until it finds a facedown.
                //Prevents from trying to change cards that are already face up.
                while (true)
                {
                    if (pHands[0].Peek(randomPosition).FaceUp == false)
                    {
                        pHands[0].Peek(randomPosition).FaceUp = true;
                        break;
                    }
                    randomPosition = rnd.Next(0, 9);
                }

                while(true)
                {
                    if (pHands[1].Peek(randomPosition).FaceUp == false)
                    {
                        pHands[1].Peek(randomPosition).FaceUp = true;
                        break;
                    }
                }

                
            }

            while (Turn(players[playerChoice], pHands[playerChoice]))
            {
                Console.WriteLine("\n");


                for (int i = 0; i < 9; i++)
                {
                    if (i == 3 || i == 6)
                    {
                        Console.Write("\n");
                    }
                    Console.Write("{0}{1}{2}  ", pHands[playerChoice].Peek(i).Suit, pHands[playerChoice].Peek(i).Value, pHands[playerChoice].Peek(i).FaceUp);
                }

                count++;
                playerChoice = count % 2;
            }

            //Get the score for each hand
            Console.WriteLine("\n");
            Console.WriteLine("Player 1 score: {0}", pHands[0].Score());
            Console.WriteLine("Player 2 score: {0}", pHands[1].Score());

        }

        public void Setup()
        {
            //Setup all objects here
            mainDeck = new Deck();
            discardPile = new List<Card>();
            players = new Player[2];
            pHands = new Hand[2];
            players[0] = new Player();
            players[1] = new Player();
            pHands[0] = new Hand();
            pHands[1] = new Hand();
             
        }

        private void Deal()
        {
            Card next;
            NUM_CARDS = 9;
            //for num cards
            for (int i = 0; i < NUM_CARDS; i++)
            {
                //for each hand
                for (int j = 0; j < 2; j++)
                {
                    //draw from deck
                    next = mainDeck.Draw();
                    //add to hand
                    pHands[j].Add(next);
                   
                }
            }
        }

        public bool Turn(Player player, Hand playerHand)
        {
            int cardPosition;
            Card drawnCard;
            Card discardedCard;
            //Choose whether to draw from main deck or discard pile

            if (playerHand.IsDone() == true)
                return false;

            if (player.ChooseDrawDiscard() == true)
            {
                drawnCard = mainDeck.Draw();
            }
            else
            {
                //Error check.  If drawing from discard pile, but discard pile is empty...
                if (discardPile.Count == 0)
                {
                    //then draw from the main deck instead
                    drawnCard = mainDeck.Draw();
                }
                else
                {
                    //if the discard pile is not empty, then draw
                    //from it and remove the card.
                    drawnCard = discardPile[0];
                    discardPile.RemoveAt(0);
                }
            }
            
            /*
             * Look at the drawn card and replace any card(face down or face up) in the hand. 
             * The new card is face up, the replaced card is added to
             * the top of the discard pile(face up).
            */

            //get the card position first
            cardPosition = player.ChooseReplace(playerHand);

            //replace card at "cardPosition" with the card that was drawn
            //store the card that was replaced
            discardedCard = playerHand.Replace(cardPosition, drawnCard);

            //Set the replaced card face up into the discard pile
            if (discardedCard.FaceUp == false)
            {
                discardedCard.FaceUp = true;
                discardPile.Add(discardedCard);
            }
            else
                discardPile.Add(discardedCard);

            return true;
        }
    }

    //======================================PLAYER CLASS=============================================//
    public class Player
    {
        Random rnd = new Random();

        public Player()
        {
            
        }

        //Chooses to either draw from the deck (true)
        //or draw from discard pile (false)
        public bool ChooseDrawDiscard()
        {
            int probability = rnd.Next(0, 100);

            if (probability <= 75)
            {
                Console.WriteLine("Choosing from main deck");
                return true;
            }
            else
            {
                Console.WriteLine("Choosing from discard");
                return false;
            }
        }


        public int ChooseReplace(Hand playerHand)
        {
            //keeps track of whether to choose a faceup or facedown
            bool cardFace = ChooseFaceUpOrDown();

            int cardPosition = ChooseCard(cardFace, playerHand);

            return cardPosition;

        }


        public bool ChooseFaceUpOrDown()
        {
            int probability = rnd.Next(0, 100);

            //true for face up
            //false for face down
            bool face = false;

            //Choose a facedown or faceup card, with a bias
            //to choose a facedown card.
            if (probability <= 70)
                return face;
            else
                face = true;

            return face;
        }

        
        public int ChooseCard(bool cardFace, Hand playerHand)
        {
            int cardPosition = rnd.Next(0, 9);
            Card c;
            //search through the hand for a card that is either facedown or faceup,
            //depending on the value of "cardFace"


            while (true)
            {
                c = playerHand.Peek(cardPosition);
                //look at a random position in the hand
                //If the boolean FaceUp value of the card in the hand matches the 
                //desired card value(faceup or facedown)
                //then we just return the position
                
                if (c.FaceUp == cardFace)
                {
                    return cardPosition;
                }
                else
                    //If the FaceUp value of the card does not match
                    //the desired value (randomized), then just assign another random
                    //card position
                    cardPosition = rnd.Next(0, 9);
            }

        }

    }

    //=========================================HAND CLASS==============================================//
    public class Hand
    {
        private List<Card> hand;

        public Hand()
        {
            hand = new List<Card>();

        }

        //add a card to the hand
        public void Add(Card c)
        {
            //(Note: This add method is from the List class)
            hand.Add(c);
        }

        public Card Peek(int position)
        {
            return hand[position];
        }

        public void TurnUp(int position)
        {
            //TODO: Logic to turn card face up?
            hand[position].FaceUp = true;
        }

        public bool IsFaceUp(int position)
        {
            if (hand[position].FaceUp == true)
                return true;
            else
                return false;
        }

        public Card Replace(int position, Card nextCard)
        {
            Card card_out = hand[position];
            Card card_in = nextCard;

            card_in.FaceUp = true;
            hand[position] = card_in;

            return card_out;
        }

        public bool IsDone()
        {
            //Go through all cards
            //If they're all faceup, return true
            for (int i = 0; i < 9; i++)
            {
                if (hand[i].FaceUp == false)
                    return false;
            }
            return true;
        }

        public int Score()
        {
            //loop through the hand
            //Check for duplicates on rows
            for (int j = 0; j < 7; j += 3)
            {
                
                //if all 3 cards are equal on each row, switch their
                //suit value to Kings.  Will loop again through the whole hand
                //to check for Kings and when calculating the score
                if (hand[j] == hand[j + 1] && hand[j + 1] == hand[j + 2])
                {
                    hand[j].Suit = 'K';
                    hand[j + 1].Suit = 'K';
                    hand[j + 2].Suit = 'K';
                }
            }

            //Check for duplicates on columns
            for (int j = 0; j < 3; j++)
            {
                if (hand[j] == hand[j + 3] && hand[j + 3] == hand[j + 6])
                {
                    hand[j].Suit = 'K';
                    hand[j + 3].Suit = 'K';
                    hand[j + 6].Suit = 'K';
                }
            }

            //Check for duplicates on row
            for (int i = 0; i < 3; i++)
            {
                //When i = 0 or i = 2, check diagonally for duplicates
                if (i == 0)
                {
                    if (hand[i] == hand[i + 4] && hand[i + 4] == hand[i + 8])
                    {
                        hand[i].Suit = 'K';
                        hand[i + 4].Suit = 'K';
                        hand[i + 8].Suit = 'K';
                    }
                }
                else if(i == 2)
                {
                    if (hand[i] == hand[i + 2] && hand[i + 2] == hand[i + 4])
                    {
                        hand[i].Suit = 'K';
                        hand[i + 2].Suit = 'K';
                        hand[i + 4].Suit = 'K';
                    }
                }
            }

            //Now loop through the hand to calculate score.
            //Ignore all Kings.
            //Jokers subtract 2 from score
            //J and Q are worth 10
            //All else are worth their face value
            int score = 0;
            for (int i = 0; i < 9; i++)
            {
                if (hand[i].Value > 1 && hand[i].Value < 11)
                {
                    score += hand[i].Value;
                }
                else if (hand[i].Value > 10 && hand[i].Value < 13)
                {
                    score += 10;
                }
                else if (hand[i].Suit == 'J')
                {
                    score -= 2;
                }
            }

            return score;
        }

    }

    //=========================================CARD CLASS==============================================//
    public class Card
    {
        public Card(int v, Char suit)
        {
            Value = v;
            Suit = suit;
            FaceUp = false;
        }
        public int Value { get; set; }
        public char Suit { get; set; }
        public bool FaceUp { get; set; }
    }

    //=========================================DECK CLASS==============================================//
    public class Deck
    {
        Random rnd = new Random();

        const int MIN_VALUE = 0;
        const int MAX_VALUE = 13;

        static List<char> SUITS = new List<char>() { 'S', 'H', 'D', 'C' };
        private List<Card> deck = new List<Card>();

        public Deck()
        {
            //Jokers
            deck.Add(new Card(0, 'J'));
            deck.Add(new Card(0, 'J'));

            foreach (char suit in SUITS)
            {
                //Ace card first
                deck.Add(new Card(1, 'A'));
                for (int i = MAX_VALUE; i > MIN_VALUE; i--)
                {
                    deck.Add(new Card(i, suit));
                }
            }
        }

        public void Shuffle()
        {
            for( int i = 0; i < deck.Count; i++)
            {
                int swap = rnd.Next(53);

                Card temp = deck[i];
                deck[i] = deck[swap];
                deck[swap] = temp;
            }
        }

        public Card Draw()
        {
            Card removedCard = deck[0];
            deck.RemoveAt(0);
            return removedCard;
        }

        public void Add(Card c)
        {
            deck.Add(c);
        }

        public Card Peek(int position)
        {
            return deck[position];
        }

        public bool IsEmpty()
        {
            if (deck.Count == 0)
                return true;
            else
                return false;
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            //Tests
            Game newGame = new Game();

            newGame.Play();

        }
    }
}
