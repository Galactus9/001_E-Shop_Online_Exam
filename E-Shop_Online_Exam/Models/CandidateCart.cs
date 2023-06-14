using System.ComponentModel;

namespace EShopOnlineExam.Models
{
    public class CandidateCart
    {
        public int Id { get; set; }
        
        public virtual Candidate Candidate { get; set; }   

        public virtual Certificate Certificates { get; set; }
    }
}
