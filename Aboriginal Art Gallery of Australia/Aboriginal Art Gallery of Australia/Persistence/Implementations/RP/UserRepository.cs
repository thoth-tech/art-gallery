using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class UserRepository : IRepository, IUserDataAccess
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        private IRepository _repo => this;

        public bool DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public UserOutputDto? GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserOutputDto> GetUsers()
        {
            throw new NotImplementedException();
        }

        public UserInputDto? InsertUser(UserInputDto user)
        {
            throw new NotImplementedException();
        }

        public UserInputDto? UpdateUser(int id, UserInputDto user)
        {
            throw new NotImplementedException();
        }
    }
}
