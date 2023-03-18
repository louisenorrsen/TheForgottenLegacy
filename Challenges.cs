﻿using static MUD.Program;
using static MUD.Rooms;

namespace MUD
{
    public class Challenges
    {
        public static Random random = new Random();
        public static string answer;
        public static Action<int> FoundKey = (roomNumber) =>
        {
            if (roomNumber == 0) Typewriter("Du har en nyckel som verkar passa i låset på dörren.");
            else if (roomNumber != 4 || roomNumber != 1) Typewriter("Nyckel passar i den låsta dörren");
            rooms[roomNumber].PrintActions();
            Console.WriteLine("   lås upp - lås upp dörren");
            answer = Console.ReadLine().Trim();
            Console.WriteLine("------------------------------");
            if (answer.Equals("lås upp", StringComparison.CurrentCultureIgnoreCase))
            {
                Typewriter("Dörren är upplåst!");
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
                if (roomNumber != 0) TypewriterBold($"\nDu står i {rooms[roomNumber].Name}");

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
                Typewriter("Du ser dig omkring och ser ett nyckelskåp på väggen.");
                rooms[roomNumber].PrintActions();
                Typewriter("   öppna - öppna nyckelskåpet");
                answer = Console.ReadLine().Trim();
                Console.WriteLine("------------------------------");
                if (answer.Equals("öppna", StringComparison.CurrentCultureIgnoreCase))
                {
                    int theDoorKey = random.Next(1, 4);
                    Typewriter("I nyckelskåpet hänger tre olika nycklar.");
                    do
                    {
                        rooms[roomNumber].PrintActions(); // NYI: Anpassa utskrift efter vilka nycklar man redan testat
                        Typewriter("   1 - ta nyckel nummer ett", 30);
                        Typewriter("   2 - ta nyckel nummer två", 30);
                        Typewriter("   3 - ta nyckel nummer tre\n", 30);
                        answer = Console.ReadLine().Trim();
                        Console.WriteLine("------------------------------");
                        if (Convert.ToInt32(answer).Equals(theDoorKey)) player.HarVardagsrumsNyckel = true;
                        if (Convert.ToInt32(answer) != theDoorKey) Typewriter("Det var tyvärr inte rätt nyckel.", 30);
                    } while (!player.HarVardagsrumsNyckel);
                    Typewriter("Du hittade rätt nyckel!", 30);
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
                if (!player.HarÖppnatByrån && !player.HarInspekteratTavlan || !player.HarTittatISpisen || !player.HarSuttitIFotöljen) rooms[roomNumber].PrintActions();
                if (!player.HarSuttitIFotöljen) Typewriter("   sitta - sätt dig ner i fåtöljen", 30);
                if (!player.HarInspekteratTavlan) Typewriter("   tavla - inspektera tavlan", 30);
                if (!player.HarTittatISpisen) Typewriter("   spis - undersök askan i spisen", 30);
                answer = Console.ReadLine().Trim();
                Console.WriteLine("------------------------------");
                if (answer == "n" || answer == "s") { ChangePlayerPosition(answer); break; }
                switch (answer)
                {
                    case "sitta":
                        Typewriter("\nDu får syn på ett till rum norrut, det ser ut som ett kök.");
                        rooms[3].RoomUnlocked = true;
                        player.HarSuttitIFotöljen = true;
                        break;
                    case "tavla":
                        Typewriter("\nDu tittar närmare på tavlan som föreställer ett gammalt torp mitt i en frodig äng, omgiven av högt gräs och vilda blommor. Solen skiner på torpet och en mjuk blå himmel syns i bakgrunden. Nere till höger ses konstnärens namn följt av siffrorna \u001b[32m1259\u001b[0m.");
                        player.HarInspekteratTavlan = true;
                        break;
                    case "spis":
                        Typewriter("\nDu tittar bland askan och finner en blänkande ädelsten, det ser ut som en \u001b[34mmörkblå safir\u001b[0m.");
                        player.HarHittatSafiren = true;
                        player.HarTittatISpisen = true;
                        break;
                    default:
                        break;
                }

                if (player.HarÖppnatByrån && player.HarInspekteratTavlan && rooms[4].ChallengeDone == false)
                {
                    rooms[2].RoomUnlocked = true;
                    rooms[roomNumber].PrintActions();
                    if (!player.HarTittatISpisen) Typewriter("   spis - undersök askan i spisen", 30);
                    answer = Console.ReadLine().Trim();
                    Console.WriteLine("------------------------------");
                    if (answer == "ö")
                    {
                        Typewriter("Du tittar på ritningen en gång till och undersöker sedan området där den stora tavlan sitter. Efter stort besvär lyckas du ta ner tavlan och finner en gömd dörr. Dörren verkar inte gå att öppna utan rätt kod\n");
                        do
                        {
                            Console.Write("Ange fyrsiffrig kod: ");
                            answer = Console.ReadLine().Trim();
                            Console.WriteLine("------------------------------");
                            if (Convert.ToInt32(answer) == 1259)
                            {
                                Typewriter("Det var rätt kod, ett klickande ljud hörs och dörren är upplåst!\n", 30);
                                rooms[4].ChallengeDone = true;
                                ChangePlayerPosition("ö");
                                break;
                            }
                            else Typewriter("Det var tyvärr fel kod, vill du prova igen?", 30);
                            Console.WriteLine("   ja - prova igen");
                            Console.WriteLine("   nej - ge upp");
                            answer = Console.ReadLine().Trim();
                            Console.WriteLine("------------------------------");
                        } while (answer != "nej");
                    }
                    else if (answer.Equals("spis", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Typewriter("\nDu tittar bland askan och finner en blänkande ädelsten, det ser ut som en \u001b[34mmörkblå safir\u001b[0m.");
                        player.HarHittatSafiren = true;
                        player.HarTittatISpisen = true;
                    }
                    else if (answer == "n" || answer == "s")
                    {
                        ChangePlayerPosition(answer);
                    }
                }
            } while (rooms[4].ChallengeDone == false);
        };
        public static Action<int> Köket = (roomNumber) =>
        {
            do
            {
                rooms[roomNumber].PrintActions();
                if (!player.HarTittatIKöket) Console.WriteLine("   titta - se dig omkring");
                if (player.HarTittatIKöket && !player.HarSovrumsNyckel) Typewriter("   nyckel - plocka upp nyckeln", 30);
                answer = Console.ReadLine().Trim();
                if (answer == "n" || answer == "s" || answer == "v") { ChangePlayerPosition(answer); break; }
                Console.WriteLine("------------------------------");
                if (answer.Equals("titta", StringComparison.CurrentCultureIgnoreCase))
                {
                    Typewriter("\nNär du ser dig omkring upptäcker du en nyckel som ligger glömd på en av köksbänkarna.");
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
                if (!player.HarÖppnatByrån) Typewriter("   byrå - öppna lådan", 30);
                if (!player.HarTittatPåBordet) Typewriter("   bord - undersök spegelbordet", 30);
                answer = Console.ReadLine().Trim();
                Console.WriteLine("------------------------------");
                if (answer == "n" || answer == "s" || answer == "ö") { ChangePlayerPosition(answer); break; }
                else if (answer.Equals("byrå", StringComparison.CurrentCultureIgnoreCase))
                {
                    Typewriter("\nNär du öppnar lådan i byrån hittar du en \u001b[31mritning över huset\u001b[37m du befinner dig i. Något är dock märkligt med ritningen, det ser ut som att det ska finnas en dörr till ett bibliotek i vardagsrummet.");
                    player.HarÖppnatByrån = true;
                    rooms[2].RoomUnlocked = player.HarInspekteratTavlan;
                }
                else if (answer.Equals("bord", StringComparison.CurrentCultureIgnoreCase))
                {
                    Typewriter("\nPå spegelbordet ligger en brevkniv, vill du plocka upp den?"); // TODO fix foreground color 
                    Typewriter("   ja - plocka upp brevkniven", 30);
                    Typewriter("   nej - låt brevkniven ligga kvar", 30);
                    answer = Console.ReadLine().Trim();
                    Console.WriteLine("------------------------------");
                    player.HarTagitEnBrevkniv = answer == "ja";
                    player.HarTittatPåBordet = true;
                }
            } while (!player.HarÖppnatByrån || !player.HarTittatPåBordet);
            if (player.HarTagitEnBrevkniv && player.HarÖppnatByrån) rooms[5].ChallengeDone = true;
            rooms[roomNumber].PrintActions();
        };
        public static Action<int> Biblioteket = (roomNumber) =>
        {
            rooms[roomNumber].PrintActions();
            Typewriter("   hylla - inspektera bokhyllan", 30);
            Typewriter("   maskin - inspektera skrivmaskinen", 30);
            Typewriter("   bok - inspektera anteckningsboken", 30);
            answer = Console.ReadLine().Trim();
            Console.WriteLine("------------------------------");
        };
        public static Action<int> Badrummet = (roomNumber) =>
        {
            
        };
        public static Action<int> Default = (roomNumber) =>
        {
            if (true)
            {
                Console.WriteLine("");
            }
        };
    }
}
