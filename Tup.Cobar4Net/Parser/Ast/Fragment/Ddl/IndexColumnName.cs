/*
* Copyright 1999-2012 Alibaba Group.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using Tup.Cobar4Net.Parser.Ast.Expression;
using Tup.Cobar4Net.Parser.Ast.Expression.Primary;
using Tup.Cobar4Net.Parser.Visitor;

namespace Tup.Cobar4Net.Parser.Ast.Fragment.Ddl
{
    /// <author>
    ///     <a href="mailto:shuo.qius@alibaba-inc.com">QIU Shuo</a>
    /// </author>
    public class IndexColumnName : IAstNode
    {
        public IndexColumnName(Identifier columnName, IExpression length, bool asc)
        {
            ColumnName = columnName;
            Length = length;
            IsAsc = asc;
        }

        public virtual Identifier ColumnName { get; }

        public virtual IExpression Length { get; }

        public virtual bool IsAsc { get; }

        public virtual void Accept(ISqlAstVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}