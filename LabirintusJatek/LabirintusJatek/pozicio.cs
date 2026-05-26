namespace labirintus_jatek
{
    internal class pozicio
    {
        public int sor;
        public int oszlop;

        public pozicio(int sor, int oszlop)
        {
            this.sor = sor;
            this.oszlop = oszlop;
        }

        public string kulcs()
        {
            return sor + ":" + oszlop;
        }
    }
}
