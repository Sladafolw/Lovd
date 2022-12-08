using Lovd.Models;

namespace Lovd.ModelsView
{
    public class ArticleView:Article
    {
        public IFormFile? Photo { get; set; }
    }
}
