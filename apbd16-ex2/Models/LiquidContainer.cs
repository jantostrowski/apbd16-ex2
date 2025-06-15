namespace apbd16_ex2.Models;

public class LiquidContainer : Container, Utils.IHazardNotifier
{
    private const string TYPE_LIQUID = "L";
    public override string Type { get; protected set; }
    
    public bool HazardousContent { get; private set; }

    public LiquidContainer(double massContainer, double maxCargo, double currCargo, double height, double depth,
        bool hazardousContent)
        : base(massContainer, maxCargo, currCargo, height, depth)
    {
        Type = TYPE_LIQUID;
        HazardousContent = hazardousContent;
        GenerateSerialNumber();
        
        double maxAllowed = hazardousContent ? maxCargo * 0.5 : maxCargo * 0.9;
        if (currCargo > maxAllowed)
        {
            var isHazardous = hazardousContent ? "niebezpiecznego" : "zwykłego";
            HazardWarning($"Niepowodzenie załadowania {isHazardous} ładunku do kontenera: ");
            throw new Utils.OverfillException($"Za duża masa ładunku ({currCargo} kg) dla kontenera. Maksymalna dozwolona: {maxAllowed} kg");        
        }
    }

    public override void Fill(double cargoMass)
    {
        double maxAllowed = HazardousContent ? MaxCargo * 0.5 : MaxCargo * 0.9;
        if (cargoMass > maxAllowed)
        {
            var isHazardous = HazardousContent ? "niebezpiecznego" : "zwykłego";
            HazardWarning($"Niepowodzenie załadowania {isHazardous} ładunku do kontenera: ");
            throw new Utils.OverfillException($"Za duża masa ładunku ({cargoMass} kg) dla kontenera. Maksymalna dozwolona: {maxAllowed} kg");
        }
        
        base.Fill(cargoMass);
    }

    public void HazardWarning(string message)
    {
        Console.WriteLine($"Uwaga! {message} - {SerialNumber}");
    }
    
    public override string ToString()
    {
        return base.ToString() + $" | Niebezpieczny: {HazardousContent}";
    }
}