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
            if (roomNumber == 0) Typewriter("Du har en nyckel som verkar passa i låset på dörren.");
            else if (roomNumber != 4 || roomNumber != 1) Typewriter("Nyckel passar i den låsta dörren");
            rooms[roomNumber].PrintActions();
            Console.WriteLine("   lås upp - lås upp dörren");
            answer = Console.ReadLine().Trim();
            Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
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
                Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
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
                        Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
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
                // BUG Print.Action() skrivs inte ut korrekt
                if (!player.HarÖppnatByrån || !player.HarInspekteratTavlan || !player.HarTittatISpisen || !player.HarSuttitIFotöljen || !player.HarTagitEnBrevkniv) rooms[roomNumber].PrintActions();
                if (!player.HarSuttitIFotöljen) Typewriter("   sitta - sätt dig ner i fåtöljen", 30);
                if (!player.HarInspekteratTavlan) Typewriter("   tavla - inspektera tavlan", 30);
                if (!player.HarTittatISpisen) Typewriter("   spis - undersök askan i spisen", 30);
                answer = Console.ReadLine().Trim();
                Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
                if (answer == "n" || answer == "s") { ChangePlayerPosition(answer); break; }
                switch (answer.ToLower())
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
                    Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
                    if (answer == "ö")
                    {
                        Typewriter("Du tittar på ritningen en gång till och undersöker sedan området där den stora tavlan sitter. Efter stort besvär lyckas du ta ner tavlan och finner en gömd dörr. Dörren verkar inte gå att öppna utan rätt kod\n");
                        do
                        {
                            Console.Write("Ange fyrsiffrig kod: ");
                            answer = Console.ReadLine().Trim();
                            Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
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
                            Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
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
                Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
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
                if (!player.HarTagitEnBrevkniv) Typewriter("   bord - undersök spegelbordet", 30);
                answer = Console.ReadLine().Trim();
                Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
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
                    Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
                    player.HarTagitEnBrevkniv = answer == "ja";
                    player.HarTittatPåBordet = true;
                }
            } while (!player.HarÖppnatByrån || !player.HarTagitEnBrevkniv);
            if (player.HarTagitEnBrevkniv && player.HarÖppnatByrån)
            {
                rooms[5].ChallengeDone = true;
                rooms[roomNumber].PrintActions();
            }
        };
        public static Action<int> Biblioteket = (roomNumber) =>
        {
            do
            {
                if (!player.HarHittatBoken)
                {
                    rooms[roomNumber].PrintActions();
                    Typewriter("   hylla - inspektera bokhyllan", 30);
                    Typewriter("   maskin - inspektera skrivmaskinen", 30);
                    Typewriter("   bok - inspektera anteckningsboken", 30);
                }
                answer = Console.ReadLine().Trim();
                Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
                if (answer == "n" || answer == "v" || answer == "ö") { ChangePlayerPosition(answer); break; }
                switch (answer.ToLower())
                {
                    case "hylla":
                        if (!player.HarLästBrevet)
                        {
                            Typewriter("\nDu står framför en stor bokhylla full av dammiga böcker i en mörk del av biblioteket.Många av böckerna ser ut att aldrig ha lästs och en del av dem verkar ha fallit sönder med tiden.Du ser några böcker med titlar som fångar din uppmärksamhet, men du kan inte hjälpa att undra om det finns någon annan bok här som kan vara viktigare.");
                        }
                        else
                        {
                            Typewriter("\nDu tittar på bokhyllan igen och din blick fastnar på en bok som står lite förskjuten från de andra. Titeln på boken lyder \"Codex Seraphinianus\" och författaren är Luigi Serafini, precis som det stod i brevet du läste tidigare. Boken ser ut att vara gammal och ovanlig, och du kan inte hjälpa att undra vad som döljer sig mellan dess sidor.");
                            player.HarHittatBoken = true;
                            Typewriter("\nVill du ta tag i boken?");
                            Typewriter("   ja - dra ut boken", 30);
                            Typewriter("   nej - låt boken vara", 30);
                        }
                        break;
                    case "maskin":
                        Typewriter("\nNär du inspekterar skrivmaskinen ser du ett papper med ett konstigt diagram på det. Det verkar vara någon form av kod, men du är inte säker på vad det betyder. Diagrammet har alla bokstäver i alfabetet, var och en med en motsvarande symbol under den. Det finns också flera rader med bokstäver som verkar vara blandade. Det ser ut som någon form av pussel som behöver lösas.\n");
                        Console.WriteLine("A B C D E F G H I J K L M N O P Q R S T U V W X Y Z\r\n| | | | | | | | | | | | | | | | | | | | | | | | | |\r\nX O M C H L F U T I S Z E B A D J G K P Q R V W Y N");
                        break;
                    case "bok":
                        Typewriter("\nNär du inspekterar anteckningsboken hittar du en skiss av ett halsband bredvid texten SAFIR. Skissen är imponerande och safiren sticker ut från blyertslinjerna med sin lysande blåa färg. Det är tydligt att skissen har gjorts med stor noggrannhet och omsorg, som om halbandet betyder väldigt mycket för den som ritade den.");
                        Console.WriteLine("                       ");
                        Console.WriteLine("                 \u001b[2m##### ");
                        Console.WriteLine("                #     #");
                        Console.WriteLine("                #     #");
                        Console.WriteLine("       \u001b[0m\u001b[3mSAFIR\u001b[0m\u001b[2m     #   # ");
                        Console.WriteLine("                  # #  ");
                        Console.WriteLine("                   #   ");
                        Console.WriteLine("                  \u001b[0m\u001b[34m/ \\ ");
                        Console.WriteLine("                  \\_/\u001b[0m ");
                        Console.WriteLine("                       ");
                        break;
                    case "ja":
                        Typewriter("\nNär du drar ut boken ur bokhyllan hör du ett klickande ljud och plötsligt öppnar bokhyllan upp sig och visar en smal öppning bakom sig. Öppningen verkar leda till en hemlig passage.");
                        rooms[9].RoomUnlocked = true;
                        rooms[roomNumber].PrintActions(); // FIXME: dubbla actions skrivs ut!
                        rooms[2].ChallengeDone = true;
                        break;
                    case "nej":
                        Typewriter("\nDu tittar på boken med tvekan. Något känns inte rätt. Du kan inte låta bli att undra vad som skulle hända om du drar ut den. Men kanske är det bäst att låta den vara.");
                        rooms[roomNumber].PrintActions();
                        break;
                    default: break;
                }
            } while (rooms[2].ChallengeDone == false);
        };
        public static Action<int> Badrummet = (roomNumber) =>
        {
            do
            {
                rooms[6].PrintActions();
                Typewriter("   handfat - öppna kranen", 30);
                Typewriter("   dusch - inspektera duschdraperiet", 30);
                if (!player.HarInspekteratSpegeln) Typewriter("   spegel - titta på din spegelbild", 30);
                if (player.HarInspekteratSpegeln && !player.HarHittatKassaskåpet) Typewriter("   tryck - tryck på knappen", 30);
                if (player.HarHittatKassaskåpet && !player.HarÖppnatKassaskåpet) Typewriter("   öppna - öppna kassaskåpet", 30);
                if (player.HarÖppnatKassaskåpet && player.HarTagitEnBrevkniv && !player.HarLästBrevet) Typewriter("   brev - öppna brevet med kniven");
                answer = Console.ReadLine().Trim();
                Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
                if (answer == "n" || answer == "v" || answer == "ö") { ChangePlayerPosition(answer); break; }
                switch (answer.ToLower())
                {
                    case "handfat":
                        break;
                    case "dusch":
                        break;
                    case "spegel":
                        Typewriter("Du går närmare spegeln och hör ett dovt knarrande ljud när du trampar på ett specifikt ställe på golvet framför spegeln. Detta känns lite misstänkt, så du tar en närmare titt på golvet. Du märker att en av golvplankorna på golvet verkar lite lösare än resten, som om den kan flyttas. Med lite ansträngning lyckas du lyfta upp golvplankan, och under den hittar du en dold knapp.");
                        player.HarInspekteratSpegeln = true;
                        break;
                    case "tryck":
                        Typewriter("Det hörs ett klickljud, och plötsligt glider en sektion av väggen undan för att avslöja ett dolt kassaskåp");
                        player.HarHittatKassaskåpet = true;
                        break;
                    case "öppna":
                        do
                        {
                            Typewriter("Du försöker öppna kassaskåpet men du behöver ange rätt bokstavskombination med fem bokstäver.\nAnge bokstavskombinationen: "); // NYI: Kassaskåpet sprängs om man skriver fel tre gånger och spelaren nollställs och får börja om.
                            answer = Console.ReadLine().Trim();
                            Console.WriteLine("\u001b[38;5;208m------------------------------\u001b[0m");
                            if (answer.Equals("kxltg") || answer.Equals("kogjv"))
                            {
                                Typewriter("Kassaskåpet öppnas och du hittar ett brev.");
                                player.HarÖppnatKassaskåpet = true;
                            }
                            else
                            {
                                Typewriter("Det var inte rätt kod, prova igen på egen risk...");
                            }
                        } while (!player.HarÖppnatKassaskåpet);
                        break;
                    case "brev":
                        Typewriter("Kära (namn), Om du läser detta brev betyder det att jag inte längre finns bland er. Men jag hoppas innerligt att mina anteckningar kan hjälpa dig att lösa gåtorna som jag har lämnat efter mig. Jag har efterlämnat en bok som du kommer att behöva för att kunna öppna den hemliga dörren bakom bokhyllan i biblioteket.Boken heter \"Codex Seraphinianus\" och är skriven av Luigi Serafini. Den är väldigt gammal och värdefull, så var försiktig med den.\nHälsningar,\nEn vän");
                        player.HarLästBrevet = true;
                        break;
                    default: break;
                }
            } while (rooms[6].ChallengeDone == false);
        };
        public static Action<int> HemligaRummet = (roomNumber) =>
        {
            Typewriter("\nDet här är slutet på spelet för tillfälligt, håll ögonen öppna efter en ny version!");
        };
    }
}
