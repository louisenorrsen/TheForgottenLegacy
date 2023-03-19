using static MUD.Rooms;
using System.Threading;
namespace MUD
{
    public class Program
    {
        public class Player
        {
            public int RoomPosition;
            public bool HasHouseKey, HarSuttitIFotöljen, HarInspekteratTavlan, HarTittatISpisen, HarHittatSafiren, HarTittatIKöket, HarÖppnatByrån, HarTittatPåBordet, HarTagitEnBrevkniv, HarVardagsrumsNyckel, HarSovrumsNyckel, HarLästBrevet, HarHittatBoken, HarHittatKassaskåpet, HarInspekteratSpegeln, HarÖppnatKassaskåpet;
            public Player(int roomPosition, bool hasHouseKey = true)
            {
                RoomPosition = roomPosition;
                HasHouseKey = hasHouseKey;
                HarSovrumsNyckel = false;
                HarVardagsrumsNyckel = false;
                HarSuttitIFotöljen = false;
                HarInspekteratTavlan = false;
                HarTittatISpisen = false;
                HarÖppnatByrån = false;
                HarTittatPåBordet = false;
                HarHittatSafiren = false;
                HarTittatIKöket = false;
                HarTagitEnBrevkniv = false;
                HarLästBrevet = false;
                HarHittatBoken = false;
                HarHittatKassaskåpet = false;
                HarInspekteratSpegeln = false;
                HarÖppnatKassaskåpet = false;
            }
        }
        public static Player player = new(0); // TBD: Ändra så Players finns i en lista av spelare. I början får man fylla i vad man heter och då läggs en ny spelare till.
        public class Room
        {
            public int Number, North, East, West, South;
            public string Name, Description;
            public bool RoomUnlocked, ChallengeDone;
            public Action<int> Challenge;
            public Room(int number, string name, string presentation, int north, int east, int south, int west, Action<int> challenge)
            {
                Number = number;
                Name = name;
                Description = presentation;
                North = north;
                East = east;
                South = south;
                West = west;
                RoomUnlocked = false;
                ChallengeDone = false;
                Challenge = challenge;
            }
            public void GoToNewRoom()
            {
                // PrintMap(Number); NYI: Karta när spelaren går in i ett nytt rum
                TypewriterBold(Name);
                Typewriter(Description);
                player.RoomPosition = Number;
                if (Challenge != null) Challenge(Number);
            }

            public void PrintActions()
            {
                Typewriter($"\nVad vill du göra?", 30);
                if (North != -1 && rooms[North].RoomUnlocked) Console.WriteLine("   n - gå norrut");
                if (East != -1 && rooms[East].RoomUnlocked) Console.WriteLine("   ö - gå österut");
                if (South != -1 && rooms[South].RoomUnlocked) Console.WriteLine("   s - gå söderut");
                if (West != -1 && rooms[West].RoomUnlocked) Console.WriteLine("   v - gå västerut");
            }
            public void PrintMap(int roomNumber)
            {
                switch (roomNumber) 
                { 
                    case 1: 
                        Console.WriteLine(" Karta  [X]      ");
                        break;
                    case 2:
                        Console.WriteLine("     [ ][ ]      ");
                        Console.WriteLine(" Karta  [ ][X]   ");
                        Console.WriteLine("        [ ]      ");
                        break;
                    case 3:
                        Console.WriteLine("        [X]      ");
                        Console.WriteLine(" Karta  [ ]      ");
                        Console.WriteLine("        [ ]      ");
                        break;
                    case 4:
                        Console.WriteLine("  Karta [X]      ");
                        Console.WriteLine("        [ ]      ");
                        break;
                    case 5:
                        Console.WriteLine("     [X][ ]      ");
                        Console.WriteLine(" Karta  [ ]      ");
                        Console.WriteLine("        [ ]      ");
                        break;
                    case 6:
                        Console.WriteLine("     [ ][ ]      ");
                        Console.WriteLine(" Karta  [ ][ ][X]");
                        Console.WriteLine("        [ ]      ");
                        break;
                    case 7:
                        Console.WriteLine("        [X]      ");
                        break;
                    case 8:
                        Console.WriteLine("        [X]      ");
                        break;
                    case 9:
                        Console.WriteLine("            [X]   ");
                        Console.WriteLine("     [ ][ ] //    "); 
                        Console.WriteLine(" Karta  [ ][ ][ ]");
                        Console.WriteLine("        [ ]      ");
                        break;
                    case 10:
                        Console.WriteLine("        [X]      ");
                        break;
                }
            } // NYI: Kartan ska visa alla upplåsta rum och ett kryss symboliserar spelarens position
        }
        public static void Main(string[] args)
        {
            bool stop = false;
            string command;
            Console.WriteLine("Hej och välkommen till The Forgotten Legacy! Skriv in kommando enligt följande:\n" +
                                "ladda /profil/ - ladda en sparad profil *NYI*\n" +
                                "spara /profil/ - spara din progress *NYI*\n" +
                                "starta - starta nytt spel"); // NYI: Låt spelaren skriva in sitt namn som sparas i en variabel som återanvänds senare i spelet.
            do
            {
                Console.Write($"> ");
                command = Console.ReadLine().Trim();
                Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
                if (command.Equals("sluta", StringComparison.CurrentCultureIgnoreCase)) stop = true;
                else if (command.Equals("ladda", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine($"{command} not yet implemented"); // NYI: ladda in en player profile med sparad progress
                }
                else if (command.Equals("spara", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine($"{command} not yet implemented"); // NYI: spara ner player progress
                }
                else if (command.Equals("starta", StringComparison.CurrentCultureIgnoreCase)) rooms[0].GoToNewRoom();
                ChangePlayerPosition(command);
            } while (!stop);
        }

        public static void ChangePlayerPosition(string command)
        {
            if (command.Equals("n", StringComparison.CurrentCultureIgnoreCase)) rooms[rooms[player.RoomPosition].North].GoToNewRoom();
            else if (command.Equals("ö", StringComparison.CurrentCultureIgnoreCase)) rooms[rooms[player.RoomPosition].East].GoToNewRoom();
            else if (command.Equals("s", StringComparison.CurrentCultureIgnoreCase)) rooms[rooms[player.RoomPosition].South].GoToNewRoom();
            else if (command.Equals("v", StringComparison.CurrentCultureIgnoreCase)) rooms[rooms[player.RoomPosition].West].GoToNewRoom();
        }
        public static void Typewriter(string text, int delay = 50)
        {
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write((i == text.Length - 1 ? $"{text[i]}\n" : text[i]));
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)
                {
                    delay = 0;
                }
                Thread.Sleep(delay);
            }
        }
        public static void TypewriterBold(string text, int delay = 50)
        {
            for (int i = 0; i < text.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(i == text.Length - 1 ? $"{text[i]}\u001b[0m\n" : $"\u001b[1m{text[i]}");
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)
                {
                    delay = 0;
                }
                Thread.Sleep(delay);
            }
        }
    }
}