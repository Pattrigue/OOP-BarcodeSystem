namespace DashSystem.Data.Products
{
    public interface IProduct
    {
        uint Id { get; }
        string Name { get; }
        decimal Price { get; }
        bool IsActive { get; }
        bool CanBeBoughtOnCredit { get; }
    }
}