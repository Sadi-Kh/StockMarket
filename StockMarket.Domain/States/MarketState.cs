﻿namespace StockMarket.Domain.States
{
    public abstract class MarketState : IStockMarketProccessor
    {
        protected StockMarketProccessor stockMarketProcessor { get; set; }
        public IEnumerable<Order> Orders => throw new NotImplementedException();
        public IEnumerable<Trade> Trades => throw new NotImplementedException();

        public MarketState(StockMarketProccessor stockMarketProcessor)
        {
            this.stockMarketProcessor = stockMarketProcessor;
        }

        public virtual void CloseMarket()
        {
            throw new NotImplementedException();
        }

        public virtual void OpenMarket()
        {
            throw new NotImplementedException();
        }

        public virtual long EnqueueOrder(TradeSide tradeSide, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public virtual long? CancelOrder(long orderId)
        {
            throw new NotImplementedException();
        }

        public virtual long ModifyOrder(long orderId, TradeSide tradeSide, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}