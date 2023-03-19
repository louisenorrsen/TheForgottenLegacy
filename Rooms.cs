using static MUD.Program;
using static MUD.Challenges;

namespace MUD
{
    public class Rooms
    {
        public static List<Room> rooms = new List<Room>() {
            new Room
            (
                0,
                "Utomhus",
                "Du står på trappen in till ett övergivet hus.\n\n" +
                "Norrut har du dörren in till huset.\n" +
                "Österut och västerut är det staket.",
                1, -1, -1, -1,
                Utomhus
            )
            {
                RoomUnlocked = true
            },
            new Room
            (
                1,
                "Entréhallen",
                "Du står i en stor entréhall med högt i tak. Det hänger en stor lykta från taket.",
                4, -1, -1, -1,
                Entréhallen
            ),
            new Room
            (
                2,
                "Biblioteket",
                "Du befinner dig i ett stort bibliotek med höga bokhyllor längs väggarna. En gammal skrivmaskin står på ett bord vid fönstret. En gammal anteckningsbok med gula sidor och ett märkligt mönster på pärmarna ligger öppen bredvid skrivmaskinen.",
                9, 6, -1, 4,
                Biblioteket
            ),
            new Room
            (
                3,
                "Köket",
                "Du är i ett stort kök med en enorm spis. Köket är fyllt med dammiga möbler och skåp täckta av spindelnät. En märklig doft hänger i luften, som om maten fortfarande lagades här för länge sedan.",
                -1, -1, 4, 5,
                Köket
            ),
            new Room
            (
                4,
                "Vardagsrummet",
                "Du befinner dig i ett rymligt vardagsrum med mjuka fåtöljer och en stor öppen spis. En stor tavla hänger på väggen.",
                3, 2, 1, -1,
                Vardagsrummet
            ),
            new Room
            (
                5,
                "Sovrummet",
                "Rummet är täckt av ett tjockt lager av damm och spindelnät. En gammal säng med trasiga täcken och kuddar står i ena hörnet, medan en rucklig byrå och ett antikt spegelbord står mot väggen på andra sidan av rummet.",
                -1, 3, -1, -1,
                Sovrummet
            ),
            new Room
            (
                6,
                "Badrummet",
                "Du står i badrummet, ett litet utrymme med träpanel på väggarna och en trägolv som knarrar under dina fötter. Ett antikt handfat av keramik med en gammal kopparpipa står vid väggen. På andra sidan rummet finns en liten duschkabin med ett randigt duschdraperi. En spegel täcker hela väggen framför dig.",
                -1, -1, -1, 2,
                Badrummet
            )
            {
                RoomUnlocked = true
            },
            new Room
            (
                7,
                "Trappan",
                "Trappan leder från bottenvåningen till övervåningen och är gjord av mörkt trä som knarrar under dina fötter när du kliver på den. Väggarna är prydda med gamla familjefoton, vilket ger intrycket av att denna trappa har sett generationer av familjemedlemmar som har gått upp och ner för den.",
                8, -1, 9, -1,
                null
            ),
            new Room
            (
                8,
                "Mörkt rum",
                "När du öppnar dörren till det här rummet känner du omedelbart hur det kalla draget från rummets mörka inre stryker över dig. Du kan knappt se något i det mörka rummet, men du kan höra ljuden av något som rör sig. När dina ögon vänjer sig vid mörkret ser du att rummet innehåller en gammal säng, ett skåp och några stolar. Men det verkar som att det finns något annat i rummet, något som du inte kan se...",
                -1, 10, 7, -1,
                null
            ),
            new Room
            (
                9,
                "Hemligt rum",
                "Du har upptäckt en dold dörr bakom en hylla. När du öppnar dörren avslöjas ett litet rum som verkar ha varit gömt i årtionden. Rummet är tomt förutom några gamla möbler som ser ut att ha använts för en gång i tiden. Du kan känna historiens närvaro när du tittar runt i det gömda rummet.",
                7, -1, 2, -1,
                HemligaRummet
            ),
            new Room
            (
                10,
                "Tornrummet",
                "Tornrummet ligger högst upp i huset och nås genom en smal spiraltrappa som leder till en dörr. När du öppnar dörren och kliver in i rummet, slås du av den fantastiska utsikten från de höga fönstren. Rummet är ganska stort och innehåller en säng, en fåtölj och en liten bokhylla. Det är uppenbart att detta rum har använts som en plats för meditation eller ensamhet, med dess fridfulla atmosfär och lugna utsikt.",
                -1, -1, -1, 8,
                null
            )
         };
    }
}
