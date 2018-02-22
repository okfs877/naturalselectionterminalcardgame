using System;
using System.Collections.Generic;

namespace NaturalSelection
{
    public class Card
    {
        public string name { get; set; }
        public int attack { get; set; }
        public int baseAttack { get; set; }
        public int health { get; set; }
        public int baseHealth  { get; set; }
        public string rarity { get; set; }
        public List<string> abilities { get; set; }
        public List<string> types { get; set; }
        public bool drawnThisTurn { get; set; }

        public Card(string name, int attack, int health, string rarity)
        {
            this.name = name;
            this.attack = attack;
            this.baseAttack = attack;
            this.health = health;
            this.baseHealth = health;
            this.rarity = rarity;
            this.abilities = new List<string>();
            this.types = new List<string>();
            this.drawnThisTurn = false;
        }

        public Card display(int n)
        {
            string abilities2 = "";
            for (int i = 0; i < abilities.Count; i++)
            {
                if (i != abilities.Count-1)
                {
                    abilities2 += abilities[i] + ", ";
                }
                else abilities2 += abilities[i];
            }
            if (abilities2==""){
                System.Console.WriteLine($"{n}: {name} - Attack: {attack} Health: {health} Abilities: None");
            } else {
                System.Console.WriteLine($"{n}: {name} - Attack: {attack} Health: {health} Abilities: {abilities2}");
            }
            return this;
        }

        public int getAttack(Card opponent, Player player)
        {
            if (this.checkTypes("Carnivore") && opponent.checkTypes("Herbivore")) 
            {
                this.attack += 2;
            }
            if (this.checkTypes("Herbivore") && opponent.checkTypes("Plant"))
            {
                this.attack += 2;
            }
            if (this.checkTypes("Plant") && opponent.checkTypes("Carnivore"))
            {
                this.attack += 2;
            }
            if (opponent.checkAbilities("Itchy") && !(this.checkTypes("Plant")))
            {
                this.attack += 2;
            }
            if (this.checkAbilities("Fed by Death"))
            {
                this.attack += player.checkGraveyardTypes("Herbivore");
                this.attack += player.checkGraveyardTypes("Carnivore");
            }
            if (this.checkAbilities("Fed by Dirt"))
            {
                this.attack += player.checkGraveyardTypes("Plant");
            }
            if (this.checkAbilities("Morbid Growth"))
            {
                this.attack += player.checkGraveyardTypes("Herbivore");
                this.attack += player.checkGraveyardTypes("Carnivore");
                this.attack += player.checkGraveyardTypes("Scavenger");
            }
            if (this.checkAbilities("Charge") && this.drawnThisTurn)
            {
                this.attack += 2;
            }
            if (this.checkAbilities("Aquaticide") && opponent.checkAbilities("Aquatic"))
            {
                this.attack += 2;
            }
            if (this.checkAbilities("Herbicide") && opponent.checkTypes("Plant"))
            {
                this.attack += 2;
            }
            if (this.checkAbilities("Strength in Numbers"))
            {
                this.attack += player.checkGraveyard(this.name);
            }
            if (this.checkAbilities("Flying") && !(opponent.checkAbilities("Flying")))
                this.attack += 1;
            if (this.checkAbilities("High Flying") && !(opponent.checkAbilities("High Flying")))
                this.attack += 3;
            if (this.checkAbilities("Insecticide") && opponent.checkTypes("Insect"))
            {
                this.attack += 2;
            }
            if (this.checkAbilities("Trample") && opponent.rarity == "Uncommon")
            {
                this.attack += 2;
            }
            if (this.checkAbilities("Trample") && opponent.rarity == "Common")
                this.attack += 4;
            if (this.checkAbilities("Fire Breathing") && opponent.checkTypes("Plant"))
            {
                this.attack += 2;
            }
            if (this.checkAbilities("Poisonous") && this.attack > 0)
            {
                this.attack += 1000;
            }
            int attackSum = this.attack;
            this.attack = this.baseAttack;
            return attackSum;
        }

