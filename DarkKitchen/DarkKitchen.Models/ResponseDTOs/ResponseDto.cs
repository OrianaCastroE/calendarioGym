namespace DarkKitchen.Models.ResponseDTOs;

public readonly record struct ResponseDto(bool executionSuccessful, string message);
