using Lovd.Models;
using System.ComponentModel.DataAnnotations;
namespace Lovd.ModelsView
{
    public class ArticleComments
    {
        public IEnumerable<Lovd.Models.Comment> comments { get; set; }
        [MaxLength(1000)]
        public  string? comment { get; set; }
       

    }
}
