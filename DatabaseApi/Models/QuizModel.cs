namespace DatabaseApi.Models
{
    public class QuizModel
    {
        public FlagModel Correct { get; set; }
        public List<FlagModel> Options { get; set; }
    }
}