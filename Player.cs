using System;
using System.Collections.Generic;

namespace NaturalSelection
{
    public class Player
    {
        public string name {get;set;}
        public List<Card> hand {get;set;}
        public Deck deck {get;set;}
        public List<Card> discard {get;set;}
        public int score {get;set;}

        public Player(string name){
            this.name = name;
            this.hand = new List<Card>();
            this.deck = new Deck();
            this.discard = new List<Card>();
            this.score = 0;
        }
        public Player drawACard(){
            if (this.hand.Count > 7){
                return this;
            }
            this.hand.Add(this.deck.contents[0]);
            this.deck.contents.RemoveAt(0);
            System.Console.WriteLine("You drew: ");
            this.hand[this.hand.Count-1].display(this.hand.Count-1);
            this.hand[this.hand.Count-1].drawnThisTurn = true;
            return this;
        }
        public Player silentDraw(){
            this.hand.Add(this.deck.contents[0]);
            this.deck.contents.RemoveAt(0);
            this.hand[this.hand.Count-1].drawnThisTurn = true;
            return this;
        }
        public Player printHand(){
            for (int i = 0; i < this.hand.Count; i++){
                this.hand[i].display(i);
            }
            return this;
        }
        public int checkGraveyard(string cardName){
            int count = 0;
            for (int i = 0; i < this.discard.Count; i++){
                if(this.discard[i].name == cardName){
                    count++;
                }
            }
            return count;
        }
        public int checkGraveyardTypes(string kind){
            int count = 0;
            for (int i = 0; i < this.discard.Count; i++){
                if(this.discard[i].checkTypes(kind)){
                    count++;
                }
            }
            return count;
        }
    }
}