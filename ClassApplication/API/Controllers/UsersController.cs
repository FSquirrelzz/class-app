using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
         [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO) {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //nameid
            var user = await _userRepository.GetUserByUserNameAsync(username);

            _mapper.Map(memberUpdateDTO, user);

            _userRepository.update(user);

            if(await _userRepository.SaveAllAsync()){
                return NoContent();
            }

            return BadRequest("Failed to update user");


        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var usersToReturn = await _userRepository.GetMembersAsync();

            return Ok(usersToReturn);
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            // var user = await _userRepository.GetUserByUserNameAsync(username);
            // var userToReturn = _mapper.Map<MemberDto>(user);
            var userToReturn = await _userRepository.GetMemberAsync(username); 
            return userToReturn;
        }
    }
}