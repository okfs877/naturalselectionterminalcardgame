using System;
using System.Collections.Generic;

namespace NaturalSelection
{
    class Terminal
    {
        public Player player {get;set;}
        public Player aiPlayer {get;set;}
        public Card playedCard {get;set;}
        public Card aiCardHolder {get;set;}
        public Terminal(Player player){
            this.player = player;
            this.aiPlayer = new Player("ai");
            for (int i = 0; i < 5; i++)
            {
                this.player.silentDraw();
                this.aiPlayer.silentDraw();
            }
            this.playedCard = null;
            this.aiCardHolder = null;
        }
        public Terminal cardFight(){
            int scoreChange = 1;
            Card aiCard = this.aiPlayer.hand[0].onPlay(this.aiPlayer, this.player);
            this.aiPlayer.hand.RemoveAt(0);
            this.aiCardHolder = aiCard;
            System.Console.WriteLine($"You played {this.playedCard.name} and your opponent played {aiCard.name}");
            if (this.playedCard.checkAbilities("Haymaker")){
                scoreChange *= 2;
            }
            if (aiCard.checkAbilities("Haymaker")){
                scoreChange *= 2;
            }
            if(this.playedCard.checkAbilities("Stealth") && !(aiCard.checkAbilities("Stealth"))){
                aiCard.health = aiCard.getHealth(this.playedCard, this.aiPlayer) - this.playedCard.getAttack(aiCard, this.player);
                if (aiCard.health <= 0){
                    this.player.score += scoreChange;
                    System.Console.WriteLine($"You won and gained {scoreChange} point(s)");
                    return this;
                } else {
                    this.playedCard.health = this.playedCard.getHealth(aiCard, this.player) - aiCard.getAttack(this.playedCard, this.aiPlayer);
                }
            } else if (aiCard.checkAbilities("Stealth") && !(this.playedCard.checkAbilities("Stealth"))){
                this.playedCard.health = this.playedCard.getHealth(aiCard, this.player) - aiCard.getAttack(this.playedCard, this.aiPlayer);
                if (this.playedCard.health <= 0){
                    this.aiPlayer.score += scoreChange;
                    System.Console.WriteLine($"You lost and your opponent gained {scoreChange} point(s)");
                    return this;
                } else {
                    aiCard.health = aiCard.getHealth(this.playedCard, this.aiPlayer) - this.playedCard.getAttack(aiCard, this.player);
                }
            } else {
                aiCard.health = aiCard.getHealth(this.playedCard, this.aiPlayer) - this.playedCard.getAttack(aiCard, this.player);
                this.playedCard.health = this.playedCard.getHealth(aiCard, this.player) - aiCard.getAttack(this.playedCard, this.aiPlayer);
            }
            if (this.playedCard.checkAbilities("Quick Feet") && aiCard.attack >= 5 && !(aiCard.checkAbilities("Poisonous"))){
                System.Console.WriteLine("The fight was a draw"); 
                return this;
            }
            if (aiCard.checkAbilities("Quick Feet") && this.playedCard.attack >= 5 && !(this.playedCard.checkAbilities("Poisonous"))){
                System.Console.WriteLine("The fight was a draw"); 
                return this;
            }
            if (this.playedCard.health > 0 && aiCard.health <= 0){
                this.player.score += scoreChange;
                System.Console.WriteLine($"You won and gained {scoreChange} point(s)");
                return this;
            } else if (this.playedCard.health <= 0 && aiCard.health > 0){
                this.aiPlayer.score += scoreChange;
                System.Console.WriteLine($"You lost and your opponent gained {scoreChange} point(s)");
                return this;
            } else if (this.playedCard.checkAbilities("Second Hand") && !(aiCard.checkAbilities("Second Hand"))){
                this.player.score += scoreChange;
                System.Console.WriteLine($"You won and gained {scoreChange} point(s)");
                return this;
            } else if (aiCard.checkAbilities("Second Hand") && !(this.playedCard.checkAbilities("Second Hand"))){
                this.aiPlayer.score += scoreChange;
                System.Console.WriteLine($"You lost and your opponent gained {scoreChange} point");
                return this;
            }
            System.Console.WriteLine("The fight was a draw");
            return this;
        }
        public Terminal playACard(){
            bool legalCard = false;
            while(!legalCard){
                System.Console.WriteLine("This is your hand: ");
                this.player.printHand();
                System.Console.WriteLine("Play a card by typing it's number, or type abilities and a number to find out more about that cards abilities: ");
                string playedCardNum = Console.ReadLine();
                if (playedCardNum.Length >9){
                    if(playedCardNum.Substring(0,9) == "abilities"){
                        if ((Int32.TryParse(playedCardNum.Substring(playedCardNum.Length-1, 1), out int abilNum))){
                            abilNum = Int32.Parse(playedCardNum.Substring(playedCardNum.Length-1, 1));
                            if (abilNum < this.player.hand.Count){
                                this.player.hand[abilNum].printAbilities();
                                continue;
                            }
                        }
                    }
                }
                if (Int32.TryParse(playedCardNum, out int num)){
                    if (num < this.player.hand.Count){
                        legalCard = true;
                        this.playedCard = this.player.hand[num].onPlay(this.player, this.aiPlayer);
                        this.player.hand.RemoveAt(num);
                    }
                }
            }
            return this;
        }
        public Terminal discardUsedCards(Card aiCard){
            this.player.discard.Add(this.playedCard);
            this.aiPlayer.discard.Add(aiCard);
            return this;
        }
        public Terminal resetGame(){
            System.Console.WriteLine("Welcome to Natural Selection");
            System.Console.WriteLine("What is your name? ");
            string name = Console.ReadLine();
            Terminal newGame = new Terminal(new Player(name));
            int cardsPlayed = 0;
            string txtInput = "";
            while (txtInput != "begin"){
                System.Console.WriteLine("type begin to start or rules for the games rules: ");
                txtInput = Console.ReadLine();
                if (txtInput=="rules"){
                    string rules = "The basics of Natural Selection are as follows: \n1. The player with the most round wins after 15 rounds wins \n";
                    rules += "2. Each game you and your opponent's deck are randomly generated \n3. Each round you choose a card to fight your opponent with \n";
                    rules += "4. The winner of each round is determined by the attack and health and abiliites and types of the cards played \n";
                    rules += "5. There are 4 main types of cards, Carnivores, Herbivores, Plants, and Scavengers. \n";
                    rules += "6. Carnivores are strong vs Herbivores, Herbivores are strong vs Plants and Plants are strong vs Carnivores. Scavengers are Neutral \n";
                    rules += "7. When cards fight they each lose health equal to the opponents attack.  If only one survives that is the winner, otherwise it is a draw";
                    System.Console.WriteLine(rules);
                }
            }
            while (cardsPlayed<15){
                newGame.player.drawACard();
                newGame.aiPlayer.silentDraw();
                newGame.playACard().cardFight().discardUsedCards(newGame.aiCardHolder);
                System.Console.WriteLine($"The current score is {newGame.player.name}: {newGame.player.score} to ai: {newGame.aiPlayer.score} and there are {14-cardsPlayed} rounds remaining");
                cardsPlayed++;
                for (int i = 0; i < newGame.player.hand.Count; i++){
                    newGame.player.hand[i].drawnThisTurn = false;
                }
                for (int j = 0; j < newGame.aiPlayer.hand.Count; j++){
                    newGame.aiPlayer.hand[j].drawnThisTurn = false;
                }
                System.Console.WriteLine("\n\n");
            }
            System.Console.WriteLine($"The final score was {newGame.player.name}: {newGame.player.score} to ai: {newGame.aiPlayer.score}");
            System.Console.WriteLine("Do you want to play again? Y/N ");
            string playagain =Console.ReadLine();
            if (playagain =="yes" || playagain == "Y" || playagain == "y"){
                newGame.resetGame();
            }
            else{
                System.Console.WriteLine("Thanks for playing");
            }
            return this;
        }
    }
}