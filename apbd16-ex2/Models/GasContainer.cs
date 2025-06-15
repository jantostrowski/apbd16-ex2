namespace apbd16_ex2.Models;

public class GasContainer : Container
{
    private const string TYPE_GAS = "G";
    public override string Type { get; protected set; }

    public double Pressure { get; private set; }

    public GasContainer(double massContainer, double maxCargo, double currCargo, double height, double depth,
        double pressure)
        : base(massContainer, maxCargo, currCargo, height, depth)
    {
        Type = TYPE_GAS;
        Pressure = pressure;
        GenerateSerialNumber();
        
        if (pressure <= 0)
            throw new ArgumentException("Nieprawidłowa wartość ciśnienia w kontenerze");
    }
    
    public override void Empty()
    {
        CurrCargo = MaxCargo * 0.05;
    }
    
    public override string ToString()
    {
        return base.ToString() + $" | Ciśnienie: {Pressure} atm";
    }
}