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

using NUnit.Framework;
using Tup.Cobar4Net.Parser.Util;
using Tup.Cobar4Net.Route.Hint;

namespace Tup.Cobar4Net.Route
{
    /// <author><a href="mailto:shuo.qius@alibaba-inc.com">QIU Shuo</a></author>
    [TestFixture(Category = "CobarHintTest")]
    public class CobarHintTest
    {
        /// <exception cref="System.Exception"/>
        [Test]
        public virtual void TestHint1()
        {
            string sql = "  /*!cobar: $dataNodeId =2.1, $table='offer'*/ select * ";
            CobarHint hint = CobarHint.ParserCobarHint(sql, 2);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual(1, hint.GetDataNodes().Count);
            Assert.AreEqual(new Pair<int, int>(2, 1), hint.GetDataNodes()[0]);
            sql = "  /*!cobar: $dataNodeId=0.0, $table='offer'*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual(1, hint.GetDataNodes().Count);
            Assert.AreEqual(new Pair<int, int>(0, 0), hint.GetDataNodes()[0]);
            sql = "  /*!cobar: $dataNodeId=0, $table='offer'*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual(1, hint.GetDataNodes().Count);
            //INFO Assert.AreEqual(new Pair<int, int>(0, null), hint.GetDataNodes()[0]);
            Assert.AreEqual(new Pair<int, int>(0, -1), hint.GetDataNodes()[0]);
            sql = "/*!cobar: $dataNodeId   = [ 1,2,5.2]  , $table =  'offer'   */ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual(3, hint.GetDataNodes().Count);
            sql = "/*!cobar: $partitionOperand=( 'member_id' = 'm1'), $table='offer'*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Pair<string[], object[][]> pair = hint.GetPartitionOperand();
            Assert.AreEqual(1, pair.GetKey().Length);
            Assert.AreEqual("MEMBER_ID", pair.GetKey()[0]);
            Assert.AreEqual(1, pair.GetValue().Length);
            Assert.AreEqual(1, pair.GetValue()[0].Length);
            Assert.AreEqual("m1", pair.GetValue()[0][0]);
            Assert.IsNull(hint.GetDataNodes());
            sql = "/*!cobar:$partitionOperand =   ( 'member_id' = ['m1'  ,   'm2' ]  ), $table='offer'  , $replica=  2*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual(2, hint.GetReplica());
            pair = hint.GetPartitionOperand();
            Assert.AreEqual(1, pair.GetKey().Length);
            Assert.AreEqual("MEMBER_ID", pair.GetKey()[0]);
            Assert.AreEqual(2, pair.GetValue().Length);
            Assert.AreEqual(1, pair.GetValue()[0].Length);
            Assert.AreEqual("m1", pair.GetValue()[0][0]);
            Assert.AreEqual("m2", pair.GetValue()[1][0]);
            Assert.IsNull(hint.GetDataNodes());
            sql = "/*!cobar:$partitionOperand=('member_id'=['m1', 'm2']),$table='offer',$replica=2*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual(2, hint.GetReplica());
            pair = hint.GetPartitionOperand();
            Assert.AreEqual(1, pair.GetKey().Length);
            Assert.AreEqual("MEMBER_ID", pair.GetKey()[0]);
            Assert.AreEqual(2, pair.GetValue().Length);
            Assert.AreEqual(1, pair.GetValue()[0].Length);
            Assert.AreEqual("m1", pair.GetValue()[0][0]);
            Assert.AreEqual("m2", pair.GetValue()[1][0]);
            Assert.IsNull(hint.GetDataNodes());
            sql = "/*!cobar:$partitionOperand = ( ['offer_id','group_id'] = [123,'3c']), $table='offer'*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual((int)RouteResultsetNode.DefaultReplicaIndex, hint
                .GetReplica());
            pair = hint.GetPartitionOperand();
            Assert.AreEqual(2, pair.GetKey().Length);
            Assert.AreEqual("OFFER_ID", pair.GetKey()[0]);
            Assert.AreEqual("GROUP_ID", pair.GetKey()[1]);
            Assert.AreEqual(1, pair.GetValue().Length);
            Assert.AreEqual(2, pair.GetValue()[0].Length);
            Assert.AreEqual(123L, pair.GetValue()[0][0]);
            Assert.AreEqual("3c", pair.GetValue()[0][1]);
            Assert.IsNull(hint.GetDataNodes());
            sql = "/*!cobar:$partitionOperand=(['offer_id' , 'group_iD' ]=[ 123 , '3c' ]) ,$table = 'offer'*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual((int)RouteResultsetNode.DefaultReplicaIndex, hint
                .GetReplica());
            pair = hint.GetPartitionOperand();
            Assert.AreEqual(2, pair.GetKey().Length);
            Assert.AreEqual("OFFER_ID", pair.GetKey()[0]);
            Assert.AreEqual("GROUP_ID", pair.GetKey()[1]);
            Assert.AreEqual(1, pair.GetValue().Length);
            Assert.AreEqual(2, pair.GetValue()[0].Length);
            Assert.AreEqual(123L, pair.GetValue()[0][0]);
            Assert.AreEqual("3c", pair.GetValue()[0][1]);
            Assert.IsNull(hint.GetDataNodes());
            sql = "/*!cobar:$partitionOperand=(['offer_id','group_id']=[123,'3c']),$table='offer'*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual((int)RouteResultsetNode.DefaultReplicaIndex, hint
                .GetReplica());
            pair = hint.GetPartitionOperand();
            Assert.AreEqual(2, pair.GetKey().Length);
            Assert.AreEqual("OFFER_ID", pair.GetKey()[0]);
            Assert.AreEqual("GROUP_ID", pair.GetKey()[1]);
            Assert.AreEqual(1, pair.GetValue().Length);
            Assert.AreEqual(2, pair.GetValue()[0].Length);
            Assert.AreEqual(123L, pair.GetValue()[0][0]);
            Assert.AreEqual("3c", pair.GetValue()[0][1]);
            Assert.IsNull(hint.GetDataNodes());
            sql = "/*!cobar:$partitionOperand=(['offer_id','group_id']=[[123,'3c'],[234,'food']]), $table='offer'*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual((int)RouteResultsetNode.DefaultReplicaIndex, hint
                .GetReplica());
            pair = hint.GetPartitionOperand();
            Assert.AreEqual(2, pair.GetKey().Length);
            Assert.AreEqual("OFFER_ID", pair.GetKey()[0]);
            Assert.AreEqual("GROUP_ID", pair.GetKey()[1]);
            Assert.AreEqual(2, pair.GetValue().Length);
            Assert.AreEqual(2, pair.GetValue()[0].Length);
            Assert.AreEqual(2, pair.GetValue()[1].Length);
            Assert.AreEqual(123L, pair.GetValue()[0][0]);
            Assert.AreEqual("3c", pair.GetValue()[0][1]);
            Assert.AreEqual(234L, pair.GetValue()[1][0]);
            Assert.AreEqual("food", pair.GetValue()[1][1]);
            Assert.IsNull(hint.GetDataNodes());
            sql = "/*!cobar:$partitionOperand= ( [ 'ofFER_id','groUp_id' ]= [ [123,'3c'],[ 234,'food']]  ), $table='offer'*/select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual("select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual((int)RouteResultsetNode.DefaultReplicaIndex, hint
                .GetReplica());
            pair = hint.GetPartitionOperand();
            Assert.AreEqual(2, pair.GetKey().Length);
            Assert.AreEqual("OFFER_ID", pair.GetKey()[0]);
            Assert.AreEqual("GROUP_ID", pair.GetKey()[1]);
            Assert.AreEqual(2, pair.GetValue().Length);
            Assert.AreEqual(2, pair.GetValue()[0].Length);
            Assert.AreEqual(2, pair.GetValue()[1].Length);
            Assert.AreEqual(123L, pair.GetValue()[0][0]);
            Assert.AreEqual("3c", pair.GetValue()[0][1]);
            Assert.AreEqual(234L, pair.GetValue()[1][0]);
            Assert.AreEqual("food", pair.GetValue()[1][1]);
            Assert.IsNull(hint.GetDataNodes());
            sql = "/*!cobar:$partitionOperand=(['offer_id']=[123,234]), $table='offer'*/ select * ";
            hint = CobarHint.ParserCobarHint(sql, 0);
            Assert.AreEqual(" select * ", hint.GetOutputSql());
            Assert.AreEqual("OFFER", hint.GetTable());
            Assert.AreEqual((int)RouteResultsetNode.DefaultReplicaIndex, hint
                .GetReplica());
            pair = hint.GetPartitionOperand();
            Assert.AreEqual(1, pair.GetKey().Length);
            Assert.AreEqual("OFFER_ID", pair.GetKey()[0]);
            Assert.AreEqual(2, pair.GetValue().Length);
            Assert.AreEqual(1, pair.GetValue()[0].Length);
            Assert.AreEqual(1, pair.GetValue()[1].Length);
            Assert.AreEqual(123L, pair.GetValue()[0][0]);
            Assert.AreEqual(234L, pair.GetValue()[1][0]);
            Assert.IsNull(hint.GetDataNodes());
        }
    }
}