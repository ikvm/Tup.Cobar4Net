﻿/*
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

using Sharpen;
using System.Threading;
using Tup.Cobar.Parser.Ast.Expression.Misc;
using Tup.Cobar.Parser.Ast.Stmt.Dml;
using Tup.Cobar.Parser.Recognizer.Mysql.Lexer;

namespace Tup.Cobar.Parser.Recognizer.Mysql.Syntax
{
    /// <author><a href="mailto:shuo.qius@alibaba-inc.com">QIU Shuo</a></author>
    [NUnit.Framework.TestFixture(Category = "MySQLDMLSelectParserTest")]
    public class MySQLDMLSelectParserTest : AbstractSyntaxTest
    {
        //[NUnit.Framework.Test]
        //public virtual void TestMain()
        //{
        //    Main(null);
        //    NUnit.Framework.Assert.IsTrue(true);
        //}
        /// <exception cref="System.Exception"/>
        public static void Main(string[] ars)
        {
            string sql = Performance.SqlBenchmarkSelect;
            for (int i = 0; i < 3; ++i)
            {
                MySQLLexer lexer = new MySQLLexer(sql);
                MySQLExprParser exprParser = new MySQLExprParser(lexer);
                MySQLDMLSelectParser parser = new MySQLDMLSelectParser(lexer, exprParser);
                QueryExpression stmt = parser.Select();
            }

            // System.out.println(stmt);
            Thread.Sleep(1000);
            long loop = 300 * 10000;
            long t1 = Runtime.CurrentTimeMillis();
            t1 = Runtime.CurrentTimeMillis();
            for (long i_1 = 0; i_1 < loop; ++i_1)
            {
                MySQLLexer lexer = new MySQLLexer(sql);
                MySQLExprParser exprParser = new MySQLExprParser(lexer);
                MySQLDMLSelectParser parser = new MySQLDMLSelectParser(lexer, exprParser);
                QueryExpression stmt = parser.Select();
            }
            long t2 = Runtime.CurrentTimeMillis();
            System.Console.Out.WriteLine((t2 - t1) * 1000.0d / loop + " us");
        }

        /// <exception cref="System.Data.Sql.SQLSyntaxErrorException"/>
        [NUnit.Framework.Test]
        public virtual void TestSelectUnion()
        {
            string sql = "(select id from t1) union all (select id from t2) union all (select id from t3) ordeR By d desC limit 1 offset ?";
            MySQLLexer lexer = new MySQLLexer(sql);
            MySQLDMLSelectParser parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser
                (lexer));
            DMLSelectUnionStatement select = (DMLSelectUnionStatement)parser.SelectUnion();
            NUnit.Framework.Assert.AreEqual(0, select.GetFirstDistinctIndex());
            NUnit.Framework.Assert.AreEqual(3, select.GetSelectStmtList().Count);
            string output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("(SELECT id FROM t1) UNION ALL (SELECT id FROM t2) UNION ALL (SELECT id FROM t3) ORDER BY d DESC LIMIT ?, 1"
                , output);
            sql = "(select id from t1) union  select id from t2 order by id union aLl (select id from t3) ordeR By d asC";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = (DMLSelectUnionStatement)parser.SelectUnion();
            NUnit.Framework.Assert.AreEqual(1, select.GetFirstDistinctIndex());
            NUnit.Framework.Assert.AreEqual(3, select.GetSelectStmtList().Count);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("(SELECT id FROM t1) UNION (SELECT id FROM t2 ORDER BY id) UNION ALL (SELECT id FROM t3) ORDER BY d"
                , output);
            sql = "(select id from t1) union distInct (select id from t2) union  select id from t3";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = (DMLSelectUnionStatement)parser.SelectUnion();
            NUnit.Framework.Assert.AreEqual(2, select.GetFirstDistinctIndex());
            NUnit.Framework.Assert.AreEqual(3, select.GetSelectStmtList().Count);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("(SELECT id FROM t1) UNION (SELECT id FROM t2) UNION (SELECT id FROM t3)"
                , output);
        }

        /// <exception cref="System.Data.Sql.SQLSyntaxErrorException"/>
        [NUnit.Framework.Test]
        public virtual void TestSelect()
        {
            string sql = "SELect t1.id , t2.* from t1, test.t2 where test.t1.id=1 and t1.id=test.t2.id";
            MySQLLexer lexer = new MySQLLexer(sql);
            MySQLDMLSelectParser parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser
                (lexer));
            DMLSelectStatement select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            string output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT t1.id, t2.* FROM t1, test.t2 WHERE test.t1.id = 1 AND t1.id = test.t2.id"
                , output);
            sql = "select * from  offer  a  straight_join wp_image b use key for join(t1,t2) on a.member_id=b.member_id inner join product_visit c where a.member_id=c.member_id and c.member_id='abc' ";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT * FROM offer AS A STRAIGHT_JOIN wp_image AS B USE KEY FOR JOIN (t1, t2) ON a.member_id = b.member_id INNER JOIN product_visit AS C WHERE a.member_id = c.member_id AND c.member_id = 'abc'"
                , output);
            sql = "SELect all tb1.id,tb2.id from tb1,tb2 where tb1.id2=tb2.id2";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT tb1.id, tb2.id FROM tb1, tb2 WHERE tb1.id2 = tb2.id2"
                , output);
            sql = "SELect distinct high_priority tb1.id,tb2.id from tb1,tb2 where tb1.id2=tb2.id2";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT DISTINCT HIGH_PRIORITY tb1.id, tb2.id FROM tb1, tb2 WHERE tb1.id2 = tb2.id2"
                , output);
            sql = "SELect distinctrow high_priority sql_small_result tb1.id,tb2.id " + "from tb1,tb2 where tb1.id2=tb2.id2";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT DISTINCTROW HIGH_PRIORITY SQL_SMALL_RESULT tb1.id, tb2.id FROM tb1, tb2 WHERE tb1.id2 = tb2.id2"
                , output);
            sql = "SELect  sql_cache id1,id2 from tb1,tb2 where tb1.id1=tb2.id1 ";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT SQL_CACHE id1, id2 FROM tb1, tb2 WHERE tb1.id1 = tb2.id1"
                , output);
            sql = "SELect  sql_cache id1,max(id2) from tb1 group by id1 having id1>10 order by id3 desc";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT SQL_CACHE id1, MAX(id2) FROM tb1 GROUP BY id1 HAVING id1 > 10 ORDER BY id3 DESC"
                , output);
            sql = "SELect  SQL_BUFFER_RESULT tb1.id1,id2 from tb1";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT SQL_BUFFER_RESULT tb1.id1, id2 FROM tb1",
                output);
            sql = "SELect  SQL_no_cache tb1.id1,id2 from tb1";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT SQL_NO_CACHE tb1.id1, id2 FROM tb1", output
                );
            sql = "SELect  SQL_CALC_FOUND_ROWS tb1.id1,id2 from tb1";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT SQL_CALC_FOUND_ROWS tb1.id1, id2 FROM tb1"
                , output);
            sql = "SELect 1+1 ";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT 1 + 1", output);
            sql = "SELect t1.* from tb ";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT t1.* FROM tb", output);
            sql = "SELect distinct high_priority straight_join sql_big_result sql_cache tb1.id,tb2.id "
                 + "from tb1,tb2 where tb1.id2=tb2.id2";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT DISTINCT HIGH_PRIORITY STRAIGHT_JOIN SQL_BIG_RESULT"
                 + " SQL_CACHE tb1.id, tb2.id FROM tb1, tb2 WHERE tb1.id2 = tb2.id2", output);
            sql = "SELect distinct id1,id2 from tb1,tb2 where tb1.id1=tb2.id2 for update";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT DISTINCT id1, id2 FROM tb1, tb2 WHERE tb1.id1 = tb2.id2 FOR UPDATE"
                , output);
            sql = "SELect distinct id1,id2 from tb1,tb2 where tb1.id1=tb2.id2 lock in share mode";
            lexer = new MySQLLexer(sql);
            parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser(lexer));
            select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT DISTINCT id1, id2 FROM tb1, tb2 WHERE tb1.id1 = tb2.id2 LOCK IN SHARE MODE"
                , output);
        }

        /// <exception cref="System.Data.Sql.SQLSyntaxErrorException"/>
        [NUnit.Framework.Test]
        public virtual void TestSelectChinese()
        {
            string sql = "SELect t1.id , t2.* from t1, test.t2 where test.t1.id='中''‘文'' and t1.id=test.t2.id";
            MySQLLexer lexer = new MySQLLexer(sql);
            MySQLDMLSelectParser parser = new MySQLDMLSelectParser(lexer, new MySQLExprParser
                (lexer));
            DMLSelectStatement select = parser.Select();
            NUnit.Framework.Assert.IsNotNull(select);
            string output = Output2MySQL(select, sql);
            NUnit.Framework.Assert.AreEqual("SELECT t1.id, t2.* FROM t1, test.t2 WHERE test.t1.id = '中\\'‘文'' AND t1.id = test.t2.id"
                , output);
        }
    }
}