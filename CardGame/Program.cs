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
        private Hand p1_hand;
        private Hand p2_hand;
        int NUM_CARDS;

        Card cardTest;
        //declare player variables

        ////Grid
        //for (int i = 0; i< 9; i++)
        //{
        //	if (i == 3 || i == 6)
        //	{
        //		Console.Write("\n");
        //	}
        //             p1_hand.Add(mainDeck.Draw());
        //	Console.Write("{0}{1}  ", p1_hand.Peek(i).Suit, p1_hand.Peek(i).Value);
        //}

        //            Console.WriteLine("\n");


        //for (int i = 0; i< 9; i++)
        //{
        //	if (i == 3 || i == 6)
        //	{
        //		Console.Write("\n");
        //	}
        //	p2_hand.Add(mainDeck.Draw());
        //	Console.Write("{0}{1}  ", p2_hand.Peek(i).Suit, p2_hand.Peek(i).Value);
        //}


        public Game()
        {
            

        }

        public void Setup()
        {
            //Setup all objects here
            mainDeck = new Deck();
            p1_hand = new Hand();
            p2_hand = new Hand();
            //set up players

            mainDeck.Shuffle();

            Deal();

            cardTest = mainDeck.Draw();

            //test Replace
            p1_hand.Replace(5, cardTest);

            Console.WriteLine("{0}{1}", cardTest.Suit, cardTest.Value);
            Console.WriteLine("{0}{1}", p1_hand.Peek(5).Suit, p1_hand.Peek(5).Value);

        }

        private void Deal()
        {
            NUM_CARDS = 9;
            //for num cards
            for (int i = 0; i < NUM_CARDS; i++)
            {
                //for each hand
                for (int j = 0; j < 2; j++)
                {
                    //draw from deck
                    Card next = mainDeck.Draw();
                    //add to hand
                    p1_hand.Add(next);
                    next = mainDeck.Draw();
                    p2_hand.Add(next);
                }
            }
        }

        public bool Turn()
        {
            //get the right player
            //player:: choose card
            //get card from hand
            //player::choose suit
            //compare


            return true;
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
                //deck.Add(new Card(1, suit));
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

            newGame.Setup();

        }
    }
}
