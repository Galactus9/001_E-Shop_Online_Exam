namespace EShopOnlineExam.Models
{
	public class CandidateCertificate
	{
		public int Id { get; set; }
		public virtual Candidate Candidate { get; set; }
		public virtual Certificate Certificate { get; set; }
	}
}
