using System;

namespace LeavePlannerApp2.Models
{
    public class Notificaitions
    {
        public int NotificationsId { get; set; }
        public MyUserStore User { get; set; }
        public DateTimeOffset DateApprovedOrRejected { get; set; }

    }
}