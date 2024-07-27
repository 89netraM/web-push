using System;

namespace WebPushChat.Models;

public record MessageModel(Guid SenderId, string Sender, string Message);
