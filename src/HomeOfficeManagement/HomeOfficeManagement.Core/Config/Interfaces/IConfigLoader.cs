using System.Threading.Tasks;

namespace HomeOfficeManagement.Core.Config.Interfaces
{
    public interface IConfigLoader<out T>
    {
        T GetConfig();
    }
}