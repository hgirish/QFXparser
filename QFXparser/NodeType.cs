﻿using System;
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
    }
    internal enum TransactionType
    {
        Transaction,
        Reinvest,
        Security,
    }
}
