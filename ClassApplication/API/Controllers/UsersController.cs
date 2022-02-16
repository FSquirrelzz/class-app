using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _photoService = photoService;
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //nameid
            var user = await _userRepository.GetUserByUserNameAsync(username);

            _mapper.Map(memberUpdateDTO, user);

            _userRepository.update(user);

            if (await _userRepository.SaveAllAsync())
            {
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
            var userToReturn = await _userRepository.GetMemberAsync(username);
            return userToReturn;
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUserNameAsync(username);
            var result = await _photoService.UploadPhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            photo.IsMain = user.Photos.Count == 0;
            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return _mapper.Map<PhotoDTO>(photo);
            }

            return BadRequest("Failed to add photo");


        }
    }
}