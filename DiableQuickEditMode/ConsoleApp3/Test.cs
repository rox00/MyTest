using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public enum BCStatusKeyEnum
    {//just using the enum name as stringKey
        appl_state,
        line0,
        line1,
        line2,
        line3,
        LINE4,
    }
    internal class Test
    {
        ConcurrentDictionary<string, string> cur = new ConcurrentDictionary<string, string>();
        public Test()
        {
            cur[BCStatusKeyEnum.appl_state.ToString()] = "-1";
            cur[BCStatusKeyEnum.appl_state.ToString()] = "-1";
            cur[BCStatusKeyEnum.appl_state.ToString()] = "-1";
            foreach (var item in Enum.GetValues(typeof(BCStatusKeyEnum)))
            {
                cur[item.ToString()] = "-1";
            }
            //send
            string ret = JsonConvert.SerializeObject(cur);
        }
    }
}
