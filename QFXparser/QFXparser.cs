using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QFXparser
{
    public class FileParser
    {
        private string _fileText;
        private RawLedgerBalance _ledgerBalance;
        private readonly CultureInfo _cultureInfo = CultureInfo.CurrentCulture;
        bool isRawTransaction = false;
        bool isRawReinvestTransaction = false;
        bool isRawSecurity = false;
        /// <summary>
        /// Initialize a FileParser with UTF-8 encoding and
        /// current culture info.
        /// </summary>
        /// <param name="fileNamePath"></param>
        public FileParser(string fileNamePath)
        {
            using (StreamReader sr = new StreamReader(fileNamePath,true))
            {
                _fileText = sr.ReadToEnd();
            }

        }

        /// <summary>
        /// Initialize a FileParser with UTF-8 encoding and current culture info.
        /// </summary>
        /// <param name="fileStream"></param>
        public FileParser(Stream fileStream)
        {
            using (StreamReader sr = new StreamReader(fileStream, true))
            {
                _fileText = sr.ReadToEnd();
            }

        }

        /// <summary>
        /// Initialize a FileParser with invariant culture info.
        /// </summary>
        /// <param name="streamReader"></param>
        public FileParser(StreamReader streamReader):this(streamReader, CultureInfo.InvariantCulture)
        {
        }

        /// <summary>
        /// Initialize a FileParser
        /// </summary>
        /// <param name="streamReader"></param>
        /// <param name="cultureInfo"></param>
        public FileParser(StreamReader streamReader, CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
            _fileText = streamReader.ReadToEnd();
        }

        public Statement BuildStatement()
        {
            RawStatement rawStatement = BuildRaw();

            Statement statement = new Statement
            {
                AccountNum = rawStatement.AccountNum
            };

            foreach (var rawTrans in rawStatement.Transactions)
            {
                Transaction trans = new Transaction
                {
                    Amount = rawTrans.Amount,
                    Memo = rawTrans.Memo,
                    Name = rawTrans.Name,
                    PostedOn = rawTrans.PostedOn,
                    RefNumber = rawTrans.RefNumber,
                    TransactionId = rawTrans.TransactionId,
                    Type = rawTrans.Type
                };
                statement.Transactions.Add(trans);
            }
            foreach (var rawReinvest in rawStatement.ReinvTransactions)
            {
                //RawInvestTransaction investTransaction = rawReinvest.InvestTransaction;
                ReinvestTransaction reinvest = new ReinvestTransaction
                {
                    SettleDate = rawReinvest.SettleDate,
                    TradeDate = rawReinvest.TradeDate,
                    TransactionId = rawReinvest.TransactionId,                    
                    SubType = rawReinvest.SubType,
                    Total = rawReinvest.Total,
                    UnitPrice = rawReinvest.UnitPrice,
                    Units = rawReinvest.Units,
                    UniqueId = rawReinvest.UniqueId,
                    IncomeType = rawReinvest.IncomeType,
                    UniqueIdType = rawReinvest.UniqueIdType,

                };
                statement.ReinvestTransactions.Add(reinvest);
            }
            foreach (var item in rawStatement.SecurityList)
            {
                FundInfo fundInfo = new FundInfo
                {
                    FiId = item.FiId,
                    FundType = item.FundType,
                    Memo = item.Memo,
                    Name = item.Name,
                    Ticker = item.Ticker,
                    UniqueId = item.UniqueId,
                    UniqueIdType = item.UniqueIdType,
                    UnitPrice = item.UnitPrice,

                };
                statement.FundInfos.Add(fundInfo);
            }
            statement.LedgerBalance = new LedgerBalance
            {
                Amount = rawStatement.LedgerBalance.Amount,
                AsOf = rawStatement.LedgerBalance.AsOf
            };

            return statement;
        }

        private RawStatement BuildRaw()
        {
            RawStatement _statement = null;
            MemberInfo currentMember = null;
            RawTransaction _currentTransaction = null;
            RawReinvestTransaction rawReinvestTransaction = null;
            RawSecurity currentSecurity = null;

            foreach (var token in Parser.Parse(_fileText))
            {
                if (token.IsElement)
                {
                    if (token.Content == "BANKTRANLIST")
                    {
                        isRawReinvestTransaction = false;
                        isRawSecurity = false;
                        isRawTransaction = true;
                        
                    }
                    if (token.Content == "INVTRANLIST")
                    {
                        isRawReinvestTransaction = true;
                        isRawTransaction = false;
                        isRawSecurity = false;
                    }
                    if (token.Content == "SECLIST")
                    {
                        isRawReinvestTransaction = false;
                        isRawTransaction = false;
                        isRawSecurity = true;
                    }
                    var result = GetPropertyInfo(token.Content);
                    if (result != null)
                    {
                        switch (result.Type)
                        {
                            case NodeType.StatementOpen:
                                _statement = new RawStatement();
                                break;
                            case NodeType.StatementClose:
                                return _statement;
                                break;
                            case NodeType.TransactionOpen:
                                _currentTransaction = new RawTransaction();
                               
                                break;
                            case NodeType.TransactionClose:
                                _statement.Transactions.Add(_currentTransaction);
                                _currentTransaction = null;
                                break;
                            case NodeType.ReinvestTransactionOpen:
                                rawReinvestTransaction = new RawReinvestTransaction();
                              
                                break;
                            case NodeType.ReinvestTransactionClose:
                                _statement.ReinvTransactions.Add(rawReinvestTransaction);
                                rawReinvestTransaction = null;
                                break;
                            case NodeType.SecurityListOpen:
                                currentSecurity = new RawSecurity();

                                break;
                            case NodeType.SecurityListClose:
                                _statement.SecurityList.Add(currentSecurity);
                                currentSecurity = null;
                                break;
                            case NodeType.StatementProp:
                                if (_statement == null)
                                {
                                    _statement = new RawStatement();
                                }
                                currentMember = result.Member;
                                break;
                            case NodeType.TransactionProp:
                                currentMember = result.Member;
                                break;
                            case NodeType.ReinvestTransactionProp:
                                currentMember = result.Member;
                                break;
                            case NodeType.SecurityListProp:
                                currentMember = result.Member;
                                break;
                            case NodeType.LedgerBalanceOpen:
                                _ledgerBalance = new RawLedgerBalance();
                                break;
                            case NodeType.LedgerBalanceClose:
                                _statement.LedgerBalance.Amount = _ledgerBalance.Amount;
                                _statement.LedgerBalance.AsOf = _ledgerBalance.AsOf;
                                break;
                            case NodeType.LedgerBalanceProp:
                                if (_ledgerBalance == null)
                                {
                                    _ledgerBalance = new RawLedgerBalance();
                                }
                                currentMember = result.Member;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        currentMember = null;
                    }
                }
                else
                {
                    if (currentMember != null && currentMember is PropertyInfo)
                    {
                        var property = (PropertyInfo)currentMember;
                        switch (property.DeclaringType.Name)
                        {
                            case "RawStatement":
                                property.SetValue(_statement, ConvertQfxType(token.Content, property.PropertyType));
                                break;
                            case "RawTransaction":
                                if (_currentTransaction != null)
                                {
                                    property.SetValue(_currentTransaction, ConvertQfxType(token.Content, property.PropertyType));
                                }

                                break;
                            case "RawReinvestTransaction":
                                if (rawReinvestTransaction != null)
                                {
                                    property.SetValue(rawReinvestTransaction, ConvertQfxType(token.Content, property.PropertyType));
                                }

                                break;
                            case "RawSecurity":
                                if (currentSecurity != null)
                                {
                                    property.SetValue(currentSecurity, ConvertQfxType(token.Content, property.PropertyType));
                                }

                                break;
                            case "RawLedgerBalance":
                                property.SetValue(_ledgerBalance, ConvertQfxType(token.Content, property.PropertyType));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return _statement;
        }

        private object ConvertQfxType(string content, Type targetType)
        {
            object result;
            if (targetType == typeof(DateTime))
            {
                result = ParsingHelper.ParseDate(content);
            }
            else
            {
                try
                {
                    result = Convert.ChangeType(content, targetType, _cultureInfo);
                }
                catch (Exception)
                {
                    result = targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
                }
            }
            return result;
        }

        private PropertyResult GetPropertyInfo(string token)
        {
            var propertyResult = new PropertyResult();

            if (typeof(RawStatement).GetCustomAttribute<NodeNameAttribute>().CloseTag == token)
            {
                propertyResult.Member = typeof(RawStatement);
                propertyResult.Type = NodeType.StatementClose;
                return propertyResult;
            }

            if (isRawTransaction)
            {
                if (typeof(RawTransaction).GetCustomAttribute<NodeNameAttribute>().CloseTag == token)
                {
                    propertyResult.Member = typeof(RawTransaction);
                    propertyResult.Type = NodeType.TransactionClose;
                    return propertyResult;
                } 
            }
            if (isRawReinvestTransaction)
            {
                if (typeof(RawReinvestTransaction).GetCustomAttribute<NodeNameAttribute>().CloseTag == token)
                {
                    propertyResult.Member = typeof(RawReinvestTransaction);
                    propertyResult.Type = NodeType.ReinvestTransactionClose;
                    return propertyResult;
                } 
            }
            if (isRawSecurity)
            {
                if (typeof(RawSecurity).GetCustomAttribute<NodeNameAttribute>().CloseTag == token)
                {
                    propertyResult.Member = typeof(RawSecurity);
                    propertyResult.Type = NodeType.SecurityListClose;
                    return propertyResult;
                }
            }
            if (typeof(RawStatement).GetCustomAttribute<NodeNameAttribute>().OpenTag == token)
            {
                propertyResult.Member = typeof(RawStatement);
                propertyResult.Type = NodeType.StatementOpen;
                return propertyResult;
            }

            if (isRawTransaction)
            {
                if (typeof(RawTransaction).GetCustomAttribute<NodeNameAttribute>().OpenTag == token)
                {
                    propertyResult.Member = typeof(RawTransaction);
                    propertyResult.Type = NodeType.TransactionOpen;
                    return propertyResult;
                } 
            }
            if (isRawReinvestTransaction)
            {
                if (typeof(RawReinvestTransaction).GetCustomAttribute<NodeNameAttribute>().OpenTag == token)
                {
                    propertyResult.Member = typeof(RawReinvestTransaction);
                    propertyResult.Type = NodeType.ReinvestTransactionOpen;
                    return propertyResult;
                } 
            }
            if (isRawSecurity)
            {
                if (typeof(RawSecurity).GetCustomAttribute<NodeNameAttribute>().OpenTag == token)
                {
                    propertyResult.Member = typeof(RawSecurity);
                    propertyResult.Type = NodeType.SecurityListOpen;
                    return propertyResult;
                }
            }
            if (typeof(RawLedgerBalance).GetCustomAttribute<NodeNameAttribute>().OpenTag == token)
            {
                propertyResult.Member = typeof(RawLedgerBalance);
                propertyResult.Type = NodeType.LedgerBalanceOpen;
                return propertyResult;
            }

            if (typeof(RawLedgerBalance).GetCustomAttribute<NodeNameAttribute>().CloseTag == token)
            {
                propertyResult.Member = typeof(RawLedgerBalance);
                propertyResult.Type = NodeType.LedgerBalanceClose;
                return propertyResult;
            }

            var statementMember = typeof(RawStatement).GetProperties().FirstOrDefault(m => m.GetCustomAttribute<NodeNameAttribute>().OpenTag == token);

            if (statementMember != null)
            {
                propertyResult.Member = statementMember;
                propertyResult.Type = NodeType.StatementProp;
                return propertyResult;
            }

            if (isRawTransaction || (!isRawReinvestTransaction && !isRawSecurity) )
            {
                var transactionMember = typeof(RawTransaction).GetProperties().Where(m => m.GetCustomAttribute<NodeNameAttribute>() != null)
                 .FirstOrDefault(m => m.GetCustomAttribute<NodeNameAttribute>().OpenTag == token);

                if (transactionMember != null)
                {
                    propertyResult.Member = transactionMember;
                    propertyResult.Type = NodeType.TransactionProp;
                    return propertyResult;
                } 
            }
            if (isRawReinvestTransaction || (!isRawTransaction && !isRawSecurity))
            {
                var reinvestTransactionMember = typeof(RawReinvestTransaction).GetProperties()
                        .Where(m => m.GetCustomAttribute<NodeNameAttribute>() != null)
                       .FirstOrDefault(m => m.GetCustomAttribute<NodeNameAttribute>().OpenTag == token);

                if (reinvestTransactionMember != null)
                {
                    propertyResult.Member = reinvestTransactionMember;
                    propertyResult.Type = NodeType.ReinvestTransactionProp;
                    return propertyResult;
                } 
            }
            if (isRawSecurity)
            {
                var securityInfo = typeof(RawSecurity).GetProperties()
                       .Where(m => m.GetCustomAttribute<NodeNameAttribute>() != null)
                      .FirstOrDefault(m => m.GetCustomAttribute<NodeNameAttribute>().OpenTag == token);

                if (securityInfo != null)
                {
                    propertyResult.Member = securityInfo;
                    propertyResult.Type = NodeType.SecurityListProp;
                    return propertyResult;
                }
            }
            var balanceMember = typeof(RawLedgerBalance).GetProperties().Where(m => m.GetCustomAttribute<NodeNameAttribute>() != null)
                .FirstOrDefault(m => m.GetCustomAttribute<NodeNameAttribute>().OpenTag == token);

            if (balanceMember != null)
            {
                propertyResult.Member = balanceMember;
                propertyResult.Type = NodeType.LedgerBalanceProp;
                return propertyResult;
            }

            return null;
        }
    }    
}
