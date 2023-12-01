using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BNMS;
using Grpc.Core;

namespace NextTest
{
    internal class BNMSStatusServer:BNMSStatusSrv.BNMSStatusSrvBase
    {
        public override Task<BNMSStatusReply> GetStatus(BNMSStatusRequest request, ServerCallContext context)
        {

            return Task.FromResult(new BNMSStatusReply
            {
                Message = "sucess!! "
            }); ;
        }

        public override Task<BNMSStatusReply> SendStatus(BNMSStatusRequest request, ServerCallContext context)
        {
            return Task.FromResult(new BNMSStatusReply
            {
                Message = "sucess!! "
            }); ;
        }
    }
}
