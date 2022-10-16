using Lovd.Models;

namespace Lovd.Services
{
    public class DataService:IDataService
    {
        private readonly LoveContext _context;
        public DataService(LoveContext context) { _context = context; }
    }
}
