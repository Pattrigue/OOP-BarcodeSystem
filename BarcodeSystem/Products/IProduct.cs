namespace BarcodeSystem.Products
{
    public interface IProduct
    {
        uint Id { get; }
        string Name { get; }
        decimal Price { get; }
        bool IsActive { get; set; }
        bool CanBeBoughtOnCredit { get; set; }
    }
}