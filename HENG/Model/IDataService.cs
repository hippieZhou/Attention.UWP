using System.Threading.Tasks;

namespace HENG.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}