using MessagePack;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace EShopOnlineExam.Models
{
    public class Candidate : IdentityUser
    {
        public Candidate()
        {
            Messages = new HashSet<Messages>();  
        }
        [DisplayName("CandidateNumber")]
        public int CandidateNumber { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [DisplayName("Native Language")]
        public string NativeLanguage { get; set; }
        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        [DisplayName("Photo Id Type")]
        public string PhotoIdType { get; set; }
        [DisplayName("Photo Id Number")]
        public string PhotoIdNumber { get; set; }
        [DisplayName("Photo Id Issue Date")]
        [DataType(DataType.Date)]
        public DateTime? PhotoIdIssueDate { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
        [DisplayName("Address Line 2")]
        public string? AddressLine2 { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("Region")]
        public string Region { get; set; }
        [DisplayName("Postal Code")]
        public int PostalCode { get; set; }
        [DisplayName("Country")]
        public string Country { get; set; }
        [DisplayName("Landline Number")]
        public string? LandlineNumber { get; set; }
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        

        public virtual ICollection<Messages> Messages { get; set; }
    }
}