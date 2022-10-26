namespace _3TeamProject.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ProductId { get; set; }  //商品ID
        public int Amount { get; set; }     //數量
        public int SubTotal { get; set; }   //小計
    }
}
