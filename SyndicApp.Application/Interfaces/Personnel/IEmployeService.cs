using SyndicApp.Application.DTOs.Personnel;

public interface IEmployeService
{
    Task<EmployeDetailsDto> GetEmployeDetailsAsync(Guid userId);
    Task UpdateEmployeAsync(Guid userId, EmployeUpdateDto dto);
}
