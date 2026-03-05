namespace HackTech_Service.Models
{
    //Reprezintă un punct pe harta SVG (ID, coordonate X, Y).
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Coordonatele care trebuie să se potrivească cu cele din SVG
        public double X { get; set; }
        public double Y { get; set; }

        public bool IsEntry { get; set; } // Dacă e intrare într-un magazin
    }
}
