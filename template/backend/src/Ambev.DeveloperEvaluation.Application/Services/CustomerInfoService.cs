using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Services.Interface;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Services;

public class CustomerInfoService : ICustomerInfoService
{
    private readonly IUserRepository _userRepository;

    public CustomerInfoService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CustomerInfoDto> GetCustomerInfoAsync(Guid customerId)
    {
        var customer = await _userRepository.GetByIdAsync(customerId);

        if (customer is null)
            throw new KeyNotFoundException("Customer not found");

        return new CustomerInfoDto(
            customer.Id,
            customer.Username,
            customer.Email,
            customer.Phone,
            customer.Role.ToString());
    }
}
