using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApiApp.Models
{
  public interface IValuesRepository {
    Task<IEnumerable<(int Id, string Value)>> GetAll();
    Task Create(string value);
    Task Update(int id, string value);
    Task Remove(int id);
  }
}