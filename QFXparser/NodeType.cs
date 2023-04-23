using System;
using System.Collections.Generic;
using System.Text;

namespace QFXparser
{
    internal enum NodeType
    {
        StatementOpen,
        StatementClose,
        TransactionOpen,
        TransactionClose,
        StatementProp,
        TransactionProp,
        LedgerBalanceOpen,
        LedgerBalanceClose,
        LedgerBalanceProp,
        ReinvestTransactionOpen,
        ReinvestTransactionClose,
        ReinvestTransactionProp,
        SecurityListOpen,
        SecurityListClose,
        SecurityListProp,
        StockOpen,
        StockClose,
        StockProp,
    }
    internal enum TransactionType
    {
        Unknown,
        Transaction,
        Reinvest,
        Security,
        Stock,
    }
}
