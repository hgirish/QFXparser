using System.Collections.Generic;

namespace QFXparser
{
    public class Statement
    {
        public string AccountNum { get; set; }
        public string Fid { get; set; }

        public string Organization { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
            = new List<Transaction>();
        public LedgerBalance LedgerBalance { get; set; }
            = new LedgerBalance();
        public ICollection<ReinvestTransaction> ReinvestTransactions { get; set; }
            = new List<ReinvestTransaction>();
        public ICollection<FundInfo> FundInfos { get; set; }
            = new List<FundInfo>();
        public ICollection<Position> PositionList { get; set; }
        = new List<Position>();
    }
}
