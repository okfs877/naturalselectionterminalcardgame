using System;
using System.Collections.Generic;

namespace NaturalSelection
{
    public class Deck
    {
        public List<Card> contents {get;set;}
        private static Random rand = new Random();
        public Deck(){
            this.contents = new List<Card>();
            //Carnivores
            List<Card> carnivores = new List<Card>();
            //rares
            for (int i = 0; i < 1; i++)
            {
                carnivores.Add(new Dragon());
                carnivores.Add(new Wolverine());
                carnivores.Add(new SabertoothTiger());
                carnivores.Add(new Orca());
            }
            //uncommons
            for (int i = 0; i < 3; i++)
            {
                carnivores.Add(new Carnivore("Tiger", 5, 5, "Uncommon"));
                carnivores.Add(new Carnivore("lynx", 4, 4, "Uncommon"));
                carnivores.Add(new CoralSnake());
                carnivores.Add(new Olinguito());
            }
            //commons
            for (int i = 0; i < 9; i++)
            {
                carnivores.Add(new Carnivore("House Cat", 2, 2, "Common"));
                carnivores.Add(new Carnivore("Python", 2, 4, "Common"));
                carnivores.Add(new Carnivore("Ocelot", 3, 3, "Common"));
                carnivores.Add(new Wolf());
                carnivores.Add(new Owl());
                carnivores.Add(new Fox());
                carnivores.Add(new Cougar());
                carnivores.Add(new KingFisher());
            }
            //Herbivores
            List<Card> herbivores = new List<Card>();
            //rares
            for (int i = 0; i < 1; i++)
            {
                herbivores.Add(new Herbivore("Stegosaurus", 6, 9, "Rare"));
                herbivores.Add(new Bison());
                herbivores.Add(new DropBear());
                herbivores.Add(new Elephant());
            }
            //uncommons
            for (int i = 0; i < 3; i++)
            {
                herbivores.Add(new Herbivore("Koala", 3, 3, "Uncommon"));
                herbivores.Add(new Panda());
                herbivores.Add(new Rhino());
                herbivores.Add(new Moose());
            }
            //commons
            for (int i = 0; i < 9; i++)
            {
                herbivores.Add(new Herbivore("Alpacca", 2, 3, "Common"));
                herbivores.Add(new Herbivore("Hippy", 2, 2, "Common"));
                herbivores.Add(new Herbivore("Emu", 3, 4, "Common"));
                herbivores.Add(new Rabbit());
                herbivores.Add(new Cow());
                herbivores.Add(new Kangaroo());
                herbivores.Add(new Squirrel());
                herbivores.Add(new Locusts());
            }
            //Plants
            List<Card> plants = new List<Card>();
            //rares
            for (int i = 0; i < 1; i++)
            {
                plants.Add(new TobaccoPlant());
                plants.Add(new PoisonIvy());
                plants.Add(new CorpseFlower());
                plants.Add(new AcaciaTree());
            }
            //uncommons
            for (int i = 0; i < 3; i++)
            {
                plants.Add(new Plant("Saguaro Cactus", 1, 12, "Uncommon"));
                plants.Add(new VenusFlyTrap());
                plants.Add(new Oleander());
                plants.Add(new Hogweed());
            }
            //commons
            for (int i = 0; i < 9; i++)
            {
                plants.Add(new DeadlyNightshade());
                plants.Add(new Blackberry());
                plants.Add(new Corn());
                plants.Add(new CreepingThistle());
                plants.Add(new Bindweed());
                plants.Add(new AppleTree());
                plants.Add(new Plant("White Snakeroot", 2, 3, "Common"));
                plants.Add(new Plant("Rose", 3, 1, "Common"));
            }
            //Scavengers
            List<Card> scavs = new List<Card>();
            //rares
            for (int i = 0; i < 1; i++)
            {
                scavs.Add(new Scavenger("Mycena Interrupta", 6, 7, "Rare"));
                scavs.Add(new GiantKillerMushroom());
                scavs.Add(new OphiocordycepsUnilateralis());
                scavs.Add(new Condor());
            }
            //uncommons
            for (int i = 0; i < 3; i++)
            {
                scavs.Add(new MaggotPile());
                scavs.Add(new BlueButterfly());
                scavs.Add(new AminitaMuscaria());
                scavs.Add(new Eagle());
            }
            //commons
            for (int i = 0; i < 9; i++)
            {
                scavs.Add(new Earthworm());
                scavs.Add(new Moth());
                scavs.Add(new Seastar());
                scavs.Add(new BlueOysterMushroom());
                scavs.Add(new EnokiMushroom());
                scavs.Add(new Crab());
                scavs.Add(new Bloatfly());
                scavs.Add(new Scavenger("Slug", 1, 4, "Common"));
            }
            for (int i = 0; i < 60; i++)
            {
                int randtype = rand.Next(0,4);
                if(randtype==1){
                    int randCardofType = rand.Next(0,carnivores.Count-1);
                    this.contents.Add(carnivores[randCardofType]);
                    carnivores.RemoveAt(randCardofType);
                }
                if(randtype==1){
                    int randCardofType = rand.Next(0,herbivores.Count-1);
                    this.contents.Add(herbivores[randCardofType]);
                    herbivores.RemoveAt(randCardofType);
                }
                if(randtype==1){
                    int randCardofType = rand.Next(0,plants.Count-1);
                    this.contents.Add(plants[randCardofType]);
                    plants.RemoveAt(randCardofType);
                } else {
                    int randCardofType = rand.Next(0,scavs.Count-1);
                    this.contents.Add(scavs[randCardofType]);
                    scavs.RemoveAt(randCardofType);
                }
            }
            this.shuffle();
        }
        public Deck shuffle(){
            for (int i = 0; i < 1000; i++)
            {
                int pos1 = rand.Next(0, this.contents.Count);
                int pos2 = rand.Next(0, this.contents.Count);
                Card temp = this.contents[pos1];
                this.contents[pos1] = this.contents[pos2];
                this.contents[pos2] = temp;
            }
            return this;
        }
    }
}