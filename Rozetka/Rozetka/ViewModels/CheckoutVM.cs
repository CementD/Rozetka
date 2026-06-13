using System.ComponentModel.DataAnnotations;

namespace Rozetka.ViewModels
{
    public class CheckoutVM
    {
        public string? CardNumber { get; set; }
        public string? CardExpiry { get; set; }
        public string? CardCvv { get; set; }
    }
}
