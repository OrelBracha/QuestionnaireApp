using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Data;
using QuestionnaireApp.Model;

namespace QuestionnaireApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : Controller
    {
        private readonly APIDbContext _context;

        public QuestionsController(APIDbContext context)
        {
            _context = context;
        }
        // GET: api/Questions
        //Receive all the Questions and answers
        [HttpGet]
        public async Task<IActionResult> GetQuestions(string? searchTerm = "", int page = 1, int pageSize = 10)
        {
            try
            {
                // Asynchronously fetch questions from the database
                var query = _context.Questions.Include(q => q.Answers).AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(q => q.QuestionText.Contains(searchTerm));
                }

                var result = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Question/id
        //Receive a specific question and it's answers
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            if (id.Equals(null) || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(m => m.QuestionID == id);
            
            if (question == null)
            {
                return NotFound();
            }

            return question;
        }
 

        // POST: Questions/Create a new question
        [HttpPost]
        public async Task<ActionResult<Question>>PostQuestion(Question question)
        {

            _context.Add(question);
            await _context.SaveChangesAsync();

            return Ok(question.QuestionID);
        }

        [HttpPost("{questionId}/vote")]
        public async Task<IActionResult> VoteForAnswer(int questionId, [FromBody] VoteRequest voteRequest)
        {
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuestionID == questionId);

            if (question == null)
            {
                return NotFound("Question not found");
            }

            var answer = question.Answers.FirstOrDefault(a => a.Id == voteRequest.AnswerId);

            if (answer == null)
            {
                return NotFound("Answer not found");
            }

            // Update the vote count
            answer.Votes++;

            // If it's a trivia question, check if the answer is correct
            bool isCorrect = question.TypeOfQuestion == QuestionType.Trivia && answer.isCorrect;

            var result = new
            {
                Votes = answer.Votes,
                IsCorrect = isCorrect
            };

             await _context.SaveChangesAsync();

            return Ok(result);
        }

    }
}
