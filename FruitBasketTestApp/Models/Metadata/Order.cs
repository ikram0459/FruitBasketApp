namespace FruitBasketTestApp.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
        public class OrderMetadata
        {
          
                [Required(ErrorMessage = "First Name is required")]
                [DisplayName("First Name")]
                [StringLength(160)]
                public string FirstName { get; set; }

                [Required(ErrorMessage = "Last Name is required")]
                [DisplayName("Last Name")]
                [StringLength(160)]
                public string LastName { get; set; }

                [Required(ErrorMessage = "Address is required")]
                [StringLength(70)]
                public string Address { get; set; }

                [Required(ErrorMessage = "City is required")]
                [StringLength(40)]
                public string City { get; set; }

                [Required(ErrorMessage = "Country is required")]
                [StringLength(40)]
                public string Country { get; set; }

   
            
        }
    }
}