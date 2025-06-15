using apbd16_ex2.Models;
using apbd16_ex2.Controllers;

class Program
{
    static void Main(string[] args)
    {
        var ships = new List<Ship>();
        var containers = new List<Container>();
        
        var p1 = new Product("Banany", 13);
        var p2 = new Product("Ryby", 2);
        
        var kurpie = new Ship("Kurpie", 20, 120, 200000);
        ships.Add(kurpie);
        
        var lubie = new Ship("Lubie", 21, 140, 220000);
        ships.Add(lubie);

        var c1 = new LiquidContainer(4000,24000, 0, 259, 243, false);
        c1.Fill(21600);

        var c2 = new GasContainer(3600,20000, 0, 259, 243, 18.0);
        c2.Fill(16000);
        
        var c3 = new RefrContainer(3700,25000, 0, 289, 243, 13);
        c3.AssignProduct(p1);
        c3.Fill(20000);
        
        var c4 = new LiquidContainer(4100,24000, 0, 259, 243, true);
        c4.Fill(12000);

        var c5 = new GasContainer(3900,21000, 0, 259, 243, 21.5);
        c5.Fill(15000);

        var c6 = new RefrContainer(3750,25000, 0, 289, 243, 1);
        c6.AssignProduct(p2);
        c6.Fill(21000);
        
        containers.AddRange(new Container[] { c1, c2, c3 });
        kurpie.DockContainer(c1, true);
        kurpie.DockContainer(c2, true);
        kurpie.DockContainer(c3, true);

        containers.AddRange(new Container[] { c4, c5, c6 });
        lubie.DockContainer(c4, true);
        lubie.DockContainer(c5, true);
        lubie.DockContainer(c6, true);
        
        AppController.Run(ships, containers);
    }
}