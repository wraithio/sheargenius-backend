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

namespace sheargenius_backend.Services
{
    public class UserServices
    {

        private readonly DataContext _dataContext;
        public UserServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<UserModel> SeeAllUsers()
        {
            return _dataContext.Users.ToList();
        }

        public bool CreateUser(UserDTO newUser)
        {
            bool result = false;

            if (!DoesUserExist(newUser.Username))
            {
                UserModel userToAdd = new();
                userToAdd.Username = newUser.Username;
                PasswordDTO hashedPassword = HashPassword(newUser.Password);
                userToAdd.Hash = hashedPassword.Hash;
                userToAdd.Salt = hashedPassword.Salt;
                userToAdd.AccountType = "";
                userToAdd.Name = "";
                userToAdd.Bio = "";
                userToAdd.Email = "";
                userToAdd.ShopName = "";
                userToAdd.Address = "";
                userToAdd.City = "";
                userToAdd.State = "";
                userToAdd.ZIP = "";
                userToAdd.Pfp = "";

                _dataContext.Users.Add(userToAdd);
                result = _dataContext.SaveChanges() != 0;
            }
            return result;
        }

        private bool DoesUserExist(string username)
        {
            //SingleorDefault finds first or default instance of whatever is in parameters
            return _dataContext.Users.SingleOrDefault(users => users.Username == username) != null;
        }

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

        public string Login(UserDTO user)
        {
            string? result = null;
            UserModel foundUser = GetUserbyUsername(user.Username);

            if (foundUser == null)
            {
                return result;
            }

            if (VerifyPassword(user.Password, foundUser.Salt, foundUser.Hash))
            {
                //JWT: JSON Web Token: a type of token used for authentification or transferring information.

                //Bearer Token: a token that grants access to a resource, such as an API. JWT can be used as a bearer token, but there are other types of tokens that can be used as a bearer token.

                //Setting the string that will be encrypted into our JWT
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

                //Now to encrypt our secret key
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                //Set the options for our token to define properties such as where the token is issued from, where it is allowed to be used, and most importantly how long the token lasts before expiring.
                var tokenOptions = new JwtSecurityToken(
                    //issuer = where is this token allowed to be generated from
                    issuer: "sheargenius-awakhjcph2deb6b9.westus-01.azurewebsites.net",
                    //audience = where this token is allowed to authenticate
                    //issuer and audience should be the same since our API is handling both login and authentication
                    audience: "sheargenius-awakhjcph2deb6b9.westus-01.azurewebsites.net",
                    //claims = addition options for auth
                    claims: new List<Claim>(),
                    //Sets the token expiration time and date. This is what make our tokens temporary, thus keeping our access to our resources safe and secure.
                    expires: DateTime.Now.AddMinutes(30),
                    //this attatches our newly encrypted super secret key that was turned into sign in credentials
                    signingCredentials: signingCredentials
                );

                //generate our JWT and sasve the token as a string into a variable
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                result = tokenString;

                //token anatomy:
                //asdfernn3435.asdfnwrn224b345h.ihihfw3fb
                //Header: asdfernn3435
                //Payload: asdfnwrn224b345h this will have info about the token, including the expiration date
                //Signature: ihihfw3fb encrypt and combine headder and payload using secret key
            }

            return result;
        }

        public UserModel GetUserbyUsername(string username)
        {
            return _dataContext.Users.SingleOrDefault(user => user.Username == username);
        }

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

        public bool UpdatePassword(UserDTO user)
        {
            bool result = false;

            var foundUser = GetUserbyUsername(user.Username);

            if (foundUser == null)
            {
                return result;
            }

            PasswordDTO hashPassword = HashPassword(user.Password);
            foundUser.Hash = hashPassword.Hash;
            foundUser.Salt = hashPassword.Salt;

            _dataContext.Update<UserModel>(foundUser);
            result = _dataContext.SaveChanges() != 0;
            return result;

        }

        public bool DeleteAccount(UserDTO user)
        {
            bool result = false;
            UserModel foundUser = GetUserbyUsername(user.Username);
            if (Login(user) == null)
            {
                return result;
            }
            if (VerifyPassword(user.Password, foundUser.Salt, foundUser.Hash))
            {
                _dataContext.Users.Remove(foundUser);
                result = _dataContext.SaveChanges() != 0;
                return result;
            }
            return result;
        }

        public bool EditAccount(UserModel foundUser, UserModel updatedUser)
        {
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
            bool result = _dataContext.SaveChanges() != 0;
            return result;
        }

        
    }
}