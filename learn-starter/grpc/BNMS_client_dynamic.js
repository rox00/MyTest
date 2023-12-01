import * as grpc from '@grpc/grpc-js';
import * as protoLoader from '@grpc/proto-loader';
import BNMSClientConfig from './BNMSClientConfig.js';
/** BNMSClient class
 * 1. get ngbt,kiosk,ntbc,cbgw status from BNMSServer
 * 2. send json command to BNMSServer for running powershell script
*/
export default class BNMSClient {
    constructor() {
        var config = new BNMSClientConfig();
        var packageDefinition = protoLoader.loadSync(
            config.protofile,
            {
                keepCase: true,
                longs: String,
                enums: String,
                defaults: true,
                oneofs: true
            });
        var bnms_proto = grpc.loadPackageDefinition(packageDefinition).BNMS;
        var target = config.servername + ':' + config.serverport;
        this.client = new bnms_proto.BNMSStatusSrv(target, grpc.credentials.createInsecure());
    }

    async GetStatus(jsonrequest) {
        return (await new Promise((resolve, reject) => {
            this.client.GetStatus({ request: jsonrequest }, function (err, response) {
                // console.log('GetStatus:', response.message);
                if (err) {
                    reject(err);
                } else {
                    resolve(response.message);
                }
            });
        }));
    }
}

//for test 
var bnmsclient = new BNMSClient();

var obj = { branchNo: "592"};
var jsonRequest = JSON.stringify(obj);
var reply = await bnmsclient.GetStatus(jsonRequest);
reply = '123456';


