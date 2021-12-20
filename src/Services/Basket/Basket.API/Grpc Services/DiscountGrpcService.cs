using Discount.GRPC.Protos;

namespace Basket.API.Grpc_Services;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _discountProtoService = discountProtoService;
    }

    public async Task<CouponModel> GetDiscount(string productName)
    {
        var discountRequest = new GetDiscountRequest {ProductName = productName};

        return await _discountProtoService.GetDiscountAsync(discountRequest);
    }
}