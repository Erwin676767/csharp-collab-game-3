class DziennikWyprawy
{
    private string[] _wpisy = {"","",""};
    private bool[] _odwiedzonePokoje = new bool[4];
    public int LiczbaWpisow { get; private set; }
    public DziennikWyprawy()
    {
        Reset(); 
    }
    public void DodajWpis(string? tresc)
    {
        if(tresc == "" || tresc == null)
        {
            return;
        }

        string? nowaTresc = tresc.Trim();
        
        
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
        return this._wpisy[index];
    }
    public void Reset()
    {
        
    }
}
//DodajWpis/PobierzWpis i aktualizacja licznika historii;