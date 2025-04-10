using WebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Controllers
{
    public class RequestHeaderVM
    {
        [DisplayFormat(DataFormatString = "{0:dd.MM_HH:mm}")]
        public DateTime Created { get; set; }

        public int Id { get; set; }

        public ReqHeadFieldType Field { get; set; }

        public string Text { get; set; }

        public RequestHeaderVM(ReqHeadFieldType field, string text)
        {
            Created = DateTime.Now;
            Field = field;
            Text = text;
        }
    }
}
