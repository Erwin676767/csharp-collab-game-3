using System;

// WSPOLNY KONTRAKT. Wszystkie dane inicjalizujemy przed Start().
const int MAKS_ENERGIA = 10;
int pokoj = 1;
int energia = MAKS_ENERGIA;
bool karta = false;
bool bezpiecznik = false;
bool zasilanie = false;
bool koniec = false;
bool wygrana = false;

string[] pokoje = {
    "Recepcja",
    "Magazyn",
    "Serwerowania",
    "Wyjście"
};

int licznikOdrzuconychProbRuchu = 0;

DziennikWyprawy dziennik = new DziennikWyprawy();

// SEKCJA 0: GOTOWY SILNIK NAUCZYCIELA. Nie zmieniaj w zadaniu podstawowym.
Start();
while (true)
{
    Console.Write("> ");
    string? wejscie = Console.ReadLine();
    if (wejscie == null)
    {
        Console.WriteLine("Koniec wejscia. Zamykam program.");
        break;
    }

    string polecenie = wejscie.Trim().ToLowerInvariant();
    if (polecenie == "koniec")
    {
        Console.WriteLine("Do zobaczenia!");
        break;
    }
    if (polecenie == "")
    {
        Console.WriteLine("Wpisz polecenie albo pomoc.");
        continue;
    }

    // Rozdzielacz dziala od poczatku, zeby zespoly mogly testowac niezaleznie.
    switch (polecenie)
    {
        case "start": Start(); break;
        case "pomoc": Pomoc(); break;
        case "status": Status(); break;
        case "mapa": Mapa(); break;
        case "opis": Rozejrzyj(); break;
        case "lewo": Idz("lewo"); break;
        case "prawo": Idz("prawo"); break;
        case "karta": Akcja("karta"); break;
        case "bezpiecznik": Akcja("bezpiecznik"); break;
        case "napraw": Akcja("napraw"); break;
        case "wyjdz": Akcja("wyjdz"); break;
        case "odkrycia": PokazOdkrycia(); break;
        case "dziennik": WyswietlWpisy(); break;
        default: Console.WriteLine("Nieznane polecenie. Wpisz pomoc."); break;
    }
}

void PokazOdkrycia()
{
    Console.WriteLine("Odkryte pokoje: ");
    for (int i = 0; i < pokoje.Length; i++)
    {
        if (dziennik.CzyOdwiedzono(i+1)) 
            Console.WriteLine($"{i+1}. {pokoje[i]} ");
        
    }
    
    switch (dziennik.LiczbaOdwiedzonych)
    {
        case 1: Console.WriteLine("Odwiedzono jeden z czterech pokoi"); break;
        case 2: Console.WriteLine("Odwiedzono dwa z czterech pokoi"); break;
        case 3: Console.WriteLine("Odwiedzono trzy z czterech pokoi"); break;
        case 4: Console.WriteLine("Odwiedzono wszystkie pokoje!"); break;
        default: Console.WriteLine("Błąd"); break;
    }

    Console.Write("Pasek odkryć: ");
    for (int i = 0; i < pokoje.Length; i++)
    {
        if (dziennik.CzyOdwiedzono(i+1))
            Console.Write("#");
        else
            Console.Write(".");
    }
    Console.WriteLine();
}

void WyswietlWpisy()
{
    Console.WriteLine("Dziennik:");
    for(int i = 0; i<dziennik.LiczbaWpisow; i++)
    {
        Console.WriteLine($"{dziennik.LiczbaWpisow-i}. {dziennik.PobierzWpis(i)}");
    }
}


void Start()
{
    pokoj = 1;
    energia = MAKS_ENERGIA;
    karta = false;
    bezpiecznik = false;
    zasilanie = false;
    koniec = false;
    wygrana = false;

    dziennik.Reset();

    licznikOdrzuconychProbRuchu = 0;

    Console.WriteLine("UCIECZKA Z SERWEROWNI: PIATEK, 16:59.");
    Console.WriteLine("Administrator napisal: u mnie dziala. Drzwi twierdza inaczej.");
    Console.WriteLine("Masz " + MAKS_ENERGIA + " energii. Wpisz pomoc. Koniec zamyka program.");
    Rozejrzyj();
}

void ZakonczTure()
{
    energia = energia - 1;
    Console.WriteLine("Pozostala energia: " + energia);

    if (wygrana)
    {
        koniec = true;
        Console.WriteLine("WYGRANA! Drzwi otwarte. Weekend uratowany.");
    }
    else if (energia <= 0)
    {
        koniec = true;
        Console.WriteLine("PRZEGRANA. Energia sie skonczyla. Wpisz start albo koniec.");
    }
}

