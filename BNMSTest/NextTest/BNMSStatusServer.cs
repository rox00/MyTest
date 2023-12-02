using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BNMS;
using Grpc.Core;
using Newtonsoft.Json.Linq;

namespace NextTest
{
    enum BNMSStatusCode
    {
        Success=0,
        Error=1
    }
    internal class BNMSStatusServer:BNMSStatusSrv.BNMSStatusSrvBase
    {
        public static string GenResultJson(BNMSStatusCode code, string replyMessage)
        {
            JObject o = new JObject();
            o.Add("StatusCode", (int)code);
            o.Add("Message", replyMessage);
            return o.ToString();
        }
        public override Task<BNMSStatusReply> Login(BNMSStatusRequest request, ServerCallContext context)
        {
            return Task.FromResult(new BNMSStatusReply
            {
                Message = BNMSStatusServer.GenResultJson(BNMSStatusCode.Error, "not implement!!!")
            });
        }
        public override Task<BNMSStatusReply> GetStatus(BNMSStatusRequest request, ServerCallContext context)
        {
            JObject o = JObject.Parse(request.Request);
            if(o.ContainsKey("key"))
            {
                JArray array = (JArray)o["key"];
                string reply = GlobalStatus.Intance.getStatus(array);

                return Task.FromResult(new BNMSStatusReply
                {
                    Message = BNMSStatusServer.GenResultJson(BNMSStatusCode.Success, reply)
                });
            }

            return Task.FromResult(new BNMSStatusReply
            {
                Message = BNMSStatusServer.GenResultJson(BNMSStatusCode.Error, "Don't contains 'key' key")
            });
        }

        public override Task<BNMSStatusReply> SendStatus(BNMSStatusRequest request, ServerCallContext context)
        {
            JObject o = JObject.Parse(request.Request);
            if (o.ContainsKey("hostname"))
            {
                string hostname = (string)o["hostname"];
                GlobalStatus.Intance.addStatus(hostname, o);

                return Task.FromResult(new BNMSStatusReply
                {
                    Message = BNMSStatusServer.GenResultJson(BNMSStatusCode.Success, "sucess")
                });
            }

            return Task.FromResult(new BNMSStatusReply
            {
                Message = BNMSStatusServer.GenResultJson(BNMSStatusCode.Error, "Don't contains 'hostname' key")
            });
        }

        public override Task<BNMSStatusReply> RunCommand(BNMSStatusRequest request, ServerCallContext context)
        {
            //JObject o = JObject.Parse(request.Request);
            //if (o.ContainsKey("hostname"))
            //{
            //    string hostname = (string)o["hostname"];
            //    GlobalStatus.Intance.addStatus(hostname, o);

            //    return Task.FromResult(new BNMSStatusReply
            //    {
            //        Message = BNMSStatusServer.GenResultJson(BNMSStatusCode.Success, "sucess")
            //    });
            //}

            return Task.FromResult(new BNMSStatusReply
            {
                Message = BNMSStatusServer.GenResultJson(BNMSStatusCode.Error, "not implement!!!")
            });
        }
    }
}
