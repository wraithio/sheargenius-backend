using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using sheargenius_backend.Context;
using sheargenius_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using sheargenius_backend.Models.DTOs;

namespace sheargenius_backend.Services
{
    public class UserServices
    {

        private readonly DataContext _dataContext;
        private readonly IConfiguration _config;
        public UserServices(DataContext dataContext, IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }

        public List<UserModel> SeeAllUsers()
        {
           return _dataContext.Users.ToList();
        }

        public async Task<bool> CreateUser(UserInfoDTO newUser)
        {
            if (await DoesUserExist(newUser.Username)) return false;
            PasswordDTO hashedPassword = HashPassword(newUser.Password);

            var userToAdd = new UserModel
            {
                Username = newUser.Username,
                Hash = hashedPassword.Hash,
                Salt = hashedPassword.Salt,
                Date = newUser.Date,
                AccountType = newUser.AccountType,
                RatingCount = newUser.RatingCount,
                Rating = newUser.Rating,
                Followers = newUser.Followers,
                Following = newUser.Following,
                Likes = newUser.Likes,
                SecurityAnswer = newUser.SecurityAnswer,
                SecurityQuestion = newUser.SecurityQuestion,
                Name = newUser.Name,
                Bio = newUser.Bio,
                Email = newUser.Email,
                ShopName = newUser.ShopName,
                Address = newUser.Address,
                City = newUser.City,
                State = newUser.State,
                ZIP = newUser.ZIP,
                Pfp = newUser.Pfp,
                IsDeleted = false
            };
            await _dataContext.Users.AddAsync(userToAdd);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditAccount(UserModel updatedUser)
        {
            var foundUser = await GetUserByUsername(updatedUser.Username);
            foundUser.AccountType = updatedUser.AccountType;
            foundUser.Name = updatedUser.Name;
            foundUser.Bio = updatedUser.Bio;
            foundUser.Email = updatedUser.Email;
            foundUser.ShopName = updatedUser.ShopName;
            foundUser.Address = updatedUser.Address;
            foundUser.City = updatedUser.State;
            foundUser.State = updatedUser.City;
            foundUser.ZIP = updatedUser.ZIP;
            foundUser.Pfp = updatedUser.Pfp;
            _dataContext.Users.Update(foundUser);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        private async Task<bool> DoesUserExist(string username) => await _dataContext.Users.SingleOrDefaultAsync(users => users.Username == username) != null;
        //SingleorDefault finds first or default instance of whatever is in parameters


        private static PasswordDTO HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(64);

            string salt = Convert.ToBase64String(saltBytes);

            string hash;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(deriveBytes.GetBytes(32));
            }

            PasswordDTO hashedPassword = new();
            hashedPassword.Salt = salt;
            hashedPassword.Hash = hash;
            return hashedPassword;
        }

        public async Task<string> Login(UserDTO user)
        {
            UserModel foundUser = await GetUserByUsername(user.Username);

            if (foundUser == null) return null;
            if (!VerifyPassword(user.Password, foundUser.Salt, foundUser.Hash)) return null;
            return GenerateJWTToken(new List<Claim>());
        }
        private string GenerateJWTToken(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));

            //Now to encrypt our secret key
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://sheargenius-awakhjcph2deb6b9.westus-01.azurewebsites.net/",
                audience: "https://sheargenius-awakhjcph2deb6b9.westus-01.azurewebsites.net/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            //token anatomy:
            //asdfernn3435.asdfnwrn224b345h.ihihfw3fb
            //Header: asdfernn3435
            //Payload: asdfnwrn224b345h this will have info about the token, including the expiration date
            //Signature: ihihfw3fb encrypt and combine headder and payload using secret key
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<UserModel> GetUserByUsername(string username) => await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username);
        public async Task<UserInfoDTO> GetProfileInfoByUsername(string username)
        {
            var currentUser = await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username);

            UserInfoDTO user = new();

            user.Id = currentUser.Id;
            user.Username = currentUser.Username;
            user.Password = "*************";
            user.Date = currentUser.Date;
            user.AccountType = currentUser.AccountType;
            user.Name = currentUser.Name;
            user.Bio = currentUser.Bio;
            user.Email = currentUser.Email;
            user.Rating = currentUser.Rating;
            user.RatingCount = currentUser.RatingCount;
            user.Followers = currentUser.Followers;
            user.Following = currentUser.Following;
            user.SecurityQuestion = currentUser.SecurityQuestion;
            user.SecurityAnswer = currentUser.SecurityAnswer;
            user.ShopName = currentUser.ShopName;
            user.Address = currentUser.Address;
            user.City = currentUser.City;
            user.State = currentUser.State;
            user.ZIP = currentUser.ZIP;
            user.Pfp = currentUser.Pfp;
            user.IsDeleted = currentUser.IsDeleted;
            return user;
        }
        public async Task<UserModel> GetUserByUserId(int id) => await _dataContext.Users.FindAsync(id);

        private static bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            string newHash;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                newHash = Convert.ToBase64String(deriveBytes.GetBytes(32));
            }
            return hash == newHash;
        }

        public async Task<bool> UpdatePassword(UserDTO user)
        {
            var foundUser = await GetUserByUsername(user.Username);

            if (foundUser == null) return false;

            PasswordDTO hashPassword = HashPassword(user.Password);
            foundUser.Hash = hashPassword.Hash;
            foundUser.Salt = hashPassword.Salt;

            _dataContext.Update<UserModel>(foundUser);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteAccount(UserDTO user)
        {
            UserModel foundUser = await GetUserByUsername(user.Username);
            if (Login(user) == null) return false;
            if (VerifyPassword(user.Password, foundUser.Salt, foundUser.Hash)) _dataContext.Users.Remove(foundUser);
            return await _dataContext.SaveChangesAsync() != 0;
        }
        // DELETE ACCOUNT ON FRONT END WILL BE EDITACCOUNT ENDPOINT THEN TOGGLE IsDeleted bool FROM FALSE TO TRUE

        public async Task<bool> ToggleFollowersAsync(string userFollowing, string userFollowed)
        {
            var followingUser = await GetUserByUsername(userFollowing);
            var followedUser = await GetUserByUsername(userFollowed);
            if (followingUser.Following.Contains(userFollowed))
            {
                followingUser.Following = followingUser.Following.Where(o => o != userFollowed).ToList();
                followedUser.Followers = followingUser.Followers.Where(o => o != userFollowing).ToList();
            }
            else
            {
                followedUser.Followers.Add(userFollowing);
                followingUser.Following.Add(userFollowed);
            }
            _dataContext.Users.Update(followingUser);
            _dataContext.Users.Update(followedUser);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public List<UserModel> GetAllBarbers() => _dataContext.Users.Where(user => user.AccountType == "Barber").ToList();

        public async Task<bool> AddRating(RatingModel rateModel)
        {
            if(rateModel==null) return false;
            var foundUser = await GetUserByUsername(rateModel.UserToRate);
            // if(foundUser.RatingCount.Contains(username)) return false;
            foundUser.Rating += rateModel.Rating;
            foundUser.RatingCount.Add(rateModel.Username);
            _dataContext.Users.Update(foundUser);
            return await _dataContext.SaveChangesAsync() != 0;
        }
    }
}