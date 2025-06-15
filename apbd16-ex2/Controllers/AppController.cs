using apbd16_ex2.Models;
using apbd16_ex2.Utils;

namespace apbd16_ex2.Controllers;

public class AppController
{
    public static List<Ship> ships = new();
    public static List<Container> containers = new();

    public static void Run(List<Ship> shipsParam, List<Container> containersParam)
    {
        ships = shipsParam;
        containers = containersParam;

        while (true)
        {
            Console.WriteLine("\n=== Polska Żegluga Morska P.P. ===");
            
            Console.WriteLine("==> Lista statków: <==");
            if (ships.Count == 0)
                Console.WriteLine("Brak");
            else
                foreach (var s in ships) Console.WriteLine($"- {s.Name}");
            
            Console.WriteLine("=> Lista kontenerów: <=");
            if (containers.Count == 0)
                Console.WriteLine("Brak");
            else
                foreach (var c in containers) Console.WriteLine($"- {c.SerialNumber}");

            Console.WriteLine("\nMożliwe akcje (podaj liczbę):");
            Console.WriteLine("1. Dodaj statek");
            if (ships.Count > 0) Console.WriteLine("2. Usuń statek");
            Console.WriteLine("3. Dodaj kontener");
            
            if (containers.Count > 0)
            {
                Console.WriteLine("4. Napełnij kontener");
                Console.WriteLine("5. Opróżnij kontener");
                Console.WriteLine("6. Załaduj kontener na statek");
                Console.WriteLine("7. Zdejmij kontener ze statku");
                Console.WriteLine("8. Przenieś kontener między statkami");
                Console.WriteLine("9. Usuń kontener");
            }
            if (ships.Count > 0)
            {
                Console.WriteLine("10. Informacje o statku");
            }
            Console.WriteLine("X. Wyjdź");

            Console.Write("Wybór: ");
            string input = Console.ReadLine()?.ToLower();

            if (input == "1") NewShip();
            else if (input == "2" && ships.Count > 0) DeleteShip();
            else if (input == "3") NewContainer();
            else if (input == "4" && containers.Count > 0) FillContainer();
            else if (input == "5" && containers.Count > 0) EmptyContainer();
            else if (input == "6" && containers.Count > 0) DockContainer();
            else if (input == "7" && containers.Count > 0) UndockContainer();
            else if (input == "8" && containers.Count > 0) MoveContainer();
            else if (input == "9" && containers.Count > 0) DeleteContainer();
            else if (input == "10" && ships.Count > 0) ShipInfo();
            else if (input == "x") break;
        }
    }

    static void NewShip()
    {
        Console.Write("Podaj nazwę statku: ");
        string name = Console.ReadLine()?.Trim();

        Console.Write("Podaj maks. prędkość (kt): ");
        if (!double.TryParse(Console.ReadLine(), out double maxSpeed)) return;
        Console.Write("Podaj maks. liczbę kontenerów: ");
        if (!int.TryParse(Console.ReadLine(), out int maxContainerCount)) return;
        Console.Write("Podaj maks. ładowność (ton): ");
        if (!double.TryParse(Console.ReadLine(), out double maxContainerWeight)) return;

        try
        {
            var ship = new Ship(name, maxSpeed, maxContainerCount, maxContainerWeight);
            ships.Add(ship);
            Console.WriteLine($"Dodano statek: {ship.Name}");
            ship.PrintInfo();
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Podano błędne dane: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Wystąpił błąd przy dodawaniu nowego statku: {e.Message}");
        }
        finally
        {
            Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
            Console.ReadKey(true);
        }
    }
    
    static void DeleteShip()
    {
        Console.Write("Podaj nazwę statku do usunięcia: ");
        string name = Console.ReadLine();
        var ship = ships.FirstOrDefault(s => s.Name == name);

        if (ship != null)
        {
            ships.Remove(ship);
            Console.WriteLine($"Statek {ship.Name} został usunięty");
        }
        else
        {
            Console.WriteLine($"Nie znaleziono statku o nazwie {name}");
        }
        
        Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
        Console.ReadKey(true);
    }

