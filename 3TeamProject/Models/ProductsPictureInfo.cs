using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class ProductsPictureInfo
    {
        public int ProductPictureId { get; set; }
        public int ProductId { get; set; }
        public string ProductPicturePath { get; set; } = null!;
        public string ProductPictureName { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
