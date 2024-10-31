
using Microsoft.Extensions.DependencyInjection;

namespace WorkoutDatabase.UI.UserCommunication
{
    public interface IUserCommunication
    {
        void PreMenu(IServiceCollection services);
        void Menu();
    }
}