    static void NewContainer()
    {
        Console.WriteLine("Wybierz typ kontenera: (L - płyn, G - gaz, C - chłodnia)");
        string type = Console.ReadLine()?.Trim().ToUpper();

        if (type != "L" && type != "G" && type != "C")
        {
            Console.WriteLine("Nieprawidłowy typ kontenera. Dozwolone: L, G, C");
            Console.ReadKey(true);
            return;
        }

        Console.Write("Podaj masę pustego kontenera (kg): ");
        if (!double.TryParse(Console.ReadLine(), out double massContainer)) return;
        Console.Write("Podaj maks. ładowność (kg): ");
        if (!double.TryParse(Console.ReadLine(), out double maxCargo)) return;
        Console.Write("Podaj masę ładunku (kg): ");
        if (!double.TryParse(Console.ReadLine(), out double currCargo)) return;
        Console.Write("Podaj wysokość (cm): ");
        if (!double.TryParse(Console.ReadLine(), out double height)) return;
        Console.Write("Podaj głębokość (cm): ");
        if (!double.TryParse(Console.ReadLine(), out double depth)) return;

        Container toCreate = null;

        try
        {
            if (type == "L")
            {
                Console.Write("Określ czy ładunek jest niebezpieczny? (t/n): ");
                string input = Console.ReadLine()?.Trim().ToLower();
                bool hazardousContent;

                if (input == "true" || input == "t" || input == "tak" || input == "y")
                {
                    hazardousContent = true;
                }
                else if (input == "false" || input == "n" || input == "nie" || input == "f")
                {
                    hazardousContent = false;
                }
                else
                {
                    Console.WriteLine("Nie określono bezpieczeństwa ładunku");

                    Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
                    Console.ReadKey(true);
                    return;
                }

                toCreate = new LiquidContainer(massContainer, maxCargo, currCargo, height, depth, hazardousContent);
            }
            else if (type == "G")
            {
                Console.Write("Podaj utrzymywane ciśnienie w kontenerze (atm): ");
                if (!double.TryParse(Console.ReadLine(), out double pressure)) return;
                toCreate = new GasContainer(massContainer, maxCargo, currCargo, height, depth, pressure);
            }
            else if (type == "C")
            {
                Console.Write("Podaj temperaturę roboczą kontenera (st. C): ");
                if (!double.TryParse(Console.ReadLine(), out double temperature)) return;
                
                Console.Write("Podaj nazwę produktu chłodniczego: ");
                string productName = Console.ReadLine()?.Trim();

                Console.Write("Podaj maks. dop. temperaturę dla produktu (st. C): ");
                if (!double.TryParse(Console.ReadLine(), out double requiredTemp)) return;
                
                var product = new Product(productName, requiredTemp);
                toCreate = new RefrContainer(massContainer, maxCargo, currCargo, height, depth, temperature);
                ((RefrContainer)toCreate).AssignProduct(product);
            }

            if (toCreate != null)
            {
                containers.Add(toCreate);
                Console.WriteLine($"Dodano kontener {toCreate.SerialNumber}");
                Console.WriteLine(toCreate);
            }
        }
        catch (OverfillException e)
        {
            Console.WriteLine($"Przepełnienie kontenera: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Podano błędne dane: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Wystąpił błąd przy dodawaniu nowego kontenera: {e.Message}");
        }
        finally
        {
            Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
            Console.ReadKey(true);
        }
    }

    static void FillContainer()
    {
        Console.Write("Podaj numer kontenera do załadowania [np. KON-L-1]: ");
        string serial = Console.ReadLine();
        var toFill = containers.FirstOrDefault(c => c.SerialNumber == serial);
        if (toFill == null)
        {
            Console.WriteLine($"Nie znaleziono kontenera o numerze: {serial}");
        }
        
        Console.Write("Podaj masę ładunku do załadowania (kg): ");
        if (!double.TryParse(Console.ReadLine(), out double mass)) return;
        
        try
        {
            toFill.Fill(mass);
            Console.WriteLine($"Załadowano {mass} kg ładunku do kontenera: {toFill.SerialNumber}");

        }
        catch (OverfillException e)
        {
            Console.WriteLine($"Błąd: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Błąd: {e.Message}");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Błąd: {e.Message}");
        }
        
        Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
        Console.ReadKey(true);
    }

    static void EmptyContainer()
    {
        Console.Write("Podaj numer kontenera do opróżnienia [np. KON-L-1]: ");
        string serial = Console.ReadLine();
        var toEmpty = containers.FirstOrDefault(c => c.SerialNumber == serial);
        
        if (toEmpty == null)
        {
            Console.WriteLine($"Nie znaleziono kontenera o numerze: {serial}");
        }
        else
        {
            toEmpty.Empty();
            Console.WriteLine($"Kontener: {toEmpty.SerialNumber} został opróżniony");
        }
        
        Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
        Console.ReadKey(true);
    }

