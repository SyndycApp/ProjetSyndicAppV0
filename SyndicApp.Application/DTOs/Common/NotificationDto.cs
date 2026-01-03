namespace SyndicApp.Application.DTOs.Common;

public record NotificationDto(
    Guid Id,
    string Titre,
    string Message,
    string Type,
    bool EstLue,
    DateTime DateCreation
);
