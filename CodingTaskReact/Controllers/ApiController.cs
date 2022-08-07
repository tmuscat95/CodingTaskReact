using Cleverbit.CodingTask.Data;
using Cleverbit.CodingTask.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cleverbit.CodingTask.Host.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly CodingTaskContext context;
        private readonly Random random;

        public ApiController(CodingTaskContext context)
        {
            this.context = context;
            this.random = new Random();
        }
        // GET: api/ping
        [HttpGet]
        public string Get()
        {
            return "Ping received";
        }

        // GET api/ping/with-auth
        [HttpGet("with-auth")]
        [Authorize]
        public string GetWithAuth()
        {
            return $"Ping received with successful authorization. User Name : {User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value}";
        }

        Match? GetCurentMatch()
        {
            var matches = context.Matches.Select(m => m).OrderBy(m => m.ExpiryTime).ToList();
            Match ret = null;

            foreach (var match in matches)
            {
                if (match.ExpiryTime > DateTime.Now)
                {
                    ret= match;
                    break;
                }
            }

            return ret;
        }


        
        [HttpGet("play")]
        [Authorize]
        public async Task<IActionResult> Play()
        {
            

            try
            {
                
                var playerUsername = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
                var player = context.Users.Where(u => u.UserName.ToLower() == playerUsername.ToLower()).Select(u => u).FirstOrDefault();
                var currentMatch = GetCurentMatch();
                if(currentMatch == null)
                {
                    return BadRequest("All matches expired.");
                }
                if (context.Guesses.Any(g => g.UserId == player.Id && g.MatchNo == currentMatch.MatchNo))
                {
                    return BadRequest($"User {playerUsername} has already guessed for the current match.");//User has alreadt guessed this match;
                }

                var newGuess = new Data.Models.Guess { MatchNo = currentMatch.MatchNo ,UserId = player.Id, GuessedNumber = random.Next(0,100) };
                context.Guesses.Add(newGuess);
                await context.SaveChangesAsync();
                return Ok(new { guessedNumber=newGuess.GuessedNumber});

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("reset-matches")]
        public async Task<IActionResult> Reset()
        {
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Guesses]");
            context.Matches.RemoveRange(context.Matches.Select(m=>m));
            for (int i = 0; i < 5; i++)
            {
                context.Matches.Add(new Data.Models.Match { MatchNo = i, ExpiryTime = DateTime.Now.AddMinutes(2 * i) });
            }
            await context.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("matches")]
        public IEnumerable<Match> Matches()
        {
            var currentMatches = context.Matches.Select(m => m).ToList();
            foreach (Match match in currentMatches)
            {
                if(match.ExpiryTime < DateTime.Now)
                {

                    var winner = "No Winner";
                    var matchGuesses = context.Guesses.Include(g=>g.User).Where(g => g.MatchNo == match.MatchNo).Select(g=>g).ToList();
                    if (matchGuesses.Any())
                    {
                        var bestGuess = matchGuesses.FirstOrDefault();
                        foreach(var guess in matchGuesses)
                        {
                            if(guess.GuessedNumber > bestGuess.GuessedNumber)
                                   bestGuess = guess;

                        }
                        winner = bestGuess.User.UserName;
                    }
                    match.Winner = winner;

                }
            }
            
            return currentMatches;
        }
    }
}