        public int getHealth(Card opponent, Player player)
        {
            if (this.checkAbilities("Aquatic") && !(opponent.checkAbilities("Aquatic")))
            {
                this.health += 1;
            }
            if (this.checkTypes("Carnivore") && opponent.checkTypes("Herbivore"))
            {
                this.health += 1;
            }
            
            if (this.checkTypes("Herbivore") && opponent.checkTypes("Plant"))
            {
                this.health += 1;
            }
            if (this.checkTypes("Plant") && opponent.checkTypes("Carnivore"))
            {
                this.health += 1;
            }
            if (this.checkAbilities("Morbid Growth"))
            {
                this.health += player.checkGraveyardTypes("Herbivore");
                this.health += player.checkGraveyardTypes("Carnivore");
                this.health += player.checkGraveyardTypes("Scavenger");
            }
            int healthSum = this.health;
            this.health = this.baseHealth;
            return healthSum;
        }
        public Card onPlay(Player player, Player opponentPlayer)
        {
            Random rand = new Random();
            if (this.checkAbilities("Delicious") && player.discard.Count > 0)
            {
                int randomInt = rand.Next(0,player.discard.Count);
                player.hand.Add(player.discard[randomInt]);
                player.discard.RemoveAt(randomInt);
            }
            if (this.checkAbilities("Bountiful Harvest"))
            {
                player.drawACard();
            }
            if (this.checkAbilities("Mind Rot") && opponentPlayer.hand.Count > 0)
            {
                int randomInt2 = rand.Next(0,opponentPlayer.hand.Count);
                opponentPlayer.discard.Add(opponentPlayer.hand[randomInt2]);
                opponentPlayer.hand.RemoveAt(randomInt2);
            }
            return this;
        }
        public bool checkAbilities(string ability)
        {
            for (int i = 0; i < this.abilities.Count; i++)
            {
                if (this.abilities[i] == ability)
                {
                    return true;
                }
            }
            return false;
        }
        public bool checkTypes(string kind)
        {
            if (this.abilities.Count==0){
                return false;
            }
            for (int i = 0; i < this.abilities.Count; i++)
            {
                if (this.types[i] == kind)
                {
                    return true;
                }
            }
            return false;
        }
        public Card printAbilities()
        {
            string abilityText = "";
            if (this.checkAbilities("Fire Breathing"))
            {
                abilityText += "This card gets +2 attack vs Plant cards\n";
            }
            if (this.checkAbilities("Stealth"))
            {
                abilityText += "This card attacks first vs non-Stealth cards\n";
            }
            if (this.checkAbilities("Charge"))
            {
                abilityText += "This card get +2 attack if it is played the turn it is drawn\n";
            }
            if (this.checkAbilities("Aquatic"))
            {
                abilityText += "This card gets +1 health vs non-Aquatic cards\n";
            }
            if (this.checkAbilities("Poisonous"))
            {
                abilityText += "This card kills opponents it damages\n";
            }
            if (this.checkAbilities("Strength in Numbers"))
            {
                abilityText += "This card gets +1 attack and +1 health for each other copy of this card played before\n";
            }
                if (this.checkAbilities("Flying"))
            {
                abilityText += "This card gets +1 attack against non-Flying cards\n";
            }
            if (this.checkAbilities("Quick Feet"))
            {
                abilityText += "This card causes fights to end in a draw if the opponent has 5 or more attack\n";
            }
            if (this.checkAbilities("Aquaticide"))
            {
                abilityText += "This card gets +2 attack vs Aquatic cards\n";
            }
            if (this.checkAbilities("Trample"))
            {
                abilityText += "This card gets +2 attack vs Uncommon cards and +4 attack vs Common cards\n";
            }
            if (this.checkAbilities("Delicious"))
            {
                abilityText += "When you play this card you draw a random card from your discard\n";
            }
            if (this.checkAbilities("Herbicide"))
            {
                abilityText += "This card gets +2 attack vs Plant cards\n";
            }
            if (this.checkAbilities("Second Hand"))
            {
                abilityText += "This card wins all draws\n";
            }
            if (this.checkAbilities("Itchy"))
            {
                abilityText += "The opponents attack is reduced by 2 if they aren't a Plant\n";
            }
            if (this.checkAbilities("Morbid Growth"))
            {
                abilityText += "This card gets +2 attack and +2 health for each herbiroe, carnivore, and scavenger in the discard\n";
            }
            if (this.checkAbilities("Insecticide"))
            {
                abilityText += "This card gains +2 attack vs Insects\n";
            }
            if (this.checkAbilities("Bountiful Harvest"))
            {
                abilityText += "When you play this card you draw a card\n";
            }
            if (this.checkAbilities("Mind Rot"))
            {
                abilityText += "When you play this card your opponent discards a card at random\n";
            }
            if (this.checkAbilities("Haymaker"))
            {
                abilityText += "The points won by the winner is doubled\n";
            }
            if (this.checkAbilities("High Flying"))
            {
                abilityText += "This card gets +3 attack vs non-High Flying cards\n";
            }
            if (this.checkAbilities("Fed by Death"))
            {
                abilityText += "This card gains +1 attack and +1 health for each Herbivore and Carnivore card in your discard";
            }
            if (this.checkAbilities("Fed by Dirt"))
            {
                abilityText += "This card gets +1 attack and +1 health for each Plant card in your discard";
            }
            System.Console.WriteLine(abilityText);
            return this;
        }
    }
    public class Carnivore : Card
    {
        public Carnivore(string name, int attack, int health, string rarity) : base(name, attack, health, rarity)
        {
            this.types.Add("Carnivore");
        }
    }
    public class Dragon : Carnivore
    {
        public Dragon() : base("Dragon", 7, 8, "Rare")
        {
            this.abilities.Add("Fire Breathing");
        }
    }
    public class Wolverine : Carnivore
    {
        public Wolverine() : base("Wolverine", 6, 2, "Rare")
        {
            this.abilities.Add("Stealth");
        }
    }
    public class SabertoothTiger : Carnivore
    {
        public SabertoothTiger() : base("Sabertooth Tiger", 5, 6, "Rare")
        {
            this.abilities.Add("Charge");
        }
    }
    public class Orca : Carnivore
    {
        public Orca() : base("Orca", 6, 7, "Rare")
        {
            this.abilities.Add("Aquatic");
        }
    }
    public class Olinguito : Carnivore
    {
        public Olinguito() : base("Olinguito", 3, 5, "Uncommon")
        {
            this.abilities.Add("Stealth");
        }
    }
    public class CoralSnake : Carnivore
    {
        public CoralSnake() : base("Coral Snake", 2, 3, "Uncommon")
        {
            this.abilities.Add("Poisonous");
        }
    }
    public class Wolf : Carnivore
    {
        public Wolf() : base("Wolf", 2, 2, "Common")
        {
            this.abilities.Add("Strength in Numbers");
        }
    }
    public class Owl : Carnivore
    {
        public Owl() : base("Owl", 1, 2, "Common")
        {
            this.abilities.Add("Flying");
        }
    }
    public class Cougar : Carnivore
    {
        public Cougar() : base("Cougar", 2, 2, "Common")
        {
            this.abilities.Add("Charge");
        }
    }
    public class Fox : Carnivore
    {
        public Fox() : base("Fox", 3, 1, "Common")
        {
            this.abilities.Add("Quick Feet");
        }
    }
    public class KingFisher : Carnivore
    {
        public KingFisher() : base("King Fisher", 2, 1, "Common")
        {
            this.abilities.Add("Flying");
            this.abilities.Add("Stealth");
            this.abilities.Add("Aquaticide");
        }
    }

