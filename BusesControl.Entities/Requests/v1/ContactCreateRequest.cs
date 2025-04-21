namespace BusesControl.Entities.Requests.v1
{
    public class ContactCreateRequest
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