    static void DockContainer()
    {
        Console.WriteLine("Dostępne kontenery:");
        foreach (var container in containers)
        {
            Console.WriteLine($"- {container.SerialNumber}");
        }
        
        Console.Write("Podaj numer kontenera do włożenia [np. KON-L-1]: "); 
        string serial = Console.ReadLine();
        var toDock = containers.FirstOrDefault(c => c.SerialNumber == serial); 
        if (toDock == null) 
        { 
            Console.WriteLine($"Nie znaleziono kontenera o numerze: {serial}"); 
            return;
        }

        Console.WriteLine("\nDostępne statki:");
        foreach (var ship in ships)
        {
            Console.WriteLine($"- {ship.Name}");
        }
        Console.Write("Podaj nazwę statku: "); 
        string name = Console.ReadLine();
        var target = ships.FirstOrDefault(s => s.Name == name); 
        if (target == null) 
        { 
            Console.WriteLine($"Nie znaleziono statku o nazwie: {name}"); 
            return;
        }
        
        target.DockContainer(toDock);
        Console.WriteLine($"Kontener: {toDock.SerialNumber} został zadokowany na statku: {target.Name}");
        
        Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
        Console.ReadKey(true);
    }

    static void MoveContainer()
    {
        Console.Write("Podaj numer kontenera do przeniesienia [np. KON-L-1]: ");
        var serial = Console.ReadLine();
        var toMove = containers.FirstOrDefault(c => c.SerialNumber == serial);
        if (toMove == null)
        {
            Console.WriteLine($"Nie znaleziono kontenera o numerze {serial}");
            Console.ReadKey(true);
            return;
        }
        
        Console.Write("Z którego statku: ");
        var fromName = Console.ReadLine();
        var from = ships.FirstOrDefault(s => s.Name == fromName);
        if (from == null)
        {
            Console.WriteLine($"Nie znaleziono statku o nazwie: {fromName}");
            Console.ReadKey(true);
            return;
        }
        
        Console.Write("Na który statek: ");
        var targetName = Console.ReadLine();
        var target = ships.FirstOrDefault(s => s.Name == targetName);
        if (target == null)
        {
            Console.WriteLine($"Nie znaleziono statku o nazwie: {targetName}");
            Console.ReadKey(true);
            return;
        }
        
        from.RemoveContainer(toMove);
        target.DockContainer(toMove);
        Console.WriteLine($"Kontener: {toMove.SerialNumber} został przeniesiony ze statku: {from.Name} na statek: {target.Name}");
        
        Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
        Console.ReadKey(true);
    }

    static void UndockContainer()
    {
        Console.Write("Podaj numer kontenera do zdjęcia [np. KON-L-1]: ");
        var serial = Console.ReadLine();
        var toUndock = containers.FirstOrDefault(c => c.SerialNumber == serial);
        if (toUndock == null)
        {
            Console.WriteLine("Nie znaleziono kontenera o numerze.");
            Console.ReadKey(true);
            return;
        }
        
        Console.Write("Z którego statku: ");
        var fromName = Console.ReadLine();
        var from = ships.FirstOrDefault(s => s.Name == fromName);
        if (from == null)
        {
            Console.WriteLine($"Nie znaleziono statku o nazwie: {fromName}");
            Console.ReadKey(true);
            return;
        }
        
        if (!from.Containers.Contains(toUndock))
        {
            Console.WriteLine($"Statek: {from.Name} nie ma na pokładzie kontenera: {toUndock.SerialNumber}");
            Console.ReadKey(true);
            return;
        }

        from.RemoveContainer(toUndock);
        Console.WriteLine($"Kontener {toUndock.SerialNumber} został zdjęty ze statku {from.Name}.");
        
        Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
        Console.ReadKey(true);
    }
    
    static void DeleteContainer()
    {
        Console.Write("Podaj numer kontenera do usunięcia [np. KON-L-1]: ");
        var serial = Console.ReadLine();
        var toDelete = containers.FirstOrDefault(c => c.SerialNumber == serial);
        if (toDelete == null)
        {
            Console.WriteLine("Nie znaleziono kontenera o numerze.");
            Console.ReadKey(true);
            return;
        }
        
        string removedFrom = null;
        foreach (var ship in ships)
        {
            if (ship.Containers.Contains(toDelete))
            {
                ship.RemoveContainer(toDelete);
                removedFrom = ship.Name;
                break;
            }
        }

        containers.Remove(toDelete);

        if (removedFrom != null)
        {
            Console.WriteLine($"Kontener: {toDelete.SerialNumber} został usunięty i zdjęty ze statku: {removedFrom}");
        }
        else
        {
            Console.WriteLine($"Kontener: {toDelete.SerialNumber} został usunięty (nie był przypisany do żadnego statku)");
        }
        
        Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
        Console.ReadKey(true);
    }
    
    static void ShipInfo()
    {
        Console.Write("Podaj nazwę statku: ");
        var name = Console.ReadLine()?.Trim();
        
        string fixName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
        
        var ship = ships.FirstOrDefault(s => s.Name == fixName);
        if (ship != null)
        {
            ship.PrintInfo();
        }
        else
        {
            Console.WriteLine($"Nie znaleziono statku o nazwie: {name}");
        }
        
        Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
        Console.ReadKey(true);
    }
}