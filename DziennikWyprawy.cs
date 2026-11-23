class DziennikWyprawy
{
    private string[] _wpisy = {"","",""};
    private bool[] _odwiedzonePokoje = new bool[4];

    public int LiczbaOdwiedzonych 
    {
        get
        {
            int licznik = 0;
            foreach (bool item in _odwiedzonePokoje)
            {
                if (item)
                {
                    licznik++;
                }
            }
            return licznik;
        }
    }

    public DziennikWyprawy()
    {
        Reset();
    }

    public void Reset()
    {
        _odwiedzonePokoje[0] = true;
        for (int i = 1; i < 4; i++)
        {
            _odwiedzonePokoje[i] = false;
        }
        for (int i = 0; i < 3; i++)
        {
            _wpisy[i] = "";
        }
        //todo liczba wpisow
    }
    public void Odwiedz(int numer)
    {
        if (numer > 0 && numer < 5 && !_odwiedzonePokoje[numer-1])
        {
            _odwiedzonePokoje[numer-1] = true;
        }
    }
    public bool CzyOdwiedzono(int numer)
    {
        if (numer > 0 && numer < 5)
        {
            return _odwiedzonePokoje[numer-1];
        }
        return false;
    }
}
