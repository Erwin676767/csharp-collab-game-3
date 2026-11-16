# Założenia
- dodajemy klase DziennikWyprawy
- zmieniamy odwołanie w Program.cs z `csharp pokoje[n]` na `DziennikWyprawy.PobierzPokoj(n)`. Jest to potrzebne gdy chcemy zachowac zalezna dlugosc `_odwiedzonePokoje[]`
- W Program.cs często pojawia się wyrażenie pokoje.Length. Migracja tablicy _pokoje[] będzie dość żmudnym procesem, mam podejrzenie że to może być w collabie 4. Chcemy robić do przodu? Na początku może zróbmy Minimum Viable Product (rozmiar _odwiedzonePokoje[] wpisany z palca), jeśli starczy czasu to przemigrujemy też pokoje. Migracja pokoi nie jest wspomniana w żadnym z rezultatów (R3.1 - R3.6)
- Save dodatkow na branchu dodatki i usuniecie z mastera wszystkich kolidujacych np autotesty? Nie sa wymagane


| Składowa publiczna | Umowa |
|---|---|
| `DziennikWyprawy()` | Konstruktor bez parametrów ustanawia stan taki jak Reset. - wywołuje Reset(). W taki sposób eliminujemy redundancję |
| `int LiczbaOdwiedzonych { get; }` | Tylko odczyt; wynik obliczany z prywatnych flag przez pętlę. Nie jest drugim, niezależnie aktualizowanym licznikiem. Dodajemy ciało gettera, które zlicza _odwiedzonePokoje |
| `int LiczbaWpisow { get; private set; }` | Liczba zajętych pozycji dziennika 0–3; klient nie może jej przypisać. |
| `void Odwiedz(int numer)` | Dla 1–4 oznacza pokój; ponowna wizyta niczego nie zwiększa. Dla numeru spoza zakresu nic nie zmienia. |
| `bool CzyOdwiedzono(int numer)` | Odczyt flagi; dla numeru spoza 1–4 zwraca false. |
| `void DodajWpis(string? tresc)` | Ignoruje null, pusty tekst i same spacje. Poprawny tekst zapisuje po Trim. Przy pełnych 3 wpisach usuwa najstarszy, zachowując kolejność. |
| `string PobierzWpis(int indeks)` | Zajęte wpisy mają indeksy 0..LiczbaWpisow-1, od najstarszego. Dla innych indeksów zwraca pusty napis; nie rzuca wyjątku w tym ćwiczeniu. |
| `void Reset()` | Cztery flagi odkryć: tylko pokój 1 odwiedzony; dziennik pusty, LiczbaWpisow=0. Czyści także stare teksty w tablicy. Nie tworzy drugiego obiektu. |
| `string[] _wpisy = {"","",""} ` | Tablica ktora przechowuje 3 ostatnie wpisy.|
| `bool[] _odwiedzonePokoje = new bool[_pokoje.Length];` | Przenosimy do klasy, bo tutaj zliczamy liczbe odwiedzonych - patrz 2 składowa|
| `string[] _pokoje = { "Recepcja", "Magazyn", "Serwerowania", "Wyjście"};` | Tablica z nazwami pokoi, numerowanych indeksami po kolei. Przenosimy do klasy gdy chcemy zachowac zaleznosc rozmiaru tablicy _odwiedzonePokoje|
| `string PobierzPokoj(int indeks)` | indeksy od 0 do 3, zwraca pokoj. Jest to 'getter' do tablicy _pokoje[], w taki sposób robimy refactor i tablica będzie elementem klasy.|


## 3. Wymagany rezultat i kontrakt

- **R3.1 - przejęcie:** nowa osoba uruchamia docelowe repo i wyjaśnia przepływ polecenia; zespół aktualizuje przekazanie.
- **R3.2 - obiekt:** prywatne odwiedziny i dziennik należą do DziennikWyprawy. Każda instancja ma własne dane; bez static i bez wystawiania tablic.
- **R3.3 - funkcja:** `dziennik` pokazuje najwyżej trzy ostatnie udane czynności, od najstarszej do najnowszej. Pusta historia ma czytelny komunikat.
- **R3.4 - refaktoryzacja:** odkrycia z csharp-collab-2 czytają i aktualizują obiekt. Po migracji w Program.cs nie ma drugiej tablicy odkryć ani licznika będącego konkurencyjnym źródłem prawdy.
- **R3.5 - poprawki:** prawidłowy reset tej samej instancji, ochrona granic i pustych wpisów, brak wpisów po odmowach, brak dodatkowego kosztu.
- **R3.6 - współpraca:** każdy pisze kod, recenzuje i przechodzi próbę samodzielną; główna gałąź przechodzi regresję; tag `collab-03-core-v1`.