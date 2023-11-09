using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Company
{
    public int Id { get; set; }

    [Display(Name = "Company Name")]
    [Required(ErrorMessage = "Company Name is required")]
    public string CompanyName { get; set; }

    [Display(Name = "Company Address")]
    [Required(ErrorMessage = "Company Address is required")]
    public string CompanyAddress { get; set; }

    [Required(ErrorMessage = "Country is required")]
    public string Country { get; set; }

    [Display(Name = "Glassdoor Rating")]
    [Range(1, 5, ErrorMessage = "Glassdoor Rating must be between 1 and 5")]
    public int GlassdoorRating { get; set; }

}
