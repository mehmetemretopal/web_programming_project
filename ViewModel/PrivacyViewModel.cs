using System.ComponentModel.DataAnnotations;
namespace authenticatedapp.ViewModel
{
	public class PrivacyViewModel
	{
		public string? Title { get; set; }
        public string? email { get; set; }
        public string? Name { get; set; }
		public string? About { get; set; }
		public string BlogText { get; set; }
    }
}
