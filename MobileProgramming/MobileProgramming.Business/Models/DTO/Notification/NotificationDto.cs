namespace MobileProgramming.Business.Models.DTO.Notification
{
    public record NotificationDto(
        int notificationId,
        int UserId,
        string Message,
        bool IsRead,
        DateTime CreatedAt
    );
}
