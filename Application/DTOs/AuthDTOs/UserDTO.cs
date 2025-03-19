using Application.Common.Converter;
using Domain.Enum;
using ProcessManagement.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Application.DTOs.AuthDTOs
{
    public class UserDTO
    {
        public string FirstName { get; set; } = "";

        [StringLength(maximumLength: 40)]
        public string LastName { get; set; } = "";
        public Gender Gender { get; set; }

        public Role RoleType { get; set; }

        public string PhoneNumber { get; set; } = "";

        public string Address { get; set; } = "";

        public DateTime DateOfBirth { get; set; }


    }
}