    public class Herbivore : Card
    {
        public Herbivore(string name, int attack, int health, string rarity) : base(name, attack, health, rarity)
        {
            this.types.Add("Herbivore");
        }
    }
    public class Bison : Herbivore
    {
        public Bison() : base("Bison", 6, 7, "Rare")
        {
            this.abilities.Add("Charge");
        }
    }
    public class DropBear : Herbivore
    {
        public DropBear() : base("Drop Bear", 5, 1, "Rare")
        {
            this.abilities.Add("Stealth");
        }
    }
    public class Elephant : Herbivore
    {
        public Elephant() : base("Elephant", 3, 7, "Rare")
        {
            this.abilities.Add("Trample");
        }
    }
    public class Panda : Herbivore
    {
        public Panda() : base("Panda", 3, 4, "Uncommon")
        {
            this.abilities.Add("Panda Swap");
        }
    }
    public class Rhino : Herbivore
    {
        public Rhino() : base("Rhino", 5, 6, "Uncommon")
        {
            this.abilities.Add("Charge");
        }
    }
    public class Moose : Herbivore
    {
        public Moose() : base("Moose", 4, 4, "Uncommon")
        {
            this.abilities.Add("Charge");
        }
    }
    public class Rabbit : Herbivore
    {
        public Rabbit() : base("Rabbit", 2, 2, "Common")
        {
            this.abilities.Add("Quick Feet");
        }
    }
    public class Cow : Herbivore
    {
        public Cow() : base("Cow", 1, 5, "Common")
        {
            this.abilities.Add("Delicious");
        }
    }
    public class Kangaroo : Herbivore
    {
        public Kangaroo() : base("Kangaroo", 2, 3, "Common")
        {
            this.abilities.Add("See Description");
        }
        public new int getAttack(Card opponent, Player player)
        {
            if (opponent.checkTypes("Herbivore"))
            {
                return base.getAttack(opponent, player) + 1;
            }
            else return base.getAttack(opponent, player);
        }
        public new int getHealth(Card opponent, Player player)
        {
            if (opponent.checkTypes("Herbivore"))
            {
                return base.getHealth(opponent, player) + 1;
            }
            else return base.getHealth(opponent, player);
        }
        public new Card printAbilities()
        {
            System.Console.WriteLine("This card get +1 attack and +1 health vs Herbivore cards");
            return this;
        }
    }
    public class Squirrel : Herbivore
    {
        public Squirrel() : base("Squirrel", 1, 1, "Common")
        {
            this.abilities.Add("Strength in Numbers");
        }
    }
    public class Locusts : Herbivore
    {
        public Locusts() : base("Locusts", 1, 2, "Common")
        {
            this.abilities.Add("Herbicide");
            this.abilities.Add("Flying");
            this.types.Add("Insect");
        }
    }
    public class Plant: Card{
        public Plant(string name, int attack, int health, string rarity):base(name, attack, health, rarity){
            this.types.Add("Plant");
        }
    }
    public class TobaccoPlant: Plant{
        public TobaccoPlant():base("Tobacco Plant", 5, 5, "Rare"){
            this.abilities.Add("Second Hand");
        }
    }
    public class PoisonIvy: Plant{
        public PoisonIvy():base("Poison Ivy", 3, 5, "Rare"){
            this.abilities.Add("Itchy");
        }
    }
    public class CorpseFlower:Plant{
        public CorpseFlower():base("Corpse Flower", 4, 2, "Rare"){
            this.abilities.Add("Morbid Growth");
        }
    }
    public class AcaciaTree:Plant{
        public AcaciaTree():base("Acacia Tree", 3, 6, "Rare"){
            this.abilities.Add("Herbicide");
        }
        public new int getAttack(Card opponent, Player player){
            if (opponent.checkTypes("Plant")){
                return base.getAttack(opponent, player) + 2;
            } else return base.getAttack(opponent, player);
        }
    }
    public class VenusFlyTrap: Plant{
        public VenusFlyTrap():base("Venus Fly Trap", 4, 4, "Uncommon"){
            this.abilities.Add("Insecticide");
        }
    }
    public class Oleander:Plant{
        public Oleander():base("Oleander", 3, 2, "Uncommon"){
            this.abilities.Add("Mind Rot");
        }
    }
    public class Hogweed:Plant{
        public Hogweed():base("Hogweed", 2, 4, "Uncommon"){
            this.abilities.Add("See Description");
        }
        public new Card printAbilities(){
            System.Console.WriteLine("This card gets +4 attack vs Herbivore and Carnivore cards");
            return this;
        }
        public new int getAttack(Card opponent, Player player){
            if(opponent.checkTypes("Herbivore") || opponent.checkTypes("Carnivore")){
                return base.getAttack(opponent, player) + 4;
            } else return base.getAttack(opponent, player);
        }
    }
        public class DeadlyNightshade:Plant{
            public DeadlyNightshade():base("Deadly Nightshade", 1, 2, "Common"){
                this.abilities.Add("Poisonous");
            }
        }
        public class Blackberry:Plant{
            public Blackberry():base("Blackberry", 2, 4, "Common"){
                this.abilities.Add("Delicious");
            }
        }
        public class Corn:Plant{
            public Corn():base("Corn", 1, 2, "Common"){
                this.abilities.Add("Bountiful Harvest");
            }
        }
        public class CreepingThistle:Plant{
            public CreepingThistle():base("Creeping Thistle", 2, 3, "Common"){
                this.abilities.Add("See Description");
            }
            public new Card printAbilities(){
                System.Console.WriteLine("This card gets +3 attack vs Herbivore cards");
                return this;
            }
            public new int getAttack(Card opponent, Player player){
                if (opponent.checkTypes("Herbivore")){
                    return base.getAttack(opponent, player) + 3;
                } else return base.getAttack(opponent, player);
            }
        }
        public class Bindweed:Plant{
            public Bindweed():base("Bindweed", 1, 5, "Common"){
            }
            public new int getAttack(Card opponent, Player player){
                if (opponent.checkTypes("Plant")){
                    return base.getAttack(opponent, player) +2;
                } else return base.getAttack(opponent, player);
            }
        }
        public class AppleTree:Plant{
            public AppleTree():base("Apple Tree", 0, 5, "Common"){
                this.abilities.Add("Delicious");
            }
        }
        public class Scavenger:Card{
            public Scavenger(string name, int attack, int health, string rarity):base(name, attack, health, rarity){
                this.types.Add("Scavenger");
            }
        }
        public class GiantKillerMushroom:Scavenger{
            public GiantKillerMushroom():base("Giant Killer Mushroom", 8, 7, "Rare"){
                this.abilities.Add("Haymaker");
            }
        }
        public class OphiocordycepsUnilateralis:Scavenger{
            public OphiocordycepsUnilateralis():base("Ophiocordyceps Unilateralis", 3, 6, "Rare"){
                this.abilities.Add("Insecticide");
            }
        }
        public class Condor:Scavenger{
            public Condor():base("Condor", 2, 6, "Rare"){
                this.abilities.Add("High Flying");
            }
        }
        public class MaggotPile:Scavenger{
            public MaggotPile():base("Maggot Pile", 1, 1, "Uncommon"){
                this.abilities.Add("Fed by Death");
                this.types.Add("Insect");
            }
        }
        public class BlueButterfly:Scavenger{
            public BlueButterfly():base("Blue Butterfly", 3, 2, "Uncommon"){
                this.abilities.Add("Quick Feet");
                this.abilities.Add("Flying");
                this.types.Add("Insect");
            }
        }
        public class AminitaMuscaria:Scavenger{
            public AminitaMuscaria():base("Aminita Muscaria", 2, 3, "Uncommon"){
                this.abilities.Add("See Description");
            }
            public new int getAttack(Card opponent, Player player){
            if(opponent.checkTypes("Plant")){
                return base.getAttack(opponent, player) + 2;
            } else if (opponent.checkTypes("Carnivore") || opponent.checkTypes("Herbivore")){
                return base.getAttack(opponent, player) +1;
            } else {return base.getAttack(opponent, player);
            }
        }
        public new Card printAbilities(){
            System.Console.WriteLine("This card gets +2 attack vs Plants and +1 attack vs Herbivores and Carnivores");
            return this;
        }
    }
    public class Eagle:Scavenger{
        public Eagle():base("Eagle", 4, 3, "Uncommon"){
            this.abilities.Add("Flying");
        }
    }
    public class Earthworm:Scavenger{
        public Earthworm():base("Earthworm", 2, 2, "Common"){
            this.abilities.Add("Fed by Dirt");
            this.types.Add("Insect");
        }
    }
    public class Moth:Scavenger{
        public Moth():base("Moth", 1, 1, "Common"){
            this.abilities.Add("Flying");
            this.types.Add("Insect");
        }
    }
    public class Seastar:Scavenger{
        public Seastar():base("Seastar", 2, 2, "Common"){
            this.abilities.Add("Aquatic");
        }
    }
    public class BlueOysterMushroom:Scavenger{
        public BlueOysterMushroom():base("Blue Oyster Mushroom", 2, 3, "Common"){
            this.abilities.Add("Herbicide");
        }
    }
    public class EnokiMushroom:Scavenger{
        public EnokiMushroom():base("Enoki Mushroom", 2, 2, "Common"){
            this.abilities.Add("Herbicide");
        }
    }
    public class Crab:Scavenger{
        public Crab():base("Crab", 2, 3, "Common"){
            this.abilities.Add("Aquatic");
        }
    }
    public class Bloatfly:Scavenger{
        public Bloatfly():base("Bloatfly", 1, 1, "Common"){
            this.abilities.Add("Flying");
            this.types.Add("Insect");
        }
    }
}