﻿using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Server.models
{
    public class Message
    {
        public int Id { get; set; }
        public string Value { get; set; }
        [NotMapped]
        public User User { get; set; }
        public DateTime SendingDate { get; set; }
    }
}