// Status jest gotowy: sluzy do testow, zanim autor mapy skonczy swoja sekcje.
void Status()
{
    Console.WriteLine("Pokoj: " + pokoj);
    Console.WriteLine("Energia: " + energia + "/" + MAKS_ENERGIA + " " + OcenaEnergii(energia));
    Console.WriteLine("Pasek Energii: " + PasekEnergii());
    Console.WriteLine("Karta: " + karta);
    Console.WriteLine("Bezpiecznik: " + bezpiecznik);
    Console.WriteLine("Zasilanie: " + zasilanie);
    Console.WriteLine("Koniec: " + koniec);
    Console.WriteLine("Wygrana: " + wygrana);
    Console.WriteLine("Liczba odrzuconych prób ruchu: " + licznikOdrzuconychProbRuchu);
    if (energia >= MinimalnyKosztWygranej() && !koniec)
    {
        Console.WriteLine("Minimalny koszt: "+ MinimalnyKosztWygranej() + "; energia: " + energia + " wystarcza.");
    }
    else if (energia < MinimalnyKosztWygranej() && !koniec)
    {
        Console.WriteLine("Minimalny koszt: "+ MinimalnyKosztWygranej() + "; energia: " + energia + " nie wystarcza.");
    }
}

int MinimalnyKosztWygranej()
{
    int koszt = 0;
    int pokojSymulacja = pokoj;

    if (!karta)
    {
        koszt += 1 + Odleglosc(pokojSymulacja,1);
        pokojSymulacja = 1;
    }
    if (!bezpiecznik && !zasilanie)
    {
        //bezpiecznik
        koszt += 1 + Odleglosc(pokojSymulacja,2);
        pokojSymulacja = 2;
        //zasilanie
        koszt += 1 + Odleglosc(pokojSymulacja,3);
        pokojSymulacja = 3;
    }
    if (bezpiecznik)
    {
        koszt += 1 + Odleglosc(pokojSymulacja,3);
        pokojSymulacja = 3;
    }
    if (wygrana)
    {
        return 0;
    }
    return koszt + Odleglosc(pokojSymulacja,4) + 1;
}
int Odleglosc(int pokojPoczatkowy, int pokojDocelowy)
{ 
    if (pokojPoczatkowy > pokojDocelowy)
    {
        return pokojPoczatkowy - pokojDocelowy;
    }
    else if (pokojDocelowy > pokojPoczatkowy)
    {
        return pokojDocelowy - pokojPoczatkowy;
    }
    return 0;
}
// SEKCJA A: INFORMACJE I MAPA. Wlasciciel: osoba A.
string NazwaPokoju(int numer)
{
    if (numer >= 1 && numer <= pokoje.Length)
        return pokoje[numer - 1];

    return "Nieznane pomieszczenie";
}

void Pomoc()
{
    Console.WriteLine("\nPomoc:");
    Console.WriteLine("\tpomoc          Wyświetla polecenia i zasady rozgrywki");
    Console.WriteLine("\tstatus         Wyświetla dane rozgrywki");
    Console.WriteLine("\tmapa           Wyświetla pokoje i znacznik pozycji");
    Console.WriteLine("\topis           Rozejrzenie się w aktualnym pokoju");
    Console.WriteLine("\tlewo           Cofnięcie się do poprzedniego pokoju (E)");
    Console.WriteLine("\tprawo          Pójście do następnego pokoju (E)");
    Console.WriteLine("\tkarta          Próba zabrania karty (E)");
    Console.WriteLine("\tbezpiecznik    Próba zabrania bezpiecznika (E)");
    Console.WriteLine("\tnapraw         Próba naprawy zasilania (E)");
    Console.WriteLine("\twyjdz          Próba otwarcia drzwi (E)");
    Console.WriteLine("\tstart          Rozpoczęcie nowej rozgrywki");
    Console.WriteLine("\tkoniec         Zamknięcie programu\n");

    Console.WriteLine("\tTylko udany ruch i akcja które są oznaczone jako (E)\n\tspowoduje zabranie energi\n");
}

void Mapa()
{
    Console.WriteLine("");
    for (int numer = 1; numer <= 4; numer++) 
    {
        string odkryte = dziennik.CzyOdwiedzono(numer) ? "odkryty" : "nieodkryty";
        if (numer == pokoj)
            Console.WriteLine($"{numer}. {NazwaPokoju(numer)} ({odkryte})  <-- jestes tutaj");
        else
            Console.WriteLine($"{numer}. {NazwaPokoju(numer)} ({odkryte})");

    }
    Console.WriteLine("");
}

