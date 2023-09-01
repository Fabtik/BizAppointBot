namespace BLL.Models.Responses
{
    public class AppointmentResponse
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime AppointedTime { get; set; }
        public bool Confirmed { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
