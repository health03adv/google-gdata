/* Copyright (c) 2006 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Xml;
using System.IO;
using System.Collections;
using Google.GData.Client;
using Google.GData.Extensions;

namespace Google.GData.Spreadsheets
{
    /// <summary>
    /// Entry API customization class for defining entries in a Worksheets feed.
    /// </summary>
    public class WorksheetEntry : AbstractEntry
    {
        /// <summary>
        /// Category used to label entries that contain Cell extension data.
        /// </summary>
        public static AtomCategory WORKSHEET_CATEGORY
        = new AtomCategory(GDataSpreadsheetsNameTable.Worksheet,
                           new AtomUri(BaseNameTable.gKind));

        private ColCountElement colCount;
        private RowCountElement rowCount;

        /// <summary>
        /// Constructs a new WorksheetEntry instance with the appropriate category
        /// to indicate that it is a worksheet.
        /// </summary>
        public WorksheetEntry() : base()
        {
            Categories.Add(WORKSHEET_CATEGORY);
        }

        /// <summary>
        /// Constructs a new WorksheetEntry instance with the given number of
        /// rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of columns.</param>
        public WorksheetEntry(uint rows, uint cols) : this()
        {
            this.ColCount = new ColCountElement(cols);

            this.RowCount = new RowCountElement(rows);
        }

        /// <summary>
        /// Constructs a new WorksheetEntry instance with a given number of
        /// rows and columns and with a set title.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of columns.</param>
        /// <param name="title">The title of the worksheet.</param>
        public WorksheetEntry(uint rows, uint cols, string title) : this(rows,cols)
        {
            this.Title = new AtomTextConstruct(AtomTextConstructElementType.Title,title);
        }

        /// <summary>
        /// The colCount element in this worksheet entry
        /// </summary>
        public ColCountElement ColCount
        {
            get { return colCount;}
            set
            {
                if (colCount != null)
                {
                    ExtensionElements.Remove(colCount);
                }
                colCount = value; 
                ExtensionElements.Add(colCount);
            }
        }

        /// <summary>
        /// Sets the number of columns for this worksheet entry
        /// </summary>
        public uint Cols
        {
            get { return this.ColCount.Count; }
            set { this.ColCount.Count = value; }
        }


        /// <summary>
        /// The rowCount element in this cell entry
        /// </summary>
        public RowCountElement RowCount
        {
            get { return rowCount;}
            set
            {
                if (rowCount != null)
                {
                    ExtensionElements.Remove(rowCount);
                }
                rowCount = value; 
                ExtensionElements.Add(rowCount);
            }
        }

        /// <summary>
        /// Sets the number of rows for this worksheet entry
        /// </summary>
        public uint Rows
        {
            get { return this.RowCount.Count; }
            set { this.RowCount.Count = value; }
        }

        /// <summary>
        /// Parses the inner state of the element
        /// </summary>
        /// <param name="e">the event arguments</param>
        /// <param name="parser">the atom feed parser calling</param>
        public override void Parse(ExtensionElementEventArgs e, AtomFeedParser parser)
        {
            XmlNode worksheetNode = e.ExtensionElement;

            if (String.Compare(worksheetNode.NamespaceURI, GDataSpreadsheetsNameTable.NSGSpreadsheets, true) == 0)
            {
                if (worksheetNode.LocalName == GDataSpreadsheetsNameTable.XmlColCountElement)
                {
                    ColCount = ColCountElement.ParseColCount(worksheetNode);
                    e.DiscardEntry = true;
                }
                else if (worksheetNode.LocalName == GDataSpreadsheetsNameTable.XmlRowCountElement)
                {
                    RowCount = RowCountElement.ParseRowCount(worksheetNode);
                    e.DiscardEntry = true;
                }
            }
        }
    }
}