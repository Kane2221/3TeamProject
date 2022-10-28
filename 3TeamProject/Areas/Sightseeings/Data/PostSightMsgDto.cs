using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Sightseeings.Data
{
	public class PostSightMsgDto
	{
        [Required]
        public int SightseeingId { get; set; }
        [MaxLength(500)]
        public string? SightseeingMessageContent { get; set; }
    }
}
