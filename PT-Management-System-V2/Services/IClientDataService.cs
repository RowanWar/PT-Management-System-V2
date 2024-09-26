using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.Services
{
    public interface IClientDataService
    {
        //These were originally workoutmodel but i changed to clientmodel
        List<ClientModel> GetAllClients();
        List<ClientModel> SearchClients(string searchTerm);

        ClientModel GetClientById(int ClientId);

        //int Insert(ClientModel client);
        //int Update(ClientModel client);

        //int Delete(ClientModel client);
    }
}
