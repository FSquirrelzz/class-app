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
        [HttpGet("{username}",Name ="GetUser")]
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
                return CreatedAtRoute("GetUser",new {username = user.UserName},_mapper.Map<PhotoDTO>(photo));
            }
            return BadRequest("Failed to add photo");
        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUserNameAsync(username);
            var photo = user.Photos.FirstOrDefault(p=>p.Id==photoId);
            if (photo.IsMain)
            {
                return BadRequest("This is already the main photo");
            }
            var currentMainPhoto = user.Photos.FirstOrDefault(x=>x.IsMain==true);
            currentMainPhoto.IsMain = false;
            photo.IsMain = true;
            if (await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to set photo to main");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUserNameAsync(username);
            var photo = user.Photos.FirstOrDefault(p=>p.Id==photoId);
            if(photo==null)
            {
                return BadRequest("Photo not found");
            }
            if (photo.IsMain)
            {
                return BadRequest("You cannot delete your main photo");
            }
            if(photo.PublicId!=null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error!=null)
                {
                    return BadRequest(result.Error.Message);
                }
            }
            user.Photos.Remove(photo);
            if(await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to delete photo");
        }
    }
}