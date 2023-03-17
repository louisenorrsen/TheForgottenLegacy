using static MUD.Program;
using static MUD.Rooms;

namespace MUD
{
    public class Challenges
    {
        public static Random random = new Random();
        public static string answer;
        public static Action<int> FoundKey = (roomNumber) =>
        {
            if (roomNumber == 0) Console.WriteLine("Du har en nyckel som verkar passa i låset på dörren.");
            else if (roomNumber == 4 || roomNumber == 1) Console.WriteLine("");
            else Console.WriteLine("Nyckel passar i den låsta dörren");
            rooms[roomNumber].PrintActions();
            Console.WriteLine("   lås upp - lås upp dörren\n");
            answer = Console.ReadLine();
            Console.WriteLine("------------------------------");
            if (answer.Equals("lås upp", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine("Dörren är upplåst!\n");
                switch (roomNumber)
                {
                    case 0:
                        rooms[1].RoomUnlocked = true;
                        break;
                    case 1:
                        rooms[4].RoomUnlocked = true;
                        break;
                    case 3:
                        rooms[5].RoomUnlocked = true;
                        break;
                }
                if (roomNumber != 0)Console.WriteLine($"\n{rooms[roomNumber].Name}\n{rooms[roomNumber].Description}\n");
                
            }
        };
        public static Action<int> Utomhus = (roomNumber) =>
        {
            if (player.HasHouseKey) FoundKey(roomNumber);
            rooms[roomNumber].PrintActions();
        };
        public static Action<int> Entréhallen = (roomNumber) =>
        {
            while (!rooms[roomNumber].ChallengeDone)
            {
                Console.WriteLine("Du ser dig omkring och ser ett nyckelskåp på väggen.");
                rooms[roomNumber].PrintActions();
                Console.WriteLine("   öppna - öppna nyckelskåpet\n");
                answer = Console.ReadLine();
                Console.WriteLine("------------------------------");
                if (answer.Equals("öppna", StringComparison.CurrentCultureIgnoreCase))
                {
                    int theDoorKey = random.Next(1, 4);
                    Console.WriteLine("I nyckelskåpet hänger tre olika nycklar.");
                    do
                    {
                        rooms[roomNumber].PrintActions();
                        Console.WriteLine("   1 - ta nyckel nummer ett");
                        Console.WriteLine("   2 - ta nyckel nummer två");
                        Console.WriteLine("   3 - ta nyckel nummer tre\n");
                        answer = Console.ReadLine();
                        Console.WriteLine("------------------------------");
                        if (Convert.ToInt32(answer).Equals(theDoorKey)) player.HarVardagsrumsNyckel = true;
                        if (Convert.ToInt32(answer) != theDoorKey) Console.WriteLine("Det var tyvärr inte rätt nyckel.");
                    } while (!player.HarVardagsrumsNyckel);
                    Console.WriteLine("Du hittade rätt nyckel!");
                    FoundKey(roomNumber);
                    rooms[roomNumber].ChallengeDone = true;
                }
            }
            if (rooms[roomNumber].ChallengeDone) rooms[roomNumber].PrintActions();
        };
        public static Action<int> Vardagsrummet = (roomNumber) =>
        {
            do
            {
                rooms[roomNumber].PrintActions();
                if (!player.HarSuttitIFotöljen) Console.WriteLine("   sitta - sätt dig ner i fåtöljen");
                if (!player.HarInspekteratTavlan) Console.WriteLine("   tavla - inspektera tavlan");
                if (!player.HarTittatISpisen) Console.WriteLine("   spis - undersök askan i spisen\n");
                answer = Console.ReadLine();
                Console.WriteLine("------------------------------");
                if (answer == "n" || answer == "s" || answer == "ö") { ChangePlayerPosition(answer); break; }
                switch (answer)
                {
                    case "sitta":
                        Console.WriteLine("\nDu får syn på ett till rum norrut, det ser ut som ett kök.");
                        rooms[3].RoomUnlocked = true;
                        player.HarSuttitIFotöljen = true;
                        break;
                    case "tavla":
                        Console.WriteLine("\nDu tittar närmare på tavlan som föreställer ett gammalt torp mitt i en frodig äng, omgiven av högt gräs och vilda blommor. Solen skiner på torpet och en mjuk blå himmel syns i bakgrunden. Nere till höger ses konstnärens namn följt av siffrorna 1259");
                        player.HarInspekteratTavlan = true;
                        break;
                    case "spis":
                        Console.WriteLine("\nDu tittar bland askan och finner en blänkande ädelsten, det ser ut som en mörkblå safir.");
                        player.HarHittatSafiren = true;
                        player.HarTittatISpisen = true;
                        break;
                    default:
                        break;
                }
            } while (!player.HarSuttitIFotöljen || !player.HarInspekteratTavlan || !player.HarTittatISpisen);
            if (player.HarÖppnatByrån && player.HarInspekteratTavlan && rooms[roomNumber].ChallengeDone == false)
            {
                rooms[2].RoomUnlocked = true;
                rooms[roomNumber].PrintActions();
                answer = Console.ReadLine();
                Console.WriteLine("------------------------------");
                if (answer == "ö")
                {
                    Console.WriteLine("Du tittar på ritningen en gång till och undersöker sedan området där den stora tavlan sitter. Efter stort besvär lyckas du ta ner tavlan och finner en gömd dörr. Dörren verkar inte gå att öppna utan rätt kod\n");
                    do
                    {
                        Console.Write("Ange fyrsiffrig kod: ");
                        answer = Console.ReadLine();
                        Console.WriteLine("------------------------------");
                        if (Convert.ToInt32(answer) == 1259)
                        {
                            Console.WriteLine("Det var rätt kod, ett klickande ljud hörs och dörren är upplåst!");
                            rooms[roomNumber].ChallengeDone = true;
                            ChangePlayerPosition("ö");
                            break;
                        }
                        else Console.WriteLine("Det var tyvärr fel kod, vill du prova igen?");
                        Console.WriteLine("   ja - prova igen");
                        Console.WriteLine("   nej - ge upp");
                        answer = Console.ReadLine();
                        Console.WriteLine("------------------------------");
                    } while (answer != "nej");
                }
                else if (answer == "n" || answer == "s")
                {
                    ChangePlayerPosition(answer);
                }
            }
        };
        public static Action<int> Köket = (roomNumber) =>
        {
            do
            {
                rooms[roomNumber].PrintActions();
                if (!player.HarTittatIKöket) Console.WriteLine("   titta - se dig omkring");
                if (player.HarTittatIKöket && !player.HarSovrumsNyckel) Console.WriteLine("   nyckel - plocka upp nyckeln");
                answer = Console.ReadLine();
                if (answer == "n" || answer == "s" || answer == "v") { ChangePlayerPosition(answer); break; }
                Console.WriteLine("------------------------------");
                if (answer.Equals("titta", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("\nNär du ser dig omkring upptäcker du en nyckel som ligger glömd på en av köksbänkarna.");
                    player.HarTittatIKöket = true;
                }
                else if (answer.Equals("nyckel", StringComparison.CurrentCultureIgnoreCase)) { player.HarSovrumsNyckel = true; rooms[3].ChallengeDone = true; }
            } while (!player.HarSovrumsNyckel);
             if (rooms[5].RoomUnlocked == false) FoundKey(roomNumber);
             if (rooms[5].RoomUnlocked == true) rooms[roomNumber].PrintActions();
        };
        public static Action<int> Sovrummet = (roomNumber) =>
        {
            do
            {
                rooms[roomNumber].PrintActions();
                if (!player.HarÖppnatByrån) Console.WriteLine("   byrå - öppna lådan");
                if (!player.HarTittatPåBordet) Console.WriteLine("   bord - undersök spegelbordet\n");
                answer = Console.ReadLine();
                Console.WriteLine("------------------------------");
                if (answer == "n" || answer == "s" || answer == "ö") { ChangePlayerPosition(answer); break; }
                else if (answer.Equals("byrå", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("När du öppnar lådan i byrån hittar du en ritning över huset du befinner dig i. Något är dock märkligt med ritningen, det ser ut som att det ska finnas en dörr till ett bibliotek i vardagsrummet.");
                    player.HarÖppnatByrån = true;
                    rooms[2].RoomUnlocked = true;
                }
                else if (answer.Equals("bord", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("På spegelbordet ligger en brevkniv, vill du plocka upp den?");
                    Console.WriteLine("   ja - plocka upp brevkniven");
                    Console.WriteLine("   nej - låt brevkniven ligga kvar");
                    answer = Console.ReadLine();
                    Console.WriteLine("------------------------------");
                    player.HarTagitEnBrevkniv = answer == "ja" ? true : false;
                    player.HarTittatPåBordet = true;
                }
            } while (!player.HarÖppnatByrån || !player.HarTittatPåBordet);
            if (player.HarTagitEnBrevkniv && player.HarÖppnatByrån) rooms[roomNumber].ChallengeDone = true;
            rooms[roomNumber].PrintActions();
        };
        public static Action<int> DefaultChallenge = (roomNumber) =>
        {
            if (true)
            {
                Console.WriteLine("");
            }
        };
    }
}
