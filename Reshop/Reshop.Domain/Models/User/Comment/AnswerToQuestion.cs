namespace Reshop.Domain.Models.User.Comment
{
    public class AnswerToQuestion
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public int QuestionId { get; set; }
        public string FullName { get; set; }
        public string AnswerText { get; set; }
        public int Like { get; set; }
        public string DateTime { get; set; }

        public QuestionForProduct QuestionForProduct { get; set; }
    }
}