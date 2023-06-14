using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EShopOnlineExam.Models
{
	public class ExamQuestion
	{
        
        public int Id { get; set; }
        public bool? status { get; set; }
        public ExamTopics ExamTopics { get; set; }
        public QuestionAnswers QuestionAnswer {get; set;}

	}
}