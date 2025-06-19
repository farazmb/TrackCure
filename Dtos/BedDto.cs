namespace TrackCure.Dtos
{
    public class BedDto
    {
        public int BedId { get; set; }
        public string BedNumber { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
