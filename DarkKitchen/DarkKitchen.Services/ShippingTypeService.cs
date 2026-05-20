using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.ShippingTypeDTOs;

namespace DarkKitchen.Services;

public class ShippingTypeService(IShippingTypeRepository shippingTypeRepository) : IShippingTypeService
{
    private readonly IShippingTypeRepository _shippingTypeRepository = shippingTypeRepository;

    public List<ShippingTypeResponseDto> GetAll()
    {
        throw new NotImplementedException();
    }

    public ShippingTypeResponseDto GetById(int id)
    {
        throw new NotImplementedException();
    }

    public ShippingTypeResponseDto Create(ShippingTypeDto dto)
    {
        if(string.IsNullOrEmpty(dto.name))
        {
            throw new BadRequestException("Shipping type name cannot be empty.");
        }

        if(dto.price <= 0)
        {
            throw new BadRequestException("Shipping type price must be greater than zero.");
        }

        var shippingType = new ShippingType
        {
            Name = dto.name,
            Price = dto.price
        };

        _shippingTypeRepository.Add(shippingType);

        return new ShippingTypeResponseDto(shippingType.Id, shippingType.Name, shippingType.Price);
    }

    public ShippingTypeResponseDto Update(int id, ShippingTypeDto dto)
    {
        throw new NotImplementedException();
    }
}
