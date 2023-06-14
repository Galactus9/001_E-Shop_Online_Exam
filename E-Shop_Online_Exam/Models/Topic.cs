using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EShopOnlineExam.Models
{
    public class Topic
    {
        public int Id { get; set; }
        [DisplayName("Certificate")]
        public Certificate Certificate { get; set; }
        [DisplayName("Topic Title")]
        public string Title { get; set; }
        [DisplayName("Topic Description")]
        public string Description { get; set; }
        public virtual ICollection<QuestionAnswers>? QuestionAnswers { get; set; }
    }
}
