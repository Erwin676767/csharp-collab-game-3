class DziennikWyprawy
{
    private string[] _wpisy = {"","",""};
    private bool[] _odwiedzonePokoje = new bool[4];
    public int LiczbaWpisow { get; private set; }

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
    public void DodajWpis(string? tresc)
    {
        string? nowaTresc = tresc.Trim();
        if(nowaTresc == "" || nowaTresc == null)
        {
            return;
        }
        
        for(int i=0;i<this._wpisy.Length;i++)
        {
            
            if(this._wpisy[i] == "")
            {
                this._wpisy[i] = nowaTresc;
                this.LiczbaWpisow+=1;
                return;
            }
            
        }
        this._wpisy[0]=this._wpisy[1];
        this._wpisy[1]=this._wpisy[2];
        this._wpisy[2]=nowaTresc;
        

    }
    
    public string PobierzWpis(int indeks){
        return this._wpisy[indeks];
    }
    public void Reset()
    {
        this._odwiedzonePokoje[0] = true;
        for (int i = 1; i < 4; i++)
        {
            this._odwiedzonePokoje[i] = false;
        }
        for (int i = 0; i < 3; i++)
        {
            this._wpisy[i] = "";
        }
        this.LiczbaWpisow = 0;
        
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
