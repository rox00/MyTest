import * as grpc from '@grpc/grpc-js';
import * as protoLoader from '@grpc/proto-loader';
import BNMSClientConfig from './BNMSClientConfig.js';

class BNMSClient {
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
            // 在这里执行异步操作
            this.client.GetStatus({ request: jsonrequest }, function (err, response) {
                console.log('GetStatus:', response.message);

                if (err) {
                    // 如果出现错误，调用 reject 函数，并传递错误原因作为参数
                    reject(err);
                } else {
                    // 如果操作成功，调用 resolve 函数，并传递操作结果作为参数
                    // var aa = response.message + Math.floor((Math.random()*10)+1)*1000;
                    resolve(response.message);
                }
            });
        }));
    }
}

var bnmsclient = new BNMSClient();
var reply = await bnmsclient.GetStatus('123456');
reply = '123456';


