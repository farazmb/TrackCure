namespace TrackCure.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int AddedBy { get; set; } // Foreign Key from the User table
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string BillingStatus { get; set; } = string.Empty;
        public string Symptom { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
