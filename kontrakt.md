# Założenia
- dodajemy klase DziennikWyprawy

| Składowa publiczna | Umowa |
|---|---|
| `DziennikWyprawy()` | Konstruktor bez parametrów ustanawia stan taki jak Reset. - wywołuje Reset() |
| `int LiczbaOdwiedzonych { get; }` | Tylko odczyt; wynik obliczany z prywatnych flag przez pętlę. Nie jest drugim, niezależnie aktualizowanym licznikiem. |
| `int LiczbaWpisow { get; private set; }` | Liczba zajętych pozycji dziennika 0–3; klient nie może jej przypisać. |
| `void Odwiedz(int numer)` | Dla 1–4 oznacza pokój; ponowna wizyta niczego nie zwiększa. Dla numeru spoza zakresu nic nie zmienia. |
| `bool CzyOdwiedzono(int numer)` | Odczyt flagi; dla numeru spoza 1–4 zwraca false. |
| `void DodajWpis(string? tresc)` | Ignoruje null, pusty tekst i same spacje. Poprawny tekst zapisuje po Trim. Przy pełnych 3 wpisach usuwa najstarszy, zachowując kolejność. |
| `string PobierzWpis(int indeks)` | Zajęte wpisy mają indeksy 0..LiczbaWpisow-1, od najstarszego. Dla innych indeksów zwraca pusty napis; nie rzuca wyjątku w tym ćwiczeniu. |
| `void Reset()` | Cztery flagi odkryć: tylko pokój 1 odwiedzony; dziennik pusty, LiczbaWpisow=0. Czyści także stare teksty w tablicy. Nie tworzy drugiego obiektu. |
| `string[] _wpisy = {"","",""} ` | Tablica ktora przechowuje 3 wpisy.|
| `bool[] _odwiedzonePokoje = new bool[4];` | przenosimy do klasy|
| `string[] _pokoje = { "Recepcja", "Magazyn", "Serwerowania", "Wyjście"};` | pokoje, przenosimy do klasy|
| `string PobierzPokoj(int indeks)` | indeksy od 0 do 3, zwraca pokoj.|
