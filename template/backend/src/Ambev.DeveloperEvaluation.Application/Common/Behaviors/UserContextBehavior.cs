using Ambev.DeveloperEvaluation.Application.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ambev.DeveloperEvaluation.Application.Common.Behaviors;

public class UserContextBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IUserContextRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICustomerInfoService _customerInfoService;

    public UserContextBehavior(
        IHttpContextAccessor httpContextAccessor, ICustomerInfoService customerInfoService)
    {
        _httpContextAccessor = httpContextAccessor;
        _customerInfoService = customerInfoService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var customerIdClaim =
            _httpContextAccessor.HttpContext?.User.FindFirst(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        if (customerIdClaim == null || !Guid.TryParse(customerIdClaim.Value, out var customerId))
            throw new UnauthorizedAccessException("Invalid customer ID in token");

        request.Customer = await _customerInfoService.GetCustomerInfoAsync(customerId);
        return await next();
    }
}
