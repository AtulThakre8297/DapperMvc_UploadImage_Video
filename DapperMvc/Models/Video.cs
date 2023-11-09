using System.ComponentModel.DataAnnotations;

namespace DapperMvc_Upload_Image.Models
{
    public class Video
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Video Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Video File")]
        [DataType(DataType.Upload)]
        public string VideoPath { get; set; }
    }

}
