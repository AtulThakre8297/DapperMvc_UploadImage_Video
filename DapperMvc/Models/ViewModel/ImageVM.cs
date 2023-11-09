using System.ComponentModel.DataAnnotations;


namespace DapperMvc_Upload_Image.Models.ViewModel
{
    public class ImageVM
    {
       
        [Required(ErrorMessage = "Please enter Image Name ")]
        public string Name { get; set; }

        //return type is IFormFile format.
        [Required(ErrorMessage = "Please choose image file ")]
        public IFormFile ImagePath { get; set; }
    }
}
