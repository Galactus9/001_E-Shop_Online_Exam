using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;

namespace EShopOnlineExam.Services
{
    public class ExamEngine
    {
        //private readonly IUnitOfWork _unitOfWork;

        //public ExamEngine(IUnitOfWork unitOfWork)
       // {
        //    _unitOfWork = unitOfWork;
       // }

        public ExamEngine()
        {
         
        }
        public async Task<CandidateExamination> ExamEngineResult (IUnitOfWork _unitOfWork, int Id)
        {
            var candidateExamination = await _unitOfWork.CandidateExamination.GetAllCandidateExaminationsExamId(Id);
            var candidateExamResults = await _unitOfWork.CandidateResults.WhereExaminationId(Id);
            candidateExamination.timestamp = 0;
            var tempQAR = 0;
            foreach (var qa in candidateExamResults)
            {
                if (qa.CandidateQuestionAnswer == qa.ExamQuestion.QuestionAnswer.CorrectIndex)
                {
                    qa.QuestionResult = 1;
                    tempQAR++;

                }
                else if (qa.CandidateQuestionAnswer != qa.ExamQuestion.QuestionAnswer.CorrectIndex)
                {
                    qa.QuestionResult = 0;
                }
                
            }

            if (tempQAR >= 65)
            {
                candidateExamination.CandidateExaminationResult = 1;
            }
            else
            {
                candidateExamination.CandidateExaminationResult = 0;
            }

            _unitOfWork.CandidateExamination.Update(candidateExamination);
            _unitOfWork.Save();

            return candidateExamination;
        }
    }
}
