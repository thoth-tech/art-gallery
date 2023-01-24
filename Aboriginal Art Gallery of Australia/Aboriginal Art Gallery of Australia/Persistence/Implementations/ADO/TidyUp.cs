using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO
{
    public class TidyUp
    {
        private readonly IConfiguration _configuration;

        public TidyUp(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        

    }
}
