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

namespace Tup.Cobar4Net.Config.Model
{
    /// <author>haiqing.zhuhq 2012-3-21</author>
    /// <author>xianmao.hexm</author>
    public sealed class CobarNodeConfig
    {
        public CobarNodeConfig(string name, string host, int port, int weight)
        {
            Name = name;
            Host = host;
            Port = port;
            Weight = weight;
        }

        public int Weight { get; set; }

        public int Port { get; set; }

        public string Host { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[CobarNodeConfig Name={0}, Host={1}, Port={2}, Weight={3}]",
                Name, Host, Port, Weight);
        }
    }
}