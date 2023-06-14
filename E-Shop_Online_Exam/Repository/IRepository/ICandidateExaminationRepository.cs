using EShopOnlineExam.Models;


namespace EShopOnlineExam.Repository.IRepository
{
    public interface ICandidateExaminationRepository : IRepository<CandidateExamination>
    {
        void Update(CandidateExamination candidateExamination);

        //Task<IEnumerable<CandidateExamination>> GetAllCandidateExaminationsAs();

        public Task<IEnumerable<CandidateExamination>> GetAllCandidateExaminationsAs();

        public Task<CandidateExamination> GetAllCandidateExaminationsExamId(int Id);
        public Task<List<CandidateExamination>> GetAllExaminationsForCand(string Id);

        public Task<CandidateExamination> GetWithExam(int Id);

    }
}
