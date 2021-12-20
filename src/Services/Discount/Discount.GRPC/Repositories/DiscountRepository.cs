using Dapper;
using Discount.GRPC.Repositories;
using Discount.GRPC.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Npgsql;

namespace Discount.GRPC.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE LOWER(ProductName) = LOWER(@ProductName)", new { ProductName = productName});
        
        return coupon ?? new Coupon {ProductName = "No Discount", Amount = 0, Description = "No Discount Desc"};
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var effected = await connection.ExecuteAsync
        ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
            new {ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount});
        
        return effected != 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var effected = await connection.ExecuteAsync
        ("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id = @Id",
            new {ProductName=coupon.ProductName, Description=coupon.Description, Amount=coupon.Amount, @Id=coupon.Id});
        
        return effected != 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        
        var effected = await connection.ExecuteAsync
        ("DELETE FROM Coupon WHERE ProductName = @ProductName",
            new {ProductName = productName});
        
        return effected != 0;
    }
}