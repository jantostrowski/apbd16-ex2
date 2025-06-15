namespace apbd16_ex2.Models;

public abstract class Container
{
    public double MassContainer { get; protected set; }
    public double MaxCargo { get; protected set; }
    public double CurrCargo { get; protected set; }
    public double Height { get; protected set; }
    public double Depth { get; protected set; }
    
    public string SerialNumber { get; protected set; } = null!;
    public abstract string Type { get; protected set; }
    
    private static int serialNumber = 1;
    
    public Container (double massContainer, double maxCargo, double currCargo, double height, double depth)
    {
        if (massContainer <= 0)
            throw new ArgumentException("Zadeklaruj masę większą niż 0");
        if (maxCargo <= 0)
            throw new ArgumentException("Zadeklaruj maks. masę ładunku większą niż 0");
        if (currCargo < 0)
            throw new ArgumentException("Zadeklaruj aktualną masę ładunku mniejszą niż maksymalna");
        if (currCargo > maxCargo)
            throw new Utils.OverfillException("Przekroczono dopuszczalną ładowność kontenera");
        if (height <= 0 || depth <= 0)
            throw new ArgumentException("Zadeklaruj wymiary większe niż 0");
        
        MassContainer = massContainer;
        MaxCargo = maxCargo;
        CurrCargo = currCargo;
        Height = height;
        Depth = depth;
    }

    protected void GenerateSerialNumber()
    {
        SerialNumber = $"KON-{Type}-{serialNumber++}";
    }

    public virtual void Fill(double cargoMass)
    {
        if (CurrCargo + cargoMass > MaxCargo)
        {
            throw new Utils.OverfillException("Przekroczono dopuszczalną ładowność kontenera");
        }

        CurrCargo += cargoMass;
    }

    public virtual void Empty()
    {
        CurrCargo = 0;
    }

    public override string ToString()
    {
        return $"Kontener {SerialNumber} | Masa pustego: {MassContainer} kg | Masa ładunku: {CurrCargo}/{MaxCargo} kg";
    }
}