namespace TrackCure.Dtos
{
    public class DoctorAvailabilityDto
    {
        public int AvailabilityId { get; set; }
        public int UserId { get; set; }
        public DateTime AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
