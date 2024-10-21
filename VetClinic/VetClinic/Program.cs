using VetClinic;

Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), new List<Color> { Color.Black, Color.White });
Fish fish1 = new Fish("Goldie", Sex.Female, 0.3, new DateTime(2021, 6, 15), new List<Color> { Color.Golden }, WaterType.Freshwater, 24.0);
Mammal mammal1 = new Mammal("Burek", Sex.Male, 12.5, new DateTime(2018, 3, 22), new List<Color> { Color.Brown }, true);
Bird bird1 = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2020, 4, 10), new List<Color> { Color.Yellow }, 0.25, true);
Client cl1=new Client("Marek","Kowalski","081821727","Marek@gmail.com");

Pet.PrintExtent();
Fish.PrintExtent();
Mammal.PrintExtent();
Bird.PrintExtent();
Client.PrintExtent();

