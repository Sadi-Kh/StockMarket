namespace StockMarket.Domain
{
    public class Trade
    {
        private long id;
        private object buyOrderId;
        private object sellOrderId;
        private decimal quantity;
        private object price;

        public Trade(long lastTradeId, object id1, object id2, decimal minQuantity, object price)
        {
            this.id = lastTradeId;
            this.buyOrderId = id1;
            this.sellOrderId = id2;
            this.quantity = minQuantity;
            this.price = price;
        }

        public long Id { get => id; }
        public object BuyOrderId { get => buyOrderId; }
        public object SellOrderId { get => sellOrderId; }
        public decimal Quantity { get => quantity; }
        public object Price { get => price; }
    }
}