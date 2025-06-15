namespace apbd16_ex2.Models;

public class RefrContainer : Container
{
    private const string TYPE_REFRIGERATED = "C";
    public override string Type { get; protected set; }
    
    public Product? CurrentProduct { get; set; }
    public double Temperature { get; set; }
    
    public RefrContainer(double massContainer, double maxCargo, double currCargo, double height, double depth,
        double temperature)
        : base(massContainer, maxCargo, currCargo, height, depth)
    {
        Type = TYPE_REFRIGERATED;
        Temperature = temperature;
        GenerateSerialNumber();
    }
  
    public override void Fill(double cargoMass)
    {
        if (CurrentProduct == null)
            throw new InvalidOperationException("Przed załadowaniem należy przypisać produkt do kontenera chłodniczego");

        base.Fill(cargoMass);
    }
    
    public void AssignProduct(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (product.RequiredTemperature < Temperature)
        {
            throw new ArgumentException($"Temperatura kontenera ({Temperature}st. C) przekracza dopuszczalną dla {CurrentProduct.Name} - maks. {CurrentProduct.RequiredTemperature} st. C");
        }

        CurrentProduct = product;
    }
    
    public override string ToString()
    {
        string productInfo = CurrentProduct != null ?  $" | Produkt: {CurrentProduct.Name}" : " | Produkt: brak";
        return base.ToString() + $" | Temperatura: {Temperature} st. C" + productInfo;
    }
}