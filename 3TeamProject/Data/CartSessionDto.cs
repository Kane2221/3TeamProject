namespace _3TeamProject.Data
{
    public class CartSessionDto
    {
        public int MemberId { get; set; }
        public int ProductId { get; set; }  //商品ID
        public int Amount { get; set; }     
        public int SubTotal { get; set; }   //小計
        public int Quantity { get; set; } //數量
    }
}
