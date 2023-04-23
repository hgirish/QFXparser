using System;

namespace QFXparser
{
    public class Transaction
    {
        public string Type { get; set; }
        public DateTime PostedOn { get; set; }
        public Decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public string RefNumber { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
    }
    public class ReinvestTransaction
    {

        public string TransactionId { get; set; }

        public DateTime TradeDate { get; set; }

        public DateTime SettleDate { get; set; }
        public string IncomeType { get; set; }

        public decimal Total { get; set; }
    
        public string SubType { get; set; }
   
        public decimal Units { get; set; }

        public decimal UnitPrice { get; set; }
        public string UniqueId { get; set; }
        public string UniqueIdType { get; set; }

    }
    public class InvestTransaction
    {        
        public string TransactionId { get; set; }
        
        public DateTime TradeDate { get; set; }
       
        public DateTime SettleDate { get; set; }

    }
    public class FundInfo
    {
        
        public decimal UnitPrice { get; set; }
        
        public string UniqueId { get; set; }
      
        public string UniqueIdType { get; set; }
       
        public string FiId { get; set; }
       
        public string Name { get; set; }
      
        public string Ticker { get; set; }
       
        public string Memo { get; set; }
      
        public string FundType { get; set; }
    }

    public class Position
    {
        public string UniqueId { get; set; }
        public string UniqueIdType { get; set; }
        public string SubAccountType { get; set; }
        public string PositionType { get; set; }
        public decimal Units { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MarketValue { get; set; }
        public decimal AverageCostBasis { get; set; }
        public DateTime PriceAsOf { get; set; }
        public string Currency { get; set; }
        public string Memo { get; set; }
        public string SourceOfMoney { get; set; }

    }
}
