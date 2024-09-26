using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.Services
{
    public interface IProfileDataService
    {
        List<CoachModel> GetCoachProfileById(int CoachId);
        //List<ClientModel> SearchClients(string searchTerm);

        //ClientModel GetClientById(int ClientId);

    }
}
