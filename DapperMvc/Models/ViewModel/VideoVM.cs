using System.ComponentModel.DataAnnotations;

namespace DapperMvc_Upload_Image.Models.ViewModel
{
    public class VideoVM
    {
        [Required]
        [Display(Name = "Video Name")]
        public string Name { get; set; }

        
        
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Please choose video file ")]
        public IFormFile VideoPath { get; set; }
    }

}
