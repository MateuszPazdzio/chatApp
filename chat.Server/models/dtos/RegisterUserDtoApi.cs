﻿namespace chat.Server.models.dtos
{
    public class RegisterUserDtoApi
    {
        public string UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        //public string Nationality { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordVerification { get; set; }
    }
}