void Rozejrzyj()
{

    Console.WriteLine("\nJestes w: " + NazwaPokoju(pokoj));

    if (pokoj == 1 && !karta)
        Console.WriteLine("\tKarta jest na biurku");
    else if (pokoj == 1)
        Console.WriteLine("\tKarta została już podniesiona");
    else if (pokoj == 2 && !bezpiecznik && !zasilanie)
        Console.WriteLine("\tBezpiecznik jest na półce");
    else if (pokoj == 2)
        Console.WriteLine("\tBezpiecznik został już podniesiony");
    else if (pokoj == 3 && !zasilanie)
        Console.WriteLine("\tZasilanie nie zostało podłączone");
    else if (pokoj == 3)
        Console.WriteLine("\tZasilanie zostało podłączone");
    else if (pokoj == 4 && zasilanie && karta)
        Console.WriteLine("\tMasz kartę i zasilanie");
    else if (pokoj == 4 && karta)
        Console.WriteLine("\tMasz kartę ale nie masz zasilania");
    else if (pokoj == 4 && zasilanie)
        Console.WriteLine("\tNie masz karty ale masz zasilanie");
    else if (pokoj == 4)
        Console.WriteLine("\tNie masz ani karty, ani zasilania");
    else
        Console.WriteLine("\tnie wiem");
    Console.WriteLine("");
}

// SEKCJA B: RUCH. Wlasciciel: osoba B.
void Idz(string kierunek)
{
    if(koniec) return;
    
    int nastepnyPokoj = pokoj;

    if(kierunek == "lewo") nastepnyPokoj--;
    else if(kierunek == "prawo") nastepnyPokoj++;
    else
    {
        Console.WriteLine("Zly kierunek!");
        return;
    }

    if(nastepnyPokoj < 1 || nastepnyPokoj > 4) 
    {
        Console.WriteLine($"Jestes na granicy! Nie możesz isc w {kierunek}");
        licznikOdrzuconychProbRuchu++;
        return;
    }

    pokoj = nastepnyPokoj;
    dziennik.Odwiedz(pokoj);
    dziennik.DodajWpis( $"Ruch w {kierunek} do pomieszczenia {NazwaPokoju(nastepnyPokoj)}");
    Rozejrzyj();
    ZakonczTure();
}

// SEKCJA C: PRZEDMIOTY I WYGRANA. Wlasciciel: osoba C.
// Dodanie wpisow w liniach: 335, 348, 370, 391
void Akcja(string co)
{
    if(koniec)
    {
        Console.WriteLine("Gra została zakończona.");
        return; 
    }

    if(co == "karta")
    {
        if(pokoj != 1 || karta)
        {
            Console.WriteLine("Tutaj nie ma karty do zebrania.");
            return;
        }
        karta = true;
        Console.WriteLine("Zabierasz karte.");

        dziennik.DodajWpis("Zabranie karty");
        ZakonczTure();
        return;
    }
    if(co == "bezpiecznik")
    {
        if(pokoj != 2 || bezpiecznik || zasilanie)
        {
            Console.WriteLine("Tutaj nie ma bezpiecznika do zebrania.");
            return;
        }
        bezpiecznik = true;
        Console.WriteLine("Zabierasz bezpiecznik.");
        dziennik.DodajWpis("Zabranie bezpiecznika");
        ZakonczTure();
        return;
    }
    if(co == "napraw")
    {
        if(pokoj != 3)
        {
            Console.WriteLine("Nie możesz naprawić zasilania w tym pokoju.");
            return;
        }else if(zasilanie)
        {
            Console.WriteLine("Zasilanie jest włączone.");
            return;
        }else if (!bezpiecznik)
        {
            Console.WriteLine("Nie masz bezpiecznika.");
            return;
        }
        bezpiecznik = false;
        zasilanie = true;
        
        dziennik.DodajWpis("Naprawa zasilania");
        Console.WriteLine("Naprawiasz zasilanie!");

        ZakonczTure();
        return;
    }
    if(co == "wyjdz")
    {
        if(pokoj != 4)
        {
            Console.WriteLine("Nie możesz wyjść z tego pokoju.");
            return;
        }else if (!karta)
        {
            Console.WriteLine("Nie masz karty, nie możesz wyjść.");
            return;
        }else if (!zasilanie)
        {
            Console.WriteLine("Zasilanie jest wyłączone, nie możesz wyjść.");            
            return;
        }

        dziennik.DodajWpis("Wyjście i wygrana");
        wygrana = true;
        koniec = true;
        ZakonczTure();
    }
}

string PasekEnergii()
{
    string tekst = "";
    for(int i = 0; i < MAKS_ENERGIA; i++)
    {
        if (i < energia)
            tekst += "#";
        else 
            tekst += ".";
    }
    return tekst;
}

string OcenaEnergii(int wartosc)
{
    if(wartosc<0 || wartosc>MAKS_ENERGIA)
        return "Nieprawidłowa wartość";
    else if(wartosc == 0)
        return "Brak energii";
    else if(wartosc<=3)
        return "Niski zapas";
    else if(wartosc<=6)
        return "Uważaj na zbędne ruchy";
    else 
        return "Duży zapas";
}
