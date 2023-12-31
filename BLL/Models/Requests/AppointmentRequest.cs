﻿namespace BLL.Models.Requests
{
    public class AppointmentRequest
    {
        public DateTime CreatedDate { get; set; }
        public DateTime AppointedTime { get; set; }
        public bool Confirmed { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
