using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class ProductsMessageBoard
    {
        public int MessageBoardId { get; set; }
        public int ProductId { get; set; }
        public int MemberId { get; set; }
        public string? ProductMessageContent { get; set; }
        public DateTime MessageCreatedTime { get; set; }
        public string ProductMessageStatus { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
