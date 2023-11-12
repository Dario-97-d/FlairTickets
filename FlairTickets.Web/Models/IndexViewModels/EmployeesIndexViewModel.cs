using System.Collections.Generic;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Models.IndexViewModels
{
    public class EmployeesIndexViewModel
    {
        public User SearchModel { get; set; }

        public IEnumerable<User> Employees { get; set; }
    }
}
