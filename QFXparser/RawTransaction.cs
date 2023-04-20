using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QFXparser
{
    [NodeName("STMTTRN", "/STMTTRN")]
    internal class RawTransaction
    {
        [NodeName("TRNTYPE")]
        public string Type { get; set; }

        [NodeName("DTPOSTED")]
        public DateTime PostedOn { get; set; }

        [NodeName("TRNAMT")]
        public Decimal Amount { get; set; }

        [NodeName("FITID")]
        public string TransactionId { get; set; }

        [NodeName("REFNUM")]
        public string RefNumber { get; set; }

        [NodeName("NAME")]
        public string Name { get; set; }

        [NodeName("MEMO")]
        public string Memo { get; set; }
    }
    [NodeName("REINVEST","/REINVEST")]
    internal class RawReinvestTransaction
    {
       
      //  public RawInvestTransaction InvestTransaction { get; set; }= new RawInvestTransaction();
        [NodeName("FITID")]
        public string TransactionId { get; set; }
        [NodeName("DTTRADE")]
        public DateTime TradeDate { get; set; }
        [NodeName("DTSETTLE")]
        public DateTime SettleDate { get; set; }
        [NodeName("INCOMETYPE")]
        public string IncomeType { get; set; }
        [NodeName("TOTAL")]
        public decimal Total { get; set; }
        [NodeName("SUBACCTSEC")]
        public string SubType { get; set; }
        [NodeName("UNITS")]
        public decimal Units { get; set; }
        [NodeName("UNITPRICE")]
        public decimal UnitPrice { get; set; }
        [NodeName("UNIQUEID")]
        public int UniqueId { get; set; }
        [NodeName("UNIQUEIDTYPE")]
        public string UniqueIdType { get; set; }

    }
    [NodeName("INVTRAN", "/INVTRAN")]
    internal class RawInvestTransaction
    {

        [NodeName("FITID")]
        public string TransactionId { get; set; }
        [NodeName("DTTRADE")]
        public DateTime TradeDate { get; set; }
        [NodeName("DTSETTLE")]
        public DateTime SettleDate { get; set; }

    }

}
