namespace apbd16_ex2.Models;

public class Ship
{
    public string Name { get; set; }
    public double MaxSpeed { get; set; }
    public int MaxContainerCount { get; set; }
    public double MaxContainerWeight { get; set; }
    
    public List<Container> Containers { get; } = new ();

    public Ship(string name, double maxSpeed, int maxContainerCount, double maxContainerWeight)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nazwa statku musi być tekstem");
        if (maxSpeed <= 0)
            throw new ArgumentException("Zadeklaruj maks. prędkość większą niż 0");
        if (maxContainerCount <= 0)
            throw new ArgumentException("Zadeklaruj maks. liczbę kontenerów większą niż 0");
        if (maxContainerWeight <= 0)
            throw new ArgumentException("Zadeklaruj maks. dopuszczalną masę statku większą niż 0");
        
        Name = name;
        MaxSpeed = maxSpeed;
        MaxContainerCount = maxContainerCount;
        MaxContainerWeight = maxContainerWeight;
    }

    public void DockContainer(Container toDock, bool silent = false)
    {
        if (Containers.Count >= MaxContainerCount)
        {
            Console.WriteLine("Statek przeładowany - przekroczono liczbę kontenerów");
            return;
        }

        double totalWeight = Containers.Sum(x => x.MassContainer + x.CurrCargo);
        
        if (totalWeight + toDock.MassContainer + toDock.CurrCargo > MaxContainerWeight * 1000)
        {
            Console.WriteLine("Statek przeładowany - przekroczono masę kontenerów");
            return;
        }
        
        Containers.Add(toDock);
        if (!silent)
            Console.WriteLine($"Załadowano kontener {toDock.SerialNumber} na statek {Name}");
    }

    public void RemoveContainer(Container toRemove)
    {
        if (Containers.Contains(toRemove))
        {
            Containers.Remove(toRemove);
            Console.WriteLine($"Kontener {toRemove.SerialNumber} został zdjęty ze statku {Name}.");
        }
        else
        {
            Console.WriteLine($"Kontener {toRemove.SerialNumber} nie znajduje się na {Name}.");
        }
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Statek: {Name} | Prędkość maks.: {MaxSpeed} kt | Liczba kontenerów: {Containers.Count}/{MaxContainerCount} | Ładowność maks.: {MaxContainerWeight} ton");
       
        foreach (var container in Containers)
        {
            string info = container.ToString();
            Console.WriteLine($" - {info}");
        }
    }
}

