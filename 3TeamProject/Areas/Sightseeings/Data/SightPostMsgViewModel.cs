using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Sightseeings.Data
{
	public class SightPostMsgViewModel
	{
        [Required]
        public int SightseeingId { get; set; }
        [MaxLength(500)]
        public string? SightseeingMessageContent { get; set; }
    }
}
