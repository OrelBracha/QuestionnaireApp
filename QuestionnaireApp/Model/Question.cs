using System.ComponentModel.DataAnnotations;

namespace QuestionnaireApp.Model
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }
        
        public string QuestionText { get; set; }

        public List<Answer> Answers { get; set; }

        public QuestionType TypeOfQuestion { get; set; }
    }

    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public int Votes { get; set; }
        public bool isCorrect { get; set; }
    }

    public enum QuestionType
    {
        Poll,
        Trivia,
        // Add more types as needed
    }
}
