namespace TrackCure.Dtos
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Designation { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string LicenseNo { get; set; } = string.Empty;
        public int Experience { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

      
    }
}
