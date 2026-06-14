using System.ComponentModel.DataAnnotations;

namespace Rozetka.ViewModels
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Номер картки є обов'язковим")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Термін дії є обов'язковим")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Формат має бути ММ/РР (наприклад, 12/27)")]
        public string CardExpiry { get; set; }

        [Required(ErrorMessage = "CVV код є обов'язковим")]
        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "CVV код має складатися з 3 цифр")]
        public string CardCvv { get; set; }
    }
}