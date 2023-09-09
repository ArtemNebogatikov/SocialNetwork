using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SocialNetwork.BLL.Services
{
    public class UserService
    {
        MessageService messageService;
        IUserRepository userRepository;
        IFriendRepository friendRepository;
        public UserService()
        {
            messageService = new MessageService();
            userRepository = new UserRepository();
            friendRepository = new FriendRepository();
        }
        public void Register(UserRegistrationData userRegistrationData)
        {
            if (String.IsNullOrEmpty(userRegistrationData.FirstName))
            {
                throw new ArgumentNullException();
            }
            if (String.IsNullOrEmpty(userRegistrationData.LastName))
            {
                throw new ArgumentNullException();
            }
            if (String.IsNullOrEmpty(userRegistrationData.Password))
            {
                throw new ArgumentNullException();
            }
            if (String.IsNullOrEmpty(userRegistrationData.Email))
            {
                throw new ArgumentNullException();
            }
            if (userRegistrationData.Password.Length < 8)
            {
                throw new ArgumentNullException();
            }
            if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
            {
                throw new ArgumentNullException();
            }
            if (userRepository.FindByEmail(userRegistrationData.Email) != null)
            {
                throw new ArgumentNullException();
            }
            var userEntity = new UserEntity()
            {
                firstname = userRegistrationData.FirstName,
                lastname = userRegistrationData.LastName,
                email = userRegistrationData.Email,
                password = userRegistrationData.Password,
            };

            if (this.userRepository.Create(userEntity) == 0)
            {
                throw new Exception();
            }
        }
        public User Authenticate(UserAuthenticationData userAuthenticationData)
        {
            var findUserEntity = userRepository.FindByEmail(userAuthenticationData.Email);
            if (findUserEntity is null)
            {
                throw new UserNotFoundException();
            }

            if (findUserEntity.password != userAuthenticationData.Password)
            {
                throw new WrongPasswordException();
            }
            return ConstructUserModel(findUserEntity);
        }
        public User FindByEmail(string email)
        {
            var findUserEntity = userRepository.FindByEmail(email);
            if (findUserEntity is null)
            {
                throw new UserNotFoundException();
            }

            return ConstructUserModel(findUserEntity);
        }
        public User FindById(int id)
        {
            var findUserEntity = userRepository.FindById(id);
            if (findUserEntity is null)
            {
                throw new UserNotFoundException();
            }
            return ConstructUserModel(findUserEntity);
        }

        public void Update(User user)
        {
            var updatebleEntity = new UserEntity()
            {
                id = user.Id,
                firstname = user.FirstName,
                lastname = user.LastName,
                email = user.Email,
                password = user.Password,
                photo = user.Photo,
                favorite_book = user.FavoriteBook,
                favorite_movie = user.FavoriteMovie
            };

            if (this.userRepository.Update(updatebleEntity) == 0)
            {
                throw new Exception();
            }
        }
        public IEnumerable<User> GetFriendsByUserId(int userId)
        {
            return friendRepository.FindAllByUserId(userId)
                    .Select(friendsEntity => FindById(friendsEntity.friend_id));
        }

        public void AddFriend(UserAddingFriendData userAddingFriendData)
        {
            var findUserEntity = userRepository.FindByEmail(userAddingFriendData.FriendEmail);
            if (findUserEntity is null) throw new UserNotFoundException();

            var friendEntity = new FriendEntity()
            {
                user_id = userAddingFriendData.UserId,
                friend_id = findUserEntity.id
            };

            if (this.friendRepository.Create(friendEntity) == 0)
                throw new Exception();
        }

        public User ConstructUserModel(UserEntity userEntity)
        {
            var incomingMessages = messageService.GetIncomingMessagesByUserId(userEntity.id);

            var outgoingMessages = messageService.GetOutcomingMessagesByUserId(userEntity.id);
            var friends = GetFriendsByUserId(userEntity.id);


            return new User(userEntity.id,
                userEntity.firstname,
                userEntity.lastname,
                userEntity.email,
                userEntity.password,
                userEntity.photo,
                userEntity.favorite_book,
                userEntity.favorite_movie,
                incomingMessages,
                outgoingMessages,
                friends
                );
        }
    }
}
