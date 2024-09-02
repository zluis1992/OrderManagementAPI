namespace Domain.Dto;

public record OrderFilterDto(Guid? Id = null, decimal? MinPrice = null, decimal? MaxPrice = null);
