using System.ComponentModel;

namespace EShopOnlineExam.ViewModels
{

    public class CertificateVM

    {
        [DisplayName("Certificate Id")]
        public int Id { get; set; }

        [DisplayName("Certificate Title")]
        public string Title { get; set; }

        [DisplayName("Certificate Description")]
        public string Description { get; set; }
        public virtual List<Topic>? Topics { get; set; }
        public virtual List<QuestionAnswers>? Questions { get; set; }

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
