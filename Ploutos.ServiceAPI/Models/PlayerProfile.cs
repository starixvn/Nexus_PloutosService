namespace Ploutos.ServiceAPI.Models
{
    using System.ComponentModel.DataAnnotations;

#pragma warning disable S109 // Magic numbers should not be used
    public class PlayerProfile
    {
        private const string RequiredErrorMessage = "{0} is required.";
        private const string MaxLengthErrorMessage = "{0} must not over {1} characters long.";
        private const string MinMaxLengthErrorMessage = "{0} must be between {2} and {1} characters long.";

        public int RowIndex { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(20, ErrorMessage = MaxLengthErrorMessage)]
        public string BrandCode { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(5, MinimumLength = 3, ErrorMessage = MinMaxLengthErrorMessage)]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(50, ErrorMessage = MaxLengthErrorMessage)]
        [EmailAddress(ErrorMessage = "{0} is invalid.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(15, MinimumLength = 7, ErrorMessage = MinMaxLengthErrorMessage)]
        public string IPAddress { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public bool? IsDeposit { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(5, MinimumLength = 2, ErrorMessage = MinMaxLengthErrorMessage)]
        public string LanguageCode { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(20, ErrorMessage = MaxLengthErrorMessage)]
        public string MobileCountryCode { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(15, MinimumLength = 7, ErrorMessage = MinMaxLengthErrorMessage)]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = MinMaxLengthErrorMessage)]
        public string Password { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int? PlayerTypeCode { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(30, ErrorMessage = MaxLengthErrorMessage)]
        public string Username { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(30, ErrorMessage = MaxLengthErrorMessage)]
        public string Username2 { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(5, MinimumLength = 2, ErrorMessage = MinMaxLengthErrorMessage)]
        public string CountryCode { get; set; }
    }
}