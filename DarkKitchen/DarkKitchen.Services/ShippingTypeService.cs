using DarkKitchen.Domain.Entities;
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
