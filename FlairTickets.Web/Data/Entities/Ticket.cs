﻿using FlairTickets.Web.Data.Repository.Interfaces;

namespace FlairTickets.Web.Data.Entities
{
    public class Ticket : IEntity
    {
        public int Id { get; set; }

        public Flight Flight { get; set; }

        public int Seat { get; set; }

        public User User { get; set; }
    }
}
