using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EShopOnlineExam.Models
{
    public enum CertificateStatus
    {
        Invalid = -1,
        Enabled = 1,
        Disabled = 2
    }

    public class Certificate

    {
        [DisplayName("Certificate Id")]
        public int Id { get; set; }
        
        [DisplayName("Certificate Title")]
        public string Title { get; set; }
        
        [DisplayName("Certificate Description")]
        public string Description { get; set; }
        public virtual ICollection<Topic>? Topics { get; set; }
      
        [DisplayName("Possible Marks")]
        public int PossibleMarks { get; set; } 
       
        [DisplayName("Score Needed To Pass")]
        public int ScoreNeededToPass { get; set; }
        
        [DisplayName("Certificate Status")]
        public CertificateStatus State { get; set; }
       
        [DisplayName("Certificate Price")]
        public int Price { get; set; }
    }
}