using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Infrastructure;
using System.Security.Claims;

namespace SyndicApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public NotificationsController(ApplicationDbContext db)
    {
        _db = db;
    }

    private Guid UserId =>
        Guid.Parse(User.FindFirstValue("uid")!);

    private bool IsSyndic =>
        User.IsInRole("Syndic");

    [HttpGet("me")]
    public async Task<IActionResult> GetMyNotifications()
    {
        var notifications = await _db.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == UserId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();

        return Ok(notifications);
    }


    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var notif = await _db.Notifications
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == UserId);

        if (notif == null)
            return NotFound();

        notif.IsRead = true;
        await _db.SaveChangesAsync();

        return Ok();
    }

    [Authorize(Roles = "Syndic")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllNotifications()
    {
        var notifications = await _db.Notifications
            .AsNoTracking()
            .OrderByDescending(n => n.SentAt)
            .Select(n => new
            {
                n.Id,
                n.Title,
                n.Message,
                n.IsRead,
                n.SentAt,
                n.UserId
            })
            .ToListAsync();

        return Ok(notifications);
    }
}
