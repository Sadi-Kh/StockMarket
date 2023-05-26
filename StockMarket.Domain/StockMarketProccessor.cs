﻿using StockMarket.Domain.Comparers;

namespace StockMarket.Domain
{
    public class StockMarketProccessor
    {
        private long lastOrderId;
        private long lastTradeId;
        private readonly List<Order> orders;
        private readonly List<Trade> trades;
        private readonly PriorityQueue<Order, Order> buyOrders;
        private readonly PriorityQueue<Order, Order> sellOrders;

        public IEnumerable<Order> Orders => orders;
        public IEnumerable<Trade> Trades => trades;

        public StockMarketProccessor(long lastOrderId = 0)
        {
            this.lastOrderId = lastOrderId;
            orders = new();
            trades = new();
            buyOrders = new(new MaxComparer());
            sellOrders = new(new MinComparer());
        }

        public long EnqueueOrder(TradeSide tradeSide, decimal quantity, decimal price)
        {
            Interlocked.Increment(ref lastOrderId);
            var order = new Order(lastTradeId, tradeSide, quantity, price);
            orders.Add(order);
            if (tradeSide == TradeSide.Buy) matchOrder(order, buyOrders, sellOrders, (decimal price1, decimal price2) => price1 >= price2);
            else matchOrder(order, sellOrders, buyOrders, (decimal price1, decimal price2) => price1 <= price2);
            return order.Id;
        }

        private void matchOrder(Order order, PriorityQueue<Order, Order> orders, PriorityQueue<Order, Order> matchingOrders, Func<decimal, decimal, bool> comparePriceDelegate)
        {
            while (matchingOrders.Count > 0 && order.Quantity > 0 && comparePriceDelegate(order.Price, matchingOrders.Peek().Price))
            {
                var peekedOrder = matchingOrders.Peek();
                makeTrade(order, peekedOrder);
            }
            if (order.Quantity > 0) orders.Enqueue(order, order);
        }

        private void makeTrade(Order order1, Order order2)
        {
            var matchingOrder = FindOrders(order1, order2);
            var buyOrder = matchingOrder.BuyOrder;
            var sellOrder = matchingOrder.SellOrder;
            decimal minQuantity = Math.Min(buyOrder.Quantity, sellOrder.Quantity);
            Interlocked.Increment(ref lastTradeId);
            var trade = new Trade(lastTradeId, buyOrder.Id, sellOrder.Id, minQuantity, sellOrder.Price);
            trades.Add(trade);

            buyOrder.decreaseQuantity(minQuantity);
            sellOrder.decreaseQuantity(minQuantity);

        }

        private static (Order BuyOrder, Order SellOrder) FindOrders(Order order1, Order order2)
        {
            if (order1.TradeSide == TradeSide.Buy) return (BuyOrder: order1, SellOrder: order2);
            else return (BuyOrder: order2, SellOrder: order1);
        }
    }
}