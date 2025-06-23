namespace TrackCure.Models
{
    public class AssignedResource
    {
        public int AssignmentId { get; set; }
        public int BedId { get; set; } //FK
        public int EquipmentId { get; set; }   // FK
        public int PatientId { get; set; }   //FK
        public int UserId { get; set; }  // FK
        public DateTime StartTime { get; set; }
        public string Status { get; set; } = string.Empty; 
    }
}
