// Copyright (c) 2012 Hammura.bi LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace Hammurabi
{ 
    
    /// <summary>
    /// Represents tangible or intangible property, i.e. something that can
    /// be owned.  Synonymous with "asset."
    /// </summary>
    public class Property : LegalEntity
    {
        public string Type;             // Nature of the property (e.g. currency, real estate, etc.)
        public Tnum ValueInDollars;     // Value of the property
        public Tstr Owner;              // Id of the owner
        public Tstr Location;           // Jurisdiction where the property is located
        
        public Property(string type, Tnum valueUSD)
        {
            Type = type;
            ValueInDollars = valueUSD;
        }
        
        public Property(string identifier)
        {
            Id = identifier;
        }
    }
    
    /// <summary>
    /// Represents a transfer of something from one legal entity to another.
    /// </summary>
    public class Transfer : LegalEntity
    {      
        public LegalEntity Transferor;     // Giver
        public LegalEntity Transferee;     // Receiver
        public Property Item;              // Item that was transferred
        public DateTime Date;              // Date of the transfer
        public string Method;              // How the item was transferred?
        
        public Transfer(LegalEntity transferor, LegalEntity transferee, Property item, DateTime date)
        {
            Transferor = transferor;
            Transferee = transferee;
            Item = item;
            Date = date;
        }
    }
    
    /// <summary>
    /// Represents a transaction or exchange between two legal entities.
    /// </summary>
    /// <remarks>
    /// A transaction is composed of one or more transfers.
    /// </remarks>
    public class Transaction : LegalEntity
    {
        public List<Transfer> Transfers;    // Component asset transfers making up the transaction
        public Tstr Purpose;              // Purpose, description, or nature of the transaction
        public Tstr ThingSold;            // Description of the thing sold, if transaction is a sale
        public Tnum PurchasePrice;        // Purchase price of thing sold, if transaction is a sale
        public DateTime DateOfSale;       // Date sale occurred, if there was one
        public DateTime DateBalanceDue;   // Date balance is due on a sale, if there was a sale

        public Transaction(params Transfer[] transfers)
        {
            foreach (Transfer t in transfers)
            {
                Transfers.Add(t);
            }
        }
        
        // TODO:
        public DateTime DateOfInitialPayment(LegalEntity recipient)
        {
            return Time.EndOf;
        }
    }
    
}



