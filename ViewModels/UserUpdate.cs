﻿namespace ProjectCMS.ViewModels
{
    public class UserUpdate:UpdateAvatar
    {
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DoB { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public IFormFile Image { get; set; }
        public int DepartmentID { get; set; }
    }
}
