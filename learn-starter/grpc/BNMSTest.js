import BNMSClient,{BNMSStatusCode} from "./BNMS_client_dynamic.js";
import os from 'os'

//for test 
// while (true) 
{

    var bnmsclient = new BNMSClient();
    //********************for Login case*********************
    {
        var hostname = os.hostname();
        var requestObj = {
            username: 'beadmin',
            password: '0000abc!',
        }
        var jsonRequest = JSON.stringify(requestObj);
        var reply = await bnmsclient.Login(jsonRequest);
        var replyObj = JSON.parse(reply);
        if (replyObj.StatusCode == BNMSStatusCode.Success) {
            //for success process
            var message = replyObj.Message
            console.log(message);
        } else {
            //for error process
            var message = replyObj.Message
            console.log(message);
        }
    }


    //********************for SendStatus case*********************
    // {
    //     var hostname = os.hostname();
    //     var requestObj = {
    //         hostname: hostname,
    //         status1: 1,
    //         status2: -1,
    //         time:Date.now().toString()
    //     }
    //     var jsonRequest = JSON.stringify(requestObj);
    //     var reply = await bnmsclient.SendStatus(jsonRequest);
    //     var replyObj = JSON.parse(reply);
    //     if (replyObj.StatusCode == BNMSStatusCode.Success) {
    //         //for success process
    //         var message = replyObj.Message
    //         console.log(message);
    //     } else {
    //         //for error process
    //         var message = replyObj.Message
    //         console.log(message);
    //     }
    // }


    //**********************for GetStatus case************************
    // {
    //     var hostname = os.hostname();
    //     var requestObj = {
    //         key: [hostname,hostname, '888']
    //     }
    //     var jsonRequest = JSON.stringify(requestObj);
    //     var reply = await bnmsclient.GetStatus(jsonRequest);
    //     var replyObj = JSON.parse(reply);
    //     if (replyObj.StatusCode == BNMSStatusCode.Success) {
    //         //for success process
    //         var message = replyObj.Message
    //         console.log(message);
    //     } else {
    //         //for error process
    //         var message = replyObj.Message
    //         console.log(message);
    //     }
    // }

    //**********************for RunCommand case************************
    {
        var hostname = os.hostname();
        var requestObj = {
            command: 'reboot',
            target:[hostname,hostname]
        }
        var jsonRequest = JSON.stringify(requestObj);
        var reply = await bnmsclient.RunCommand(jsonRequest);
        var replyObj = JSON.parse(reply);
        if (replyObj.StatusCode == BNMSStatusCode.Success) {
            //for success process
            var message = replyObj.Message
            console.log(message);
        } else {
            //for error process
            var message = replyObj.Message
            console.log(message);
        }
    }
}
