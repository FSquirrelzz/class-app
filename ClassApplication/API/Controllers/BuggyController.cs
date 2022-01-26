using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController:BaseAPIController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context=context;
        }

        [Authorize]
        [HttpGet("auth")]//401 unauthorized
        public ActionResult<string>GetSecret()//api/buggy/auth
        {
            return "SecretString";
        }
        [HttpGet("not-found")]//404 not found
        public ActionResult<AppUser> GetNotFound()//api/buggy/not-found
        {
            var thing = _context.Users.Find(-1);
            if (thing == null)
            {
                return NotFound();
            }
            return Ok();
        }
        [HttpGet("server-error")]//500 server error
        public ActionResult<string>GetServerError()//api/buggy/server-error
        {
           var thing = _context.Users.Find(-1);
           var thingToString = thing.ToString();//NullReferenceException
           return thingToString;
        }
        [HttpGet("bad-request")]//api/buggy/bad-request
        public ActionResult<string>GetBadRequest()
        {
            return BadRequest("Bad information");
        }
    }
}