using System;
using System.Collections.Generic;
using System.Text;

namespace QFXparser
{
    [NodeName("CCSTMTRS")]
    internal class RawStatement
    {
        [NodeName("ACCTID")]
        public string AccountNum { get; set; }
        [NodeName("ORG")]
        public string Organization { get; set; }
        [NodeName("FID")]
        public string Fid { get; set; }

        [NodeName("BANKTRANLIST")]
        public ICollection<RawTransaction> Transactions { get; set; } = new List<RawTransaction>();

        [NodeName("LEDGERBAL")]
        public RawLedgerBalance LedgerBalance { get; set; } = new RawLedgerBalance();
        [NodeName("INVTRANLIST")]
        public ICollection<RawReinvestTransaction> ReinvTransactions { get; set; } = new List<RawReinvestTransaction>();
        [NodeName("SECLIST")]
        public ICollection<RawSecurity> SecurityList { get; set; } = new List<RawSecurity>();
        [NodeName("SECLIST")]
        public ICollection<RawStock> StockList { get; set; } = new List<RawStock>();
        [NodeName("INVPOSLIST")]
        public ICollection<RawPosition> PositionList { get; set; } = new List<RawPosition>();
    }
